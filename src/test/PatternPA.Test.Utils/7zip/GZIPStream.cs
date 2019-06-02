using System.IO;
using PatternPA.Container;
using PatternPA.Core.Interfaces;

namespace PatternPA.Test.Utils._7zip
{
   public static class GzipStreamHelper
    {
        public static double Compress(string path)
        {
            var fIn = new FileInfo(path);
            long sizeIn = fIn.Length;

            var gzip = ContainerService.Instance.Resolve<IArchiver>();
            string outFile = gzip.Compress(fIn);

            var fOut = new FileInfo(outFile);
            long sizeOut = fOut.Length;
            double compressionRate = (double)sizeIn / sizeOut;

            fOut.Delete();

            return compressionRate;
        }
    }
}
