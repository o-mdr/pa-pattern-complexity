using System.IO;

namespace PatternPA.Core.Interfaces
{
    public interface IArchiver
    {
        string Compress(FileInfo fi);
        string Decompress(FileInfo fi);

        double GetMaxCompressionRatio(string fPath);

    }
}