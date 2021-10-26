using System.IO;

namespace PatternPA.Utils.Entropy.MSE
{
    public class AbstractMse
    {
        public string PathToExecutable { get; protected set; }
        public string OutputFile { get; protected set; }
        public string InputFile { get; protected set; }

        public AbstractMse()
        {
            PathToExecutable = Path.GetFullPath("mse.exe");
            if (!File.Exists(PathToExecutable))
            {
                throw new FileNotFoundException("Cannot find mse.exe file required to compute MSE",
                    PathToExecutable);
            }
        }
    }
}