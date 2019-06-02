using System.Linq;
using NUnit.Framework;
using PatternPA.Utils.Huffman.Impl;
using PatternPA.Utils.Huffman.Interface;

namespace PatternPA.Test.Utils.Huffman
{
    [TestFixture]
    public class WordProbabilityTest
    {
        [TestCase("0010000200222", 1, 13)]
        [TestCase("0010000200222", 2, 7)]
        [TestCase("0010000200222", 4, 4)]
        [TestCase("0010000200222", 7, 2)]
        [TestCase("0010000200222", 9, 2)]
        [TestCase("0010000200222", 12, 2)]
        [TestCase("0010000200222", 13, 1)]
        [TestCase("0010000200222", 14, 1)]
        [TestCase("0010000200222", 28, 1)]
        public void MapToWordsTest(string sequence, int wordSize, int expectedCount)
        {
            IWordProbability wp = new WordProbability();
            var map = wp.MapToWords(sequence, wordSize);
            Assert.AreEqual(expectedCount, map.Count());
        }

        [Test]
        public void ReduceToWordsProbabilityTest()
        {
            IWordProbability wp = new WordProbability();
            var map = wp.MapToWords("0010000200222", 2);
            var result = wp.ReduceToWordsProbability(map);

            Assert.AreEqual(5, result.Count);
        }
    }
}