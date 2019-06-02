using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using log4net;
using PatternPA.Container;
using PatternPA.Core.Interfaces;
using PatternPA.Core.Interfaces.FileOperation;
using PatternPA.Core.Interfaces.Nhanes;
using PatternPA.Core.Model;
using PatternPA.Core.Model.Nhanes;
using PatternPA.Utils;
using PatternPA.Utils._7zip;
using PatternPA.Utils.Extension;
using PatternPA.Utils.Huffman.Impl;
using PatternPA.Utils.Huffman.Interface;

namespace PatternPA.Infrastructure.Services
{
    public class NhanesService : AbstractService
    {
        protected INhanesCsvParser nhanesCsvParser;
        public const string PersonPamSuffix = ".Nhanes.csv";
        public const string PersonBinHuffmanSuffix = ".Nhanes.Huffman.bin";
        private readonly PersonCalculationService personCalcService = new PersonCalculationService();
        private static ILog log = LogManager.GetLogger("UnitTestLogger");
        private static ILog wordsLog = LogManager.GetLogger("WordsLogger");
        

        public NhanesService()
        {
            nhanesCsvParser = ContainerService.Instance.Resolve<INhanesCsvParser>();
        }

        /// <summary>
        /// For each person saves one .csv file with physical activity monitoring
        /// Each file follow this file name conversion
        /// [SubjectId].NhanesPAM.csv for example
        /// 12345.NhanesPAM.csv
        /// </summary>
        /// <param name="fromBulkCsvFolder">folder from where to read PAM bulk records</param>
        /// <param name="toPeopleCsvFolder">destination folder with multiple output files, one per person</param>
        public void SavePeoplePamAsSeparateCsvFiles(string fromBulkCsvFolder,
            string toPeopleCsvFolder)
        {
            if (!Directory.Exists(fromBulkCsvFolder))
                throw new DirectoryNotFoundException(fromBulkCsvFolder + " doesn't exist");

            if (!Directory.Exists(toPeopleCsvFolder))
                Directory.CreateDirectory(toPeopleCsvFolder);

            var fNames = Directory.EnumerateFiles(fromBulkCsvFolder, "*.csv");

            foreach (string fName in fNames)
            {
                SavePeoplePamAsSeparateCsvFile(fName, toPeopleCsvFolder);
            }
        }

        private void SavePeoplePamAsSeparateCsvFile(string fromBulkCsvFile, string ouputFolder)
        {
            //get bulk records
            var fileRecords = nhanesCsvParser.ParseCsv(fromBulkCsvFile);

            //get all unique people
            var ids = fileRecords.Select(r => r.SubjectId).Distinct();

            //for every unique person
            foreach (int id in ids)
            {
                //set file name
                string fName = Path.Combine(ouputFolder, id + PersonPamSuffix);
                //check if exist, bulk files can have one person split between 2 bulk files
                bool fExist = File.Exists(fName);
                //get records for the person
                int localCopyId = id;
                var personRecords = fileRecords.Where(r => r.SubjectId == localCopyId);

                //save to new file or append to existing
                using (var writer = new StreamWriter(fName, fExist))
                {
                    foreach (var record in personRecords)
                    {
                        writer.Write(record + Environment.NewLine);
                    }
                }
            }
        }

        public BitArray GetHuffmanEncodedSequence(Person p, int wordLengh)
        {
            IHuffmanFixedLengthCoding c = new HuffmanFixedLengthCoding();
            string sequence = p.GetNhanceSequenceString();

            c.Build(sequence, wordLengh);
            var encoded = c.Encode();

            string traceStr = "Person " + p.Id + " has " + c.WordsProbability.Count + " words";

            log.Debug(traceStr);

            wordsLog.Debug("Person " + p.Id);
            c.TraceWords(wordsLog);
            encoded.TraceEncoded(wordsLog);
            wordsLog.Debug("==========*******===========");
            wordsLog.Debug("==========*******===========");
            wordsLog.Debug("==========*******===========");

            p.WordsProbability = c.WordsProbability;

            return encoded;
        }

        public string GetWordsProbabilitiesAsString(Person p, int wordLengh)
        {
            IHuffmanFixedLengthCoding c = new HuffmanFixedLengthCoding();
            string sequence = p.GetNhanceSequenceString();
            c.Build(sequence, wordLengh);
            log.Debug(p.Id + ". Total numer of unique words in sequence: " + c.WordsProbability.Count);
            return c.GetWordsAsSingleString();
        }

        public IEnumerable<string> GetSequenceAsSplitedWords(Person p, int wordLengh)
        {
            string sequence = p.GetNhanceSequenceString();

            IWordProbability wp = new WordProbability();
            var words = wp.MapToWords(sequence, wordLengh);

            string traceStr = "Person " + p.Id + " has " + words.Count() + " words";

            log.Debug(traceStr);

            return words;
        }

        public string SaveSequenceAsSplitedWords(Person p, int wordLengh, string fName = "")
        {
            var words = GetSequenceAsSplitedWords(p, wordLengh);

            if (String.IsNullOrEmpty(fName))
            {
                string dir = "Words";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                fName = String.Concat(dir + "\\" + p.Id +  ".wordLength.",
                                      wordLengh,
                                      ".txt");
            }
            
            string savableString = words.Aggregate((workingSentence, next) =>
                next + " " + workingSentence);
            //save
            ICsvFileWriter writer = new CsvFileWriter(fName);
            writer.WriteData(savableString, fName, true);

            return fName;
        }

        public IDictionary<string, decimal> GetWordsProbability(Person p, int wordLengh)
        {
            IHuffmanFixedLengthCoding c = new HuffmanFixedLengthCoding();
            string sequence = p.GetNhanceSequenceString();

            c.Build(sequence, wordLengh);
            return c.WordsProbability;
        }

        public IDictionary<string, decimal> GetCanonicalWordsProbability(Person p, int wordLength)
        {
            IHuffmanFixedLengthCoding c = new HuffmanFixedLengthCoding();
            string sequence = p.GetNhanceSequenceString();
            return c.CalculateCanonicalWordsProbability(sequence, wordLength);
        }

        public string SaveHuffmanEncodedSequence(Person p, int wordLengh, string fName = "")
        {
            if (String.IsNullOrEmpty(fName))
            {
                string dir = "Huffman";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                fName = String.Concat(dir + "\\" + p.Id, ".wordLength.", wordLengh, PersonBinHuffmanSuffix);
            }
            var bits = GetHuffmanEncodedSequence(p, wordLengh);
            if (bits.Count == 0)
                throw new InvalidOperationException(
                    "Sequence cannot be processed, because the data is constructed from one word. " +
                    "Person id: " + p.Id +
                    ". Check this person file: " + p.DataFilePath);

            IBinaryFileWriter writer = new BinaryFileWriter();
            writer.Write(fName, FileMode.OpenOrCreate, bits);
            return fName;
        }

        public decimal GetCompressionRate(Person p, int wordLength, CompressionType type)
        {
            string fUncompressed = SaveHuffmanEncodedSequence(p, wordLength);
            IArchiver archiver = GetArchiver(type);

            var fiToBeCompressed = new FileInfo(fUncompressed);
            string fCompressed = archiver.Compress(fiToBeCompressed);
            var fiCompressed = new FileInfo(fCompressed);

            //compression rate
            if (fiCompressed.Length == 0)
                throw new ArgumentException();

            decimal rate = (decimal)fiToBeCompressed.Length / fiCompressed.Length;


            //remove the files
            fiToBeCompressed.Delete();
            fiCompressed.Delete();

            return rate;
        }

        public string SaveCompressionRate(Person p, int wordLength,
            CompressionType type, string fName = "")
        {
            decimal rate = GetCompressionRate(p, wordLength, type);
            if (String.IsNullOrEmpty(fName))
            {
                string dir = "Huffman";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                fName = String.Concat(dir + "\\" + p.Id, ".wordLength.",
                                      wordLength,
                                      ".CompressedBy.",
                                      Enum.GetName(typeof(CompressionType), type),
                                      ".csv");
            }

            string txt = String.Format("{0},{1}{2}", p.Id, rate, Environment.NewLine);
            ICsvFileWriter writer = new CsvFileWriter(fName);
            writer.WriteData(txt, fName, true);
            return fName;
        }

        public Dictionary<int, double> GetAutoCorrelation(Person p)
        {
            var data = from r in p.NhanesRecords
                       select r.IntencityCode;

            return personCalcService.InternalGetAutoCorrelationsForAllShifts(data);
        }


        private IArchiver GetArchiver(CompressionType type)
        {
            IArchiver compressor;
            switch (type)
            {
                case CompressionType.Gzip:
                    {
                        compressor = new GZipArchiver();
                        break;
                    }
                case CompressionType.Lzma:
                    {
                        compressor = new LZMAArchiver();
                        break;
                    }
                case CompressionType.Ppmd:
                    {
                        compressor = new PPMDArchiver();
                        break;
                    }
                default:
                    throw new NotSupportedException();
            }

            return compressor;
        }


        public string SaveNhanesPersonActivityStats(Person p, string fName = null)
        {
            if (String.IsNullOrEmpty(fName))
            {
                string dir = "Nhanes";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                fName = String.Concat(dir + "\\" + p.Id, ".Nhanes.Stats.csv");
            }

            //get all unique people
            var g = new NhanesPersonActivityStatsGenerator();
            var stats = g.Generate(p.NhanesRecords);

            //check if exist, bulk files can have one person split between 2 bulk files
            bool fExist = File.Exists(fName);


            //save to new file or append to existing
            using (var writer = new StreamWriter(fName, fExist))
            {
                writer.Write(p.Id + "," + stats + Environment.NewLine);
            }

            return fName;
        }

        
    }
}