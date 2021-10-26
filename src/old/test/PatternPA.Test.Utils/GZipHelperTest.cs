using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using PatternPA.Test.Utils._7zip;

namespace PatternPA.Test.Utils
{
    [TestFixture]
    public class GZipHelperTest : AbstractUtilsTest
    {
        private int sequenceLenght = 100;
        private string tmpFilePath;
        private Random r = new Random();

        [Test]
        public void CompressLineOfOneEventTest()
        {
            IEnumerable<int> sequence = rndEventGenerator.GenerateLineOfOneEvent(0, sequenceLenght);

            tmpFilePath = String.Concat(r.Next(), "tmp.bin");

            binaryFileWriter.Write(tmpFilePath, FileMode.CreateNew, sequence);
            double compressionRate = GzipStreamHelper.Compress(tmpFilePath);

            Assert.IsTrue(compressionRate > 0);
        }

        [Test]
        public void CompressIntegrationTest()
        {
            IEnumerable<int> availableEvents = new[] {0, 1, 2};
            IEnumerable<int> sequence = rndEventGenerator.GenerateRandomEvents(availableEvents, sequenceLenght);

            tmpFilePath = String.Concat(r.Next(), "tmp.bin");

            binaryFileWriter.Write(tmpFilePath, FileMode.CreateNew, sequence);
            double compressionRate = GzipStreamHelper.Compress(tmpFilePath);

            Assert.IsTrue(compressionRate > 0);
        }
    }
}