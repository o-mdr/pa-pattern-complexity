using System.IO;
using System.Text;
using NUnit.Framework;
using PatternPA.Core.Interfaces;
using PatternPA.Utils._7zip;

namespace PatternPA.Test.Utils._7zip
{
	[TestFixture]
	public class LZMATest
	{
		
		[Test]
		public void CompressDecompressWithLZMATest()
		{
			string toBeEncodedPath = "file.txt";
			string expectedContent = "test content to aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

			File.WriteAllText(toBeEncodedPath, expectedContent, Encoding.ASCII);
			var toBeEncodedFileInfo = new FileInfo(toBeEncodedPath);

			IArchiver archiver = new LZMAArchiver();
			string encodedPath = archiver.Compress(toBeEncodedFileInfo);
			var encodedFileInfo = new FileInfo(encodedPath);

			Assert.IsTrue(File.Exists(encodedPath));
			Assert.IsTrue(encodedFileInfo.Length == 36);

			File.Delete(toBeEncodedPath);

			//DECODE
			string decodedPath = archiver.Decompress(encodedFileInfo);
			Assert.IsTrue(File.Exists(decodedPath));
			string actualContent = File.ReadAllText(decodedPath);
			Assert.IsTrue(actualContent.Length == 69);
			Assert.AreEqual(expectedContent, expectedContent);
		}

		[Test]
		public void LZMAARchiver_Compress()
		{
			string toBeEncodedPath = "file.txt";
			string expectedContent = "test content to aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

			File.WriteAllText(toBeEncodedPath, expectedContent, Encoding.ASCII);
			var toBeEncodedFileInfo = new FileInfo(toBeEncodedPath);

			IArchiver archiver = new LZMAArchiver();
			string encodedPath = archiver.Compress(toBeEncodedFileInfo);
			var encodedFileInfo = new FileInfo(encodedPath);

			Assert.IsTrue(File.Exists(encodedPath));
			Assert.IsTrue(encodedFileInfo.Length == 36);
		}
	}
}
