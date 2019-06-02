using NUnit.Framework;
using PatternPA.Utils;

namespace PatternPA.Test.Utils
{
    [TestFixture]
    public class ShuffleTest
    {
        [Test]
        public void GetFisherYatesShuffleTest()
        {
            var expected = new [] {0, 0, 1, 1, 0, 1, 2, 1, 2, 1};
            var actual = expected.GetFisherYatesShuffle();

            Assert.That(expected, Is.EquivalentTo(actual));
            Assert.That(expected, Is.Not.EqualTo(actual));
        }
    }
}
