using System;
using System.IO;
using NUnit.Framework;
using PatternPA.Utils.Entropy.MSE;

namespace PatternPA.Test.Utils.Entropy.MSE
{
    [TestFixture]
    public class SingleFileMultiScaleEntropyTest
    {
        private const string dataPath = "Data\\MSE_Sample.txt";
        
        [Test]
        public void CheckThatDataFileExistTest()
        {
            var entropy = new SingleFileMse(dataPath);
            Assert.IsNotNull(entropy);
            Assert.Pass("No exception has bee thrown so we good to use the test file");
        }

        [Test]
        public void AllDefaultParamsComputeTest()
        {
            var entropy = new SingleFileMse(dataPath);
            
            var builder = new MseArgumentBuilder();
            string args = builder.Build();

            string output = entropy.Compute(args);
            Assert.IsTrue(File.Exists(output));
            Assert.AreEqual(236, File.ReadAllBytes(output).Length);
        }
    }
}
