using System.Collections.Generic;
using System.IO;
using System.Linq;
using PatternPA.Core.Interfaces.FileOperation;

namespace PatternPA.Utils
{
    public class FileWriter : IFileWriter
    {
        private static object syncRoot1 = new object();
        private static object syncRoot2 = new object();
        private static string defaultFileName = "output.txt";

        private StreamWriter sWriter;

        public FileWriter() : this(defaultFileName) { }

        public FileWriter(string filePath)
        {
            FilePath = filePath;
        }

        public string FilePath { get; set; }

        #region IFileWriter Members

        public void WriteText<T>(IEnumerable<T> data)
        {
            lock (syncRoot1)
            {
                using (sWriter = new StreamWriter(FilePath))
                {
                    foreach (T record in data)
                    {
                        sWriter.Write(record.ToString());
                    }
                }
            }
        }

        public void WriteBinary(IEnumerable<int> data)
        {
            lock (syncRoot2)
            {
                using (var stream = new FileStream(FilePath, FileMode.OpenOrCreate))
                {
                    using (var writer = new BinaryWriter(stream))
                    {
                        data.ToList().ForEach(writer.Write);
                        writer.Close();
                    }
                }
            }
        }

        #endregion
    }
}