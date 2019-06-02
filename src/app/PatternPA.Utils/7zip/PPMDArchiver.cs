using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using PatternPA.Core.Interfaces;

namespace PatternPA.Utils._7zip
{
    public class PPMDArchiver : IArchiver
    {
        private const string CompressedExtension = ".PPMD";

        public string Compress(FileInfo fi)
        {
            string encodedPath = fi.FullName + CompressedExtension;

            //{0}=archiveName {1}=fileName
            string argsTemplate = "a -t7z \"{0}\" \"{1}\" -m0=PPMd";
            string args = String.Format(argsTemplate, encodedPath, fi.FullName);

            string fullPath = Path.GetFullPath(@"7za.exe");

            if(!File.Exists(fullPath))
            {
                throw new 
                    FileNotFoundException("7za.exe compressor could be found.", fullPath);
            }

            var info = new ProcessStartInfo(fullPath, args);
            var process = Process.Start(info);
            process.WaitForExit();

            return encodedPath;
        }

        public string Decompress(FileInfo fi)
        {
            string decodedPath = fi.FullName.Remove(fi.FullName.Length - fi.Extension.Length);

            //{0}=archiveName 
            string argsTemplate = "e \"{0}\"";
            string args = String.Format(argsTemplate, fi.FullName);

            string fullPath = Path.GetFullPath(@"7za.exe");

            if (!File.Exists(fullPath))
            {
                throw new
                    FileNotFoundException("7za.exe compressor could be found.", fullPath);
            }

            var info = new ProcessStartInfo(fullPath, args);
            var process = Process.Start(info);
            process.WaitForExit();

            return decodedPath;
        }

        public double GetMaxCompressionRatio(string fPath)
        {
            throw new NotImplementedException();
        }
    }
}
