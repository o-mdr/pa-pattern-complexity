using System;
using System.Collections.Generic;
using System.IO;
using PatternPA.Core.Interfaces.FileOperation;
using PatternPA.Core.Model;

namespace PatternPA.Utils
{
    /// <summary>
    ///   The csv parser.
    /// </summary>
    public class CsvParser : ICsvParser
    {
        #region ICsvParser

        public IEnumerable<EventRecord> ParseCsv(string filePath, int skipFirstRows)
        {
            if (!File.Exists(filePath))
            {
                new FileNotFoundException(filePath + " was not found");
            }
            if (skipFirstRows < 0)
            {
                throw new ArgumentOutOfRangeException("skipFirstRows",
                    "skipFirstRows parameter cannot be less than 0");
            }

            var list = new List<EventRecord>();
            
            using (var readFile = new StreamReader(filePath))
            {
                string line;
                string[] row;
                EventRecord record;

                // skip header
                for (int i = skipFirstRows; i > 0; i--)
                {
                    readFile.ReadLine();
                }

                while (!String.IsNullOrEmpty(line = readFile.ReadLine()))
                {
                    line = line.Replace("#", String.Empty);
                    line = line.Replace("\t", String.Empty);
                    row = line.Split(',');
                    record = new EventRecord
                                 {
                                     Date = Convert.ToDateTime(row[0]),
                                     DataCount = Convert.ToInt32(row[1].Replace(" ", String.Empty)),
                                     Duration =
                                         TimeSpan.FromSeconds(Convert.ToDouble(row[2].Replace(" ", String.Empty))),
                                     ActivityCode = (ActivityCodes) Convert.ToInt32(row[3].Replace(" ", String.Empty)),
                                     CumulativeStepCount = Convert.ToInt32(row[4].Replace(" ", String.Empty)),
                                     ActivityScore = Convert.ToDouble(row[5].Replace(" ", String.Empty)),
                                 };

                    list.Add(record);
                }
            }

            return list;
        }

        #endregion
    }
}