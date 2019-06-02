using System;
using System.Collections;
using System.IO;
using System.Linq;
using NUnit.Framework;
using PatternPA.Core.Model;

using PatternPA.Utils.Huffman.Impl;
using PatternPA.Utils.Huffman.Interface;

namespace PatternPA.Test.Integration.Nhanes.LongRunningManualTests
{
    [TestFixture]
    public class NhanesGroupTest : AbstractGroupTest
    {
        //string from = ConfigurationManager.AppSettings["longRunningNhanesData"];
        //int wordLenght = 3;
        //int[] wordLenghts = { 5, 10, 15, 30, 45, 60 };
        //int[] wordLenghts = { 3, 5, 10, 15, 30, 45, 60, 120, 180, 240, 300, 360, 420, 480, 540, 600, 660, 720 };
        //int[] wordLenghts = { 3, 5, 660, 720 };
        //int[] wordLenghts = { 3, 5, 10, 15, 30};
        //int[] wordLenghts = { 60 };
        int[] wordLenghts = { 4 };
        //int[] wordLenghts = { 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        public static Group TestedGroup { get; set; }

        //[Test]
        //[Ignore]
        //public void InitGroup()
        //{
        //    Assert.That(TestedGroup.People.ToList().Count > 0);
        //}


        [Test]
        public void SaveCompressionRatesGzipAll()
        {
            foreach (int lenght in wordLenghts)
            {
                var type = CompressionType.Gzip;
                string fInName = GetFileName(lenght, type);

                string fOutName = nhanesGroupService.SaveCompressionRates(TestedGroup,
                                                                          fInName,
                                                                          type,
                                                                          lenght);
            }
        }

        [Test]
        public void SaveCompressionRatesLZMAAll()
        {
            foreach (int lenght in wordLenghts)
            {
                var type = CompressionType.Lzma;
                string fInName = GetFileName(lenght, type);

                string fOutName = nhanesGroupService.SaveCompressionRates(TestedGroup,
                                                                          fInName,
                                                                          type,
                                                                          lenght);
            }
        }

        [Test]
        public void SaveCompressionRatesPPMDAll()
        {
            foreach (int lenght in wordLenghts)
            {
                var type = CompressionType.Ppmd;
                string fInName = GetFileName(lenght, type);

                string fOutName = nhanesGroupService.SaveCompressionRates(TestedGroup,
                                                                          fInName,
                                                                          type,
                                                                          lenght);
            }
        }

        //[Test]
        //[Ignore]
        //public void SaveWords()
        //{
        //    foreach (int lenght in wordLenghts)
        //    {
        //        var result = nhanesGroupService.SaveSequenceAsSplitedWords(
        //            TestedGroup, lenght);

        //        foreach (var fName in result)
        //        {
        //            Assert.That(File.Exists(fName));
        //            Assert.That(File.ReadAllBytes(fName).Length > 0);
        //        }
        //    }
        //}


        //[Test]
        //[Ignore]
        //public void DeleteFilesWithAllSedentaryData()
        //{
        //    Assert.That(TestedGroup.People.ToList().Count > 0);

        //    string fName = "DeletedIdWithAllSedentarydata.txt";

        //    foreach (var person in TestedGroup.People)
        //    {
        //        var bits = GetHuffmanEncodedSequence(person, 1);
        //        if (bits.Count == 0)
        //        {
        //            File.AppendAllText(fName, person.Id + ", "
        //                + person.DataFilePath
        //                + Environment.NewLine);
        //            File.Delete(person.DataFilePath);
        //        }
        //    }
        //}

        //[Ignore]
        ////[TestCase(3)]
        ////[TestCase(5)]
        ////[TestCase(10)]
        ////[TestCase(15)]
        //public void SaveCommonWordsProbabilitiesTest(int testingWordLenght)
        //{

        //    var fName = nhanesGroupService.SaveCommonWordsProbabilities(TestedGroup, testingWordLenght);

        //    Assert.That(File.Exists(fName));
        //    Assert.That(File.ReadAllBytes(fName).Length > 0);
        //}

        [Test]
        //[Ignore]
        public void SaveWordsCountTest()
        {
            foreach (int lenght in wordLenghts)
            {
                nhanesGroupService.SaveWordsProbabilitiesAsString(TestedGroup, lenght);
                var fName = nhanesGroupService.SaveWordsCount(TestedGroup, lenght);

                Assert.That(File.Exists(fName));
                Assert.That(File.ReadAllBytes(fName).Length > 0);
            }
        }

        [Test]
        //[Ignore]
        public void SaveCanonicalWordsCountTest()
        {
            foreach (int lenght in wordLenghts)
            {
                var fName = nhanesGroupService.SaveCanonicalWordsCount(TestedGroup, lenght);

                Assert.That(File.Exists(fName));
                Assert.That(File.ReadAllBytes(fName).Length > 0);
            }
        }

        //[Test]
        ////[Ignore]
        //public void SaveNhanesGroupActivityStatsTest()
        //{
        //    var fName = nhanesGroupService.SaveNhanesGroupActivityStats(TestedGroup);
        //    Assert.That(File.Exists(fName));
        //    Assert.That(File.ReadAllBytes(fName).Length > 0);
        //}

        //[TestCase(10)]
        //[Ignore]
        //public void SaveNhanesGroupWordsTest(int wordLength)
        //{
        //    var result = nhanesGroupService.SaveWordsProbabilitiesAsString(TestedGroup, wordLength);
        //    foreach (var fName in result)
        //    {
        //        Assert.That(File.Exists(fName));
        //        Assert.That(File.ReadAllBytes(fName).Length > 0);
        //    }
        //}

        private BitArray GetHuffmanEncodedSequence(Person p, int wordLengh)
        {
            IHuffmanFixedLengthCoding c = new HuffmanFixedLengthCoding();
            string sequence = p.GetNhanceSequenceString();
            c.Build(sequence, wordLengh);
            var encoded = c.Encode();
            return encoded;
        }



        private string GetFileName(int wordLenght, CompressionType type)
        {
            string dir = "Huffman";
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
    }
}