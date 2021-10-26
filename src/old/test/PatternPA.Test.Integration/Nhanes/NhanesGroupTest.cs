using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using NUnit.Framework;
using PatternPA.Core.Model;
using PatternPA.Infrastructure.Services;

namespace PatternPA.Test.Integration.Nhanes
{
    [TestFixture]
    public class NhanesGroupTest : AbstractGroupTest
    {
        string from = ConfigurationManager.AppSettings["subsampleNhanesFloder"];

        [Test]
        public void InitGroup()
        {
            var group = nhanesGroupFactory.Create(from);

            Assert.AreEqual(10, group.People.ToList().Count);
        }

        [Test]
        public void SaveCompressionRatesGzipAll()
        {
            var group = nhanesGroupFactory.Create(from);

            int wordLenght = 5;
            var type = CompressionType.Gzip;
            string fInName = GetFileName(wordLenght, type);

            string fOutName = nhanesGroupService.SaveCompressionRates(group,
                fInName,
                type,
                wordLenght);

            Assert.AreEqual(10, group.People.ToList().Count);
            Assert.That(File.Exists(fOutName));
            Assert.That(File.ReadAllBytes(fOutName).Length > 0);
        }

        [Test]
        public void SaveCompressionRatesLZMAAll()
        {
            var group = nhanesGroupFactory.Create(from);

            int wordLenght = 1;
            var type = CompressionType.Lzma;
            string fInName = GetFileName(wordLenght, type);

            string fOutName = nhanesGroupService.SaveCompressionRates(group,
                fInName,
                type,
                wordLenght);

            Assert.AreEqual(10, group.People.ToList().Count);
            Assert.That(File.Exists(fOutName));
            Assert.That(File.ReadAllBytes(fOutName).Length > 0);
        }

        [Test]
        public void SaveCompressionRatesPPMDAll()
        {
            var group = nhanesGroupFactory.Create(from);

            int wordLenght = 1;
            var type = CompressionType.Ppmd;
            string fInName = GetFileName(wordLenght, type);

            string fOutName = nhanesGroupService.SaveCompressionRates(group,
                fInName,
                type,
                wordLenght);

            Assert.AreEqual(10, group.People.ToList().Count);
            Assert.That(File.Exists(fOutName));
            Assert.That(File.ReadAllBytes(fOutName).Length > 0);
        }

        [Test]
        public void JoinWordsProbabilitiesTest()
        {
            var left = new Dictionary<string, decimal>
                            {
                                {"000", 0.4m},
                                {"111", 0.3m},
                                {"222", 0.2m},
                                {"220", 0.05m},
                                {"221", 0.05m}
                            };
            var right = new Dictionary<string, decimal>
                            {
                                {"000", 0.6m},
                                {"111", 0.1m},
                                {"222", 0.1m},
                                {"120", 0.15m},
                                {"211", 0.05m}
                            };
            var expected = new Dictionary<string, decimal>
                            {
                                {"000", 0.5m},
                                {"111", 0.2m},
                                {"222", 0.15m}
                            };

            var gs = new NhanesGroupService();
            var actual = gs.JoinWordsProbabilities(left, right);
            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void CalculateCommonWordsProbabilitiesTest()
        {
            int wordLenght = 5;
            var group = nhanesGroupFactory.Create(from);
            var result = nhanesGroupService.CalculateCommonWordsProbabilities(group, wordLenght);

            Assert.AreEqual(10, group.People.ToList().Count);
            Assert.AreEqual(22, result.Count);
            Assert.AreEqual(0.6394050228340983714321708726m, result.Values.Sum());
        }

        [Test]
        public void SaveCommonWordsProbabilitiesTest()
        {
            int wordLenght = 10;
            var group = nhanesGroupFactory.Create(from);
            var fName = nhanesGroupService.SaveCommonWordsProbabilities(group, wordLenght);

            Assert.AreEqual(10, group.People.ToList().Count);
            Assert.That(File.Exists(fName));
            Assert.That(File.ReadAllBytes(fName).Length > 0);
        }

        [Test]
        public void SaveNhanesGroupActivityStatsTest()
        {
            var group = nhanesGroupFactory.Create(from);
            var fName = nhanesGroupService.SaveNhanesGroupActivityStats(group);
            Assert.That(File.Exists(fName));
            Assert.That(File.ReadAllBytes(fName).Length > 0);
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