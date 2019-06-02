using System;
using System.Collections.Generic;

namespace PatternPA.Core.Interfaces.FileOperation
{
    public interface ICsvFileWriter
    {
        String Separator { get; set; }
        string OutputFilePath { get; set; }

        string WriteData<T>(IEnumerable<T> data,
                            string toFilePath,
                            bool asOneLineString = false,
                            bool shouldAppend = false);

        string WriteData(string data, string toFilePath, bool shouldAppend = false);
    }
}