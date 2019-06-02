using System.Configuration;
using System.IO;
using System.Linq;
using NUnit.Framework;
using PatternPA.Core.Extensions;
using PatternPA.Core.Interfaces.Factories;
using PatternPA.Core.Model;
using PatternPA.Infrastructure.Factories;
using PatternPA.Infrastructure.Services;

namespace PatternPA.Test.Integration.Nhanes
{
    [TestFixture]
    public class NhanesPersonTest : AbstractIntegrationTest
    {
        private readonly string subjectFile = ConfigurationManager.AppSettings["sampleNhanesFilePath"];
        private readonly IPersonFactory personFactory = new NhanesPersonFactory();
        private readonly NhanesService nhanesService = new NhanesService();

        [Test]
        public void InitializeTest()
        {
            var person = personFactory.Create(subjectFile);
            Assert.AreEqual("21015", person.Id);
            Assert.AreEqual(5416, person.NhanesRecords.Count());
        }

        [Test]
        public void ToNhanceSequenceStringTest()
        {
            var person = personFactory.Create(subjectFile);
            string s = person.GetNhanceSequenceString();
            Assert.AreEqual(5416, s.Length);
        }

        [TestCase(1)]
        [TestCase(30)]
        [TestCase(60)]
        public void ApplyHuffmanCodding(int wordLength)
        {
            var person = personFactory.Create(subjectFile);
            var bits = nhanesService.GetHuffmanEncodedSequence(person, wordLength);
            Assert.IsNotNullOrEmpty(bits.ToBitString());
        }

        //[TestCase(5)]
        [TestCase(10)]
        public void SaveWords(int wordLength)
        {
            var person = personFactory.Create(subjectFile);
            var fName = nhanesService.SaveSequenceAsSplitedWords(person, wordLength);
            Assert.That(File.Exists(fName));
            Assert.That(File.ReadAllBytes(fName).Length > 0);
        }

        //[TestCase(30)]
        [TestCase(4)]
        public void SaveHuffmanEncodedSequence(int wordLength)
        {
            var person = personFactory.Create(subjectFile);
            string fName = nhanesService.SaveHuffmanEncodedSequence(person, wordLength);
            Assert.That(File.Exists(fName));
            Assert.That(File.ReadAllBytes(fName).Length > 0);
        }

        [Test]
        public void SaveNhanesPersonActivityStatsTest()
        {
            var person = personFactory.Create(subjectFile);
            string fName = nhanesService.SaveNhanesPersonActivityStats(person);
            Assert.That(File.Exists(fName));
            Assert.That(File.ReadAllBytes(fName).Length > 0);
        }

        [Test]
        public void GetCompressionRate_GZIP()
        {
            decimal expected = 5.7344461305007587253414264036m;
            int wordLength = 1;
            var person = personFactory.Create(subjectFile);
            decimal actual = nhanesService.GetCompressionRate(person, wordLength, CompressionType.Gzip);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SaveCompressionRate_GZIP()
        {
            int wordLength = 1;
            var person = personFactory.Create(subjectFile);
            string fName = nhanesService.SaveCompressionRate(person, wordLength, CompressionType.Gzip);
            Assert.That(File.Exists(fName));
            Assert.That(File.ReadAllBytes(fName).Length > 0);
        }

        [Test]
        public void GetCompressionRate_LZMA()
        {
            decimal expected = 8.482603815937149270482603816m;
            int wordLength = 1;
            var person = personFactory.Create(subjectFile);
            decimal actual = nhanesService.GetCompressionRate(person, wordLength, CompressionType.Lzma);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SaveCompressionRate_LZMA()
        {
            int wordLength = 1;
            var person = personFactory.Create(subjectFile);
            string fName = nhanesService.SaveCompressionRate(person, wordLength, CompressionType.Lzma);
            Assert.That(File.Exists(fName));
            Assert.That(File.ReadAllBytes(fName).Length > 0);
        }

        [Test]
        public void GetCompressionRate_PPMD()
        {
            decimal expected = 9.06235011990407673860911271m;
            int wordLength = 1;
            var person = personFactory.Create(subjectFile);
            decimal actual = nhanesService.GetCompressionRate(person, wordLength, CompressionType.Ppmd);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SaveCompressionRate_PPMD()
        {
            int wordLength = 1;
            var person = personFactory.Create(subjectFile);
            string fName = nhanesService.SaveCompressionRate(person, wordLength, CompressionType.Ppmd);
            Assert.That(File.Exists(fName));
            Assert.That(File.ReadAllBytes(fName).Length > 0);
        }

        [Test]
        public void GetAutocorrelation()
        {
            var person = personFactory.Create(subjectFile);
            var result = nhanesService.GetAutoCorrelation(person);
            Assert.AreEqual(2708, result.Count);
        }
    }
}