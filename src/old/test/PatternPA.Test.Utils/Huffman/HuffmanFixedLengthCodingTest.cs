using System;
using System.IO;
using NUnit.Framework;
using PatternPA.Core.Extensions;
using PatternPA.Utils.Extension;
using PatternPA.Utils.Huffman.Impl;
using PatternPA.Utils.Huffman.Interface;

namespace PatternPA.Test.Utils.Huffman
{
    [TestFixture]
    public class HuffmanFixedLengthCodingTest
    {
        [TestCase("001101010110101010101022212121212222", 2, "1000100110110110111111111111100010101010000")]
        public void EncodeTest(string sequence, int wordLenth, string expected)
        {
            IHuffmanFixedLengthCoding c = new HuffmanFixedLengthCoding();
            c.Build(sequence, wordLenth);
            var actual = c.Encode();
            
            c.TraceWords();
            actual.TraceEncoded();

            Assert.AreEqual(expected, actual.ToBitString());
        }

        [TestCase("001101010110101010101022212121212222", 2)]
        [TestCase("001101010110101010101022212121212222", 3)]
        [TestCase("001101010110101010101022212121212222", 4)]
        [TestCase("001101010110101010101022212121212222", 5)]
        [TestCase("001101010110101010101022212121212222", 6)]
        [TestCase("001101010110101010101022212121212222", 7)]
        public void EncodeDecodeTest(string sequence, int wordLenth)
        {
            IHuffmanFixedLengthCoding c = new HuffmanFixedLengthCoding();
            c.Build(sequence, wordLenth);
            var encoded = c.Encode();
            c.TraceWords();

            encoded.TraceEncoded();

            string actual = c.Decode(encoded);
            Assert.AreEqual(sequence, actual);
        }

        //[TestCase("Data\\AnnaKarenina.txt", 10)]//this takes ages...
        [TestCase("Data\\AnnaKarenina.Chapter1.txt", 10)]
        //[TestCase("Data\\AnnaKarenina.Chapter1.txt", 6)]
        //[TestCase("Data\\AnnaKarenina.Chapter1.txt", 3)]
        public void EncodeDecodeFromFileTest(string path, int wordLenth)
        {
            string content = File.ReadAllText(path);

            IHuffmanFixedLengthCoding c = new HuffmanFixedLengthCoding();
            c.Build(content, wordLenth);
            var encoded = c.Encode();

            c.TraceWords();
            encoded.TraceEncoded();
            
            string actual = c.Decode(encoded);
            Assert.AreEqual(content, actual);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EncodeWithoutBuildingAtreeThrowsException()
        {
            IHuffmanFixedLengthCoding c = new HuffmanFixedLengthCoding();
            c.Encode();
            Assert.Fail("Should not come here");
        }
    }
}