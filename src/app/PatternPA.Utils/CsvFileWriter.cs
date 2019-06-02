using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using PatternPA.Core.Interfaces.FileOperation;

namespace PatternPA.Utils
{
    public class CsvFileWriter : ICsvFileWriter
    {
        private static readonly object SyncRoot1 = new object();
        private static readonly object SyncRoot2 = new object();
        private const string DefaultFileName = "output.csv";

        public string OutputFilePath { get; set; }
        private StreamWriter _writer;

        public CsvFileWriter() : this(DefaultFileName) { }

        public CsvFileWriter(string filePath)
        {
            Separator = ", ";
            OutputFilePath = filePath;
        }

        #region ICSVFileWriter Members

        public String Separator { get; set; }

        public string WriteData<T>(IEnumerable<T> data,
                                    string toFilePath,
                                    bool asOneLineString = false,
                                    bool shouldAppend = false)
        {
            if (String.IsNullOrEmpty(toFilePath))
            {
                toFilePath = OutputFilePath;
            }

            lock (SyncRoot1)
            {
                using (_writer = new StreamWriter(toFilePath, shouldAppend))
                {
                    if (asOneLineString)
                    {
                        foreach (T record in data)
                        {
                            _writer.Write(record.ToString());
                        }
                    }
                    else
                    {
                        foreach (T record in data)
                        {
                            _writer.WriteLine(record.ToString());
                        }
                    }
                }
            }

            return toFilePath;
        }

        public string WriteData(string data,
                              string toFilePath,
                              bool shouldAppend = false)
        {
            if (String.IsNullOrEmpty(toFilePath))
            {
                toFilePath = OutputFilePath;
            }

            lock (SyncRoot1)
            {
                using (_writer = new StreamWriter(toFilePath, shouldAppend))
                {
                    _writer.Write(data);
                }
            }

            return toFilePath;
        }

        #endregion

        #region Obsolete

        [Obsolete]
        public void WriteDataTable(DataTable table)
        {
            lock (SyncRoot2)
            {
                using (_writer = new StreamWriter(OutputFilePath))
                {
                    // header
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        DataColumn column = table.Columns[i];
                        _writer.Write(column.ColumnName);

                        if (i < table.Columns.Count - 1)
                        {
                            _writer.Write(Separator);
                        }
                    }

                    _writer.WriteLine();

                    // data
                    foreach (DataRow row in table.Rows)
                    {
                        string str = row[0] + Separator + row[1] + Separator + row[2] + Separator + row[3];

                        _writer.WriteLine(str);
                    }
                }
            }
        }

        #endregion
    }
}