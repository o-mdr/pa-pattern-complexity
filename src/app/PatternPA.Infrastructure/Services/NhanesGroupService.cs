using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatternPA.Core.Interfaces.Factories;
using PatternPA.Core.Interfaces.FileOperation;
using PatternPA.Core.Model;
using PatternPA.Core.Model.Nhanes;
using PatternPA.Infrastructure.Factories;
using PatternPA.Utils;
using PatternPA.Utils.Extension;
using PatternPA.Utils.Huffman.Impl;
using PatternPA.Utils.Huffman.Interface;

namespace PatternPA.Infrastructure.Services
{
    public class NhanesGroupService : AbstractService
    {
        private readonly IPersonFactory personFactory = new NhanesPersonFactory();
        private readonly NhanesService nhanesService = new NhanesService();
        private readonly NhanesPersonService nhanesPersonService = new NhanesPersonService();
        
        private static Random r = new Random();

        public void Initialize(Group group)
        {
            var di = new DirectoryInfo(group.DataFolderPath);
            FileInfo[] fileInfos = di.GetFiles();

            if (group.People != null)
                return;

            group.People = new List<Person>(fileInfos.Length);

            group.People = fileInfos.Select(
                fileInfo =>
                {
                    var p = personFactory.Create(fileInfo.FullName);
                    return p;
                });
        }

        public string SaveCompressionRates(Group group, string toFile,
            CompressionType type, int wordLenght)
        {
            //Parallel.ForEach(group.People,
            //                 person =>
            //                 {
            //                     nhanesService.SaveCompressionRate(person, wordLenght, type, toFile);
            //                     //person.Dispose();
            //                 });
            
            foreach (Person person in group.People)
            {
                nhanesService.SaveCompressionRate(person, wordLenght, type, toFile);
            }

            return toFile;
        }

        public string GenerateAndSavePeopleInNhanesFormat(int populationSize, 
            string outputFolder,
            IEnumerable<int> availableEvents,
            double degreeOfRandomness)
        {
            outputFolder = outputFolder + "\\DegreeOfRandomness_" + degreeOfRandomness;

            for (int i = 0; i < populationSize; i++)
            {
                int length = r.Next(7200, 14400);
                Person p = nhanesPersonService.GenerateInNhanes2003Format(
                    availableEvents,
                    length,
                    degreeOfRandomness);
                                
                nhanesPersonService.SavePersonInNhanesFormat(p, degreeOfRandomness, outputFolder);    
            }            

            return outputFolder;
        }

        public IDictionary<string, decimal> CalculateCommonWordsProbabilities(Group group, int wordLength)
        {
            IDictionary<string, decimal> current = null;
            foreach (Person person in group.People)
            {
                nhanesService.GetHuffmanEncodedSequence(person, wordLength);

                //first init
                if (current == null)
                {
                    current = person.WordsProbability;
                    continue;
                }

                current = JoinWordsProbabilities(current, person.WordsProbability);
            }

            return current;
        }

        public IDictionary<object, int> CalculateWordsCount(Group group, int wordLength)
        {
            IDictionary<object, int> current = new Dictionary<object, int>();
            foreach (Person person in group.People)
            {
                var words = nhanesService.GetWordsProbability(person, wordLength);
                current.Add(person.Id, words.Count);
            }

            return current;
        }

        private IDictionary<object, int> CalculateCanonicalWordsCount(Group group, int wordLength)
        {
            IDictionary<object, int> current = new Dictionary<object, int>();
            foreach (Person person in group.People)
            {
                var words = nhanesService.GetCanonicalWordsProbability(person, wordLength);
                current.Add(person.Id, words.Count);
            }

            return current;
        }

        //selects common words and sums and divides probabilities on 2
        public IDictionary<string, decimal> JoinWordsProbabilities(
            IDictionary<string, decimal> left, IDictionary<string, decimal> right)
        {
            return left.Join(right,
                l => l.Key,
                r => r.Key, (l, r) => new { l.Key, Value = (l.Value + r.Value) / 2 })
                .OrderByDescending(t => t.Value)
                .ToDictionary(t => t.Key, t => t.Value);
        }

        public string SaveCommonWordsProbabilities(Group group, int wordLength, string fName = "")
        {
            if (String.IsNullOrEmpty(fName))
            {
                string dir = "Huffman";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                fName = String.Concat(dir + "\\" + "Group.wordLength.",
                                      wordLength,
                                      ".CommonVocabulary.csv");
            }

            var commonWordsProbabilities = CalculateCommonWordsProbabilities(group, wordLength);

            //save
            ICsvFileWriter writer = new CsvFileWriter(fName);
            string header = String.Format("{0} common words that present {1} probability {2}",
                commonWordsProbabilities.Count, commonWordsProbabilities.Values.Sum(),
                Environment.NewLine);
            writer.WriteData(header, fName, true);

            string wordsProbability = ToSaveableString(commonWordsProbabilities);
            writer.WriteData(wordsProbability, fName, true);

            return fName;
        }

        public IEnumerable<string> SaveSequenceAsSplitedWords(
            Group group, int wordLength, string fName = "")
        {
            foreach (Person person in group.People)
            {
                if (String.IsNullOrEmpty(fName))
                {
                    string dir = "Words";
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    fName = String.Concat(dir + "\\" + person.Id +
                                            ".Group.wordLength.",
                                            wordLength,
                                            ".txt");
                }

                nhanesService.SaveSequenceAsSplitedWords(person, wordLength, fName);

                yield return fName;
            }
        }

        public string SaveWordsCount(Group group, int wordLength, string fName = "")
        {
            if (String.IsNullOrEmpty(fName))
            {
                string dir = "Huffman";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                fName = String.Concat(dir + "\\" + "Group.wordLength.",
                                      wordLength,
                                      ".WordsCount.csv");
            }

            var commonWordsCount = CalculateWordsCount(group, wordLength);

            //save
            ICsvFileWriter writer = new CsvFileWriter(fName);
            string wordsProbability = ToSaveableString(commonWordsCount);
            writer.WriteData(wordsProbability, fName, true);

            return fName;
        }

        public string SaveCanonicalWordsCount(Group group, int wordLength, string fName = "")
        {
            if (String.IsNullOrEmpty(fName))
            {
                string dir = "Huffman";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                fName = String.Concat(dir + "\\" + "Group.wordLength.",
                                      wordLength,
                                      ".CanonicalWordsCount.csv");
            }

            var commonWordsCount = CalculateCanonicalWordsCount(group, wordLength);

            //save
            ICsvFileWriter writer = new CsvFileWriter(fName);
            string wordsProbability = ToSaveableString(commonWordsCount);
            writer.WriteData(wordsProbability, fName, true);

            return fName;
        }

        

        private string ToSaveableString<TKey, TValue>(IDictionary<TKey, TValue> commonWordsProbabilities)
        {
            var sb = new StringBuilder();
            foreach (var entry in commonWordsProbabilities)
            {
                sb.Append(entry.Key);
                sb.Append(",");
                sb.Append(entry.Value);
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        public IEnumerable<string> SaveWordsProbabilitiesAsString(Group group,
            int wordLengh, string fName = "")
        {
            var list = new List<string>();
            foreach (Person person in group.People)
            {
                if (String.IsNullOrEmpty(fName))
                {
                    string dir = "WordsPresentation";
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    fName = String.Concat(dir + "\\" +
                        person.Id + "." +
                        wordLengh + ".Nhanes.Words.csv");
                }

                string words = nhanesService.GetWordsProbabilitiesAsString(person, wordLengh);

                using (var writer = new StreamWriter(fName))
                {
                    writer.Write(words);
                }

                list.Add(fName);
                fName = String.Empty;
            }

            return list;
        }

        public string SaveNhanesGroupActivityStats(Group group, string fName = null)
        {
            if (String.IsNullOrEmpty(fName))
            {
                string dir = "Nhanes";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                fName = String.Concat(dir + "\\" + "Nhanes.Group.Stats.csv");
            }

            //write helping header file
            string fHeader = fName.Replace(".csv", ".Header.csv");
            using (var writer = new StreamWriter(fHeader))
            {
                writer.Write("PersonId, " +
                    NhanesPersonActivityStats.GetCsvHeader() +
                    Environment.NewLine);
            }

            int i = 0;
            foreach (var person in group.People)
            {
                nhanesService.SaveNhanesPersonActivityStats(person, fName);

                if (i % 1000 == 0)
                    log.Info("Iteration: " + i);
                i++;
            }

            return fName;
        }
    }
}