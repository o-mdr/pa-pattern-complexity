using System.Linq;
using NUnit.Framework;
using PatternPA.Core.Extensions;

namespace PatternPA.Test.Core.Extension
{
    [TestFixture]
    public class StringSplitByTestFixture
    {
        [TestCase("abcabcabcabcabca", 1, 16)]
        [TestCase("abcabcabcabcabca", 2, 8)]
        [TestCase("abcabcabcabcabca", 3, 6)]
        [TestCase("abcabcabcabcabca", 4, 4)]
        [TestCase("abcabcabcabcabca", 5, 4)]
        [TestCase("abcabcabcabcabca", 6, 3)]
        [TestCase("abcabcabcabcabca", 7, 3)]
        [TestCase("abcabcabcabcabca", 8, 2)]
        [TestCase("abcabcabcabcabca", 9, 2)]
        [TestCase("abcabcabcabcabca", 10, 2)]
        [TestCase("abcabcabcabcabca", 11, 2)]
        [TestCase("abcabcabcabcabca", 12, 2)]
        [TestCase("abcabcabcabcabca", 13, 2)]
        [TestCase("abcabcabcabcabca", 14, 2)]
        [TestCase("abcabcabcabcabca", 15, 2)]
        [TestCase("abcabcabcabcabca", 16, 1)]
        [TestCase("abcabcabcabcabca", 17, 1)]
        [TestCase("abcabcabcabcabca", 512, 1)]
        public void SplitByTest(string str, int chunkLength, int expCount)
        {
            var words = str.SplitBy(chunkLength);
            int actual = words.Count();
            Assert.AreEqual(expCount, actual);
        }
    }
}
