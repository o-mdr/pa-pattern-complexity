using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace PatternPA.Core.Interfaces.FileOperation
{
    public interface IBinaryFileWriter
    {
        void Write(string path, FileMode mode, IEnumerable<int> data);
        void Write(string path, FileMode mode, IEnumerable<byte> data);
        void Write(string path, FileMode mode, BitArray data);
    }
}