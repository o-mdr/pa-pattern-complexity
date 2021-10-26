using System;
using System.IO;
using System.IO.Compression;
using PatternPA.Core.Interfaces;

namespace PatternPA.Utils
{
    public class GZipArchiver : IArchiver
    {
        #region IArchiver Members

        public string Compress(FileInfo fi)
        {
            string outPath;

            using (FileStream inFile = fi.OpenRead())
            {
                outPath = fi.FullName + ".gz";
                using (FileStream outFile = File.Create(outPath))
                {
                    using (var compress = new GZipStream(outFile, CompressionMode.Compress))
                    {
                        inFile.CopyTo(compress);
                    }
                }
            }

            return outPath;
        }

        public string Decompress(FileInfo fi)
        {
            using (FileStream inFile = fi.OpenRead())
            {
                string curFile = fi.FullName;
                string origName = curFile.Remove(curFile.Length - fi.Extension.Length);

                using (FileStream outFile = File.Create(origName))
                {
                    using (var decompress = new GZipStream(inFile, CompressionMode.Decompress))
                    {
                        decompress.CopyTo(outFile);
                    }
                }

                return origName;
            }
        }

        #endregion

        public double GetMaxCompressionRatio(string fPath)
        {
            if (!File.Exists(fPath))
            {
                throw new FileNotFoundException();    
            }

            //Compress
            byte[] input = File.ReadAllBytes(fPath);
            var destinationStream = new MemoryStream();
            using (var writerStream = new GZipStream(destinationStream, CompressionMode.Compress))
            {
                writerStream.CopyTo(destinationStream);
            }

            return (double)input.Length / destinationStream.Length;
        }
    }
}