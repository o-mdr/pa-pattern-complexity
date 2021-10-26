using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PatternPA.Core.Interfaces.FileOperation;

namespace PatternPA.Utils
{
    public class BinaryFileWriter : IBinaryFileWriter
    {
        public void Write(string path, FileMode mode, IEnumerable<int> data)
        {
            using (var stream = new FileStream(path, mode))
            {
                using (var writer = new BinaryWriter(stream))
                {
                    data.ToList().ForEach(writer.Write);
                }
            }
        }

        public void Write(string path, FileMode mode, IEnumerable<byte> data)
        {
            using (var stream = new FileStream(path, mode))
            {
                using (var writer = new BinaryWriter(stream))
                {
                    data.ToList().ForEach(writer.Write);
                }
            }
        }

        public void Write(string path, FileMode mode, BitArray data)
        {
            using (var stream = new FileStream(path, mode))
            {
                using (var writer = new BinaryWriter(stream))
                {
                    foreach (bool bit in data)
                    {
                        writer.Write(bit);
                    }
                    stream.Flush(true);
                }
            }
        }
    }
}