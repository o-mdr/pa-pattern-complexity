using System.Configuration;
using System.IO;
using System.Linq;
using NUnit.Framework;
using PatternPA.Core.Extensions;
using PatternPA.Core.Interfaces.Factories;
using PatternPA.Core.Model;
using PatternPA.Infrastructure.Factories;
using PatternPA.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace PatternPA.Test.Integration.Nhanes.ArtificialGenerator
{
    [TestFixture]
    public class NhanesGroupTest : AbstractGroupTest
    {      
        IEnumerable<int> availableEvents = new[] { 0, 1, 2, 3, 4, 5 };
        int[] wordLenghts = { 3, 5, 10, 15, 30, 45, 60, 120, 180, 240, 300, 360, 420, 480, 540, 600, 660, 720 };
        private static Random r = new Random();
        private static string outputFolder = "Artificial";
        string testsBaseDir = ConfigurationManager.AppSettings["artificialBaseDir"];

        
        public static List<string> subDirsDataSource = new List<string>();

        public Group TestedGroup { get; set; }

        [SetUp]
        public void SetUp()
        {
            var subdirs = Directory.EnumerateDirectories(testsBaseDir);
            subDirsDataSource.AddRange(subdirs);            
        }

        //[Ignore]
        //[Test, Combinatorial]
        //[TestCase(1000, 0.01)]
        //public void GenerateAndSavePeopleInNhanesFormatTest(
        //    [Values(1000)] int populationSize, 
        //    [Range(0.1, 1, 0.1)] double degreeOfRandomness)
        //{
        //    string fOutName = nhanesGroupService.GenerateAndSavePeopleInNhanesFormat(
        //        populationSize, outputFolder, availableEvents, degreeOfRandomness);
        //}


        [Test]         
        public void SaveCompressionRatesGzipAll()
        {
            foreach (string from in subDirsDataSource)
            {
                var di = new DirectoryInfo(from);
                
                TestedGroup = nhanesGroupFactory.Create(from);
                Assert.IsNotNull(TestedGroup);

                foreach (int lenght in wordLenghts)
                {
                    var type = CompressionType.Gzip;
                    string fInName = GetFileName(lenght, type, di.Name);

                    string fOutName = nhanesGroupService.SaveCompressionRates(TestedGroup,
                                                                              fInName,
                                                                              type,
                                                                              lenght);
                }
            }
        }

        [Test]
        public void SaveCompressionRatesLZMAAll()
        {
            foreach (string from in subDirsDataSource)
            {
                var di = new DirectoryInfo(from);

                TestedGroup = nhanesGroupFactory.Create(from);
                Assert.IsNotNull(TestedGroup);
                foreach (int lenght in wordLenghts)
                {
                    var type = CompressionType.Lzma;
                    string fInName = GetFileName(lenght, type, di.Name);

                    string fOutName = nhanesGroupService.SaveCompressionRates(TestedGroup,
                                                                              fInName,
                                                                              type,
                                                                              lenght);
                }
            }
        }

        [Test]
        //[Ignore]
        public void SaveCompressionRatesPPMDAll()
        {
            foreach (string from in subDirsDataSource)
            {
                var di = new DirectoryInfo(from);
                TestedGroup = nhanesGroupFactory.Create(from);
                Assert.IsNotNull(TestedGroup);

                foreach (int lenght in wordLenghts)
                {
                    var type = CompressionType.Ppmd;
                    string fInName = GetFileName(lenght, type, di.Name);

                    string fOutName = nhanesGroupService.SaveCompressionRates(TestedGroup,
                                                                              fInName,
                                                                              type,
                                                                              lenght);
                }
            }
        }

        [Test]        
        public void SaveWordsCountTest()
        {
            foreach (string from in subDirsDataSource)
            {
                var di = new DirectoryInfo(from);
                TestedGroup = nhanesGroupFactory.Create(from);
                Assert.IsNotNull(TestedGroup);

                foreach (int lenght in wordLenghts)
                {
                    string fOut = GetWordsCountFileName(lenght, di.Name);
                    var fName = nhanesGroupService.SaveWordsCount(TestedGroup, lenght, fOut);

                    Assert.That(File.Exists(fName));
                    Assert.That(File.ReadAllBytes(fName).Length > 0);
                }
            }
        }

        private string GetFileName(int wordLenght, CompressionType type, string degreeOfRandomness)
        {
            string dir = "ArtificialComplexityMetrics\\" + degreeOfRandomness;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return String.Concat(dir + "\\CompressionRates.wordLength.",
                                 wordLenght,
                                 ".CompressedBy.",
                                 Enum.GetName(typeof(CompressionType), type),
                                 ".csv");
        }

        private string GetWordsCountFileName(int wordLenght, string degreeOfRandomness)
        {
            string dir = "ArtificialComplexityMetrics\\" + degreeOfRandomness;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return String.Concat(dir + "\\WordsCount.",
                                 wordLenght,                                 
                                 ".csv");
        }
    }
}
