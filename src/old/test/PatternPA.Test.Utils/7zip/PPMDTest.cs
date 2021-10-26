using System.IO;
using System.Text;
using NUnit.Framework;
using PatternPA.Core.Interfaces;
using PatternPA.Utils._7zip;

namespace PatternPA.Test.Utils._7zip
{
    [TestFixture]
    public class PPMDTest
    {
        [Test]
        public void CompressDecompressWithPPMDTest()
        {
            string toBeEncodedPath = "file.txt";
            string str = "012";
            var sb = new StringBuilder();
            for (int i = 0; i < 100; i++)
            {
                sb.Append(str);
            }
            string expectedContent = sb.ToString();

            File.WriteAllText(toBeEncodedPath, expectedContent, Encoding.ASCII);
            var toBeEncodedFileInfo = new FileInfo(toBeEncodedPath);

            IArchiver archiver = new PPMDArchiver();
            string encodedPath = archiver.Compress(toBeEncodedFileInfo);
            var encodedFileInfo = new FileInfo(encodedPath);

            Assert.IsTrue(File.Exists(encodedPath));
            Assert.IsTrue(encodedFileInfo.Length == 122);

            File.Delete(toBeEncodedPath);

            //DECODE
            string decodedPath = archiver.Decompress(encodedFileInfo);
            Assert.IsTrue(File.Exists(decodedPath));
            string actualContent = File.ReadAllText(decodedPath);
            Assert.IsTrue(actualContent.Length == 300);
            Assert.AreEqual(expectedContent, expectedContent);
        }

        [Test]
        public void PPMZArchiver_Compress() 
        {
            string toBeEncodedPath = "file.txt";
            string str = "012";
            var sb = new StringBuilder();
            for (int i = 0; i < 100; i++)
            {
                sb.Append(str);
            }
            string expectedContent = sb.ToString();

            File.WriteAllText(toBeEncodedPath, expectedContent, Encoding.ASCII);
            var toBeEncodedFileInfo = new FileInfo(toBeEncodedPath);

            IArchiver archiver = new PPMDArchiver();
            string encodedPath = archiver.Compress(toBeEncodedFileInfo);
            var encodedFileInfo = new FileInfo(encodedPath);

            Assert.IsTrue(File.Exists(encodedPath));
            Assert.IsTrue(encodedFileInfo.Length == 122);
        }
    }
}
