using System.Collections.Generic;
using NUnit.Framework;

namespace PatternPA.Test.Utils
{
    [TestFixture]
    public class BinaryConverterTest : AbstractUtilsTest
    {
        public TestContext TestContext { get; set; }

        [Test]
        public void ToBinArrayTestHelper()
        {
            IEnumerable<int> data = new[] {0, 1, 2, 3, 4};
            byte[] actual = binaryConverter.ToBinArray(data);

            Assert.IsTrue(actual.Length == 2);
            Assert.AreEqual(118, actual[0]);
            Assert.AreEqual(0, actual[1]);
        }
    }
}