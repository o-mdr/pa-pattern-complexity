using System;
using System.Diagnostics;
using System.IO;

namespace PatternPA.Utils.Entropy.MSE
{
    public class SingleFileMse : AbstractMse
    {
        public SingleFileMse(string inputFile, string outputFile = "")
        {
            InputFile = Path.GetFullPath(inputFile);

            if (!File.Exists(InputFile))
            {
                throw new FileNotFoundException("Cannot find data file required to compute MSE",
                    InputFile);
            }

            if (String.IsNullOrEmpty(outputFile))
            {
                string inputNameWithoutExt = Path.GetFileNameWithoutExtension(inputFile);
                outputFile = "MseResult_" + inputNameWithoutExt + ".txt";
            }

            OutputFile = Path.GetFullPath(outputFile);
        }

        public string Compute(string args)
        {
            string exeName = Path.GetFileName(PathToExecutable);
            string rootDir = Path.GetDirectoryName(PathToExecutable);

            if (String.IsNullOrEmpty(rootDir))
            {
                throw new DirectoryNotFoundException();
            }

            var info = new ProcessStartInfo(exeName, args)
                           {
                               UseShellExecute = false,
                               RedirectStandardInput = true,
                               RedirectStandardOutput = true,
                               WorkingDirectory = rootDir
                           };

            var process = Process.Start(info);

            //writing input
            using (var inputWriter = process.StandardInput)
            {
                var data = File.ReadAllBytes(InputFile);
                inputWriter.Write(data);
            }

            //read output
            using (var reader = process.StandardOutput)
            {
                string result = reader.ReadToEnd();
                File.WriteAllText(OutputFile, result);
            }

            process.WaitForExit();

            return OutputFile;
        }
    }
}
