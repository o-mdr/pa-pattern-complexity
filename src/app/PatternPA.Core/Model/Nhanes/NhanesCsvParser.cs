using System;
using System.Collections.Generic;
using System.IO;
using PatternPA.Core.Interfaces.Nhanes;

namespace PatternPA.Core.Model.Nhanes
{

    public class NhanesCsvParser : INhanesCsvParser
    {
        private enum Version
        {
            Version2003_2004,
            Version2005_2006
        }

        /// <summary>
        /// <see cref="http://www.cdc.gov/nchs/nhanes/nhanes2003-2004/PAXRAW_C.htm"/>
        /// </summary>
        private class Version2003_2004
        {
            internal const int SubjectIdIdx = 0;
            internal const int ReliabilityFlagIdx = 1;
            internal const int CalibrationFlagIdx = 2;
            internal const int DayOfWeekIdx = 3;
            internal const int SequenceIdIdx = 4;
            internal const int HourIdx = 5;
            internal const int MinuteIdx = 6;
            internal const int DeviceIntensityIdx = 7;
            internal const int IntencityCodeIdx = 8;
        }

        /// <summary>
        /// <see cref="http://www.cdc.gov/nchs/nhanes/nhanes2005-2006/PAXRAW_D.htm"/>
        /// </summary>
        private class Version2005_2006
        {
            internal const int SubjectIdIdx = 0;
            internal const int ReliabilityFlagIdx = 1;
            internal const int CalibrationFlagIdx = 2;
            internal const int DayOfWeekIdx = 3;
            internal const int SequenceIdIdx = 4;
            internal const int HourIdx = 5;
            internal const int MinuteIdx = 6;
            internal const int DeviceIntensityIdx = 7;
            internal const int StepCountIdx = 8;
            internal const int IntencityCodeIdx = 9;
        }

        public IEnumerable<NhanesRecord> ParseCsv(string filePath)
        {
            if (!File.Exists(filePath))
            {
                new FileNotFoundException(filePath + " was not found");
            }

            Version fileVersion = GetVersion(filePath, headerLines: 1);
            switch (fileVersion)
            {
                case Version.Version2003_2004:
                    return ParseCsvVersion2003_2004(filePath, headerLines:1);
                case Version.Version2005_2006:
                    return ParseCsvVersion2005_2006(filePath, headerLines: 1);
                default: throw new NotSupportedException("Unknown nhanes file version");
            }

        }

        private IEnumerable<NhanesRecord> ParseCsvVersion2005_2006(string filePath, int headerLines = 0)
        {
            var records = new List<NhanesRecord>();
            using (var streamReader = new StreamReader(filePath))
            {
                string line;

                //skip header
                for (int i = 0; i < headerLines; i++)
                {
                    streamReader.ReadLine();
                }

                while (!String.IsNullOrEmpty(line = streamReader.ReadLine()))
                {
                    string[] row = line.Split(',');
                    var record = new NhanesRecord();
                    int subjectId = Convert.ToInt32(row[Version2005_2006.SubjectIdIdx]);
                    byte reliabilityFlag = Convert.ToByte(row[Version2005_2006.ReliabilityFlagIdx]);
                    byte calibrationFlag = Convert.ToByte(row[Version2005_2006.CalibrationFlagIdx]);
                    byte dayOfWeek = Convert.ToByte(row[Version2005_2006.DayOfWeekIdx]);
                    int sequenceId = Convert.ToInt32(row[Version2005_2006.SequenceIdIdx]);
                    byte hour = Convert.ToByte(row[Version2005_2006.HourIdx]);
                    byte minute = Convert.ToByte(row[Version2005_2006.MinuteIdx]);
                    int deviceIntensity = Convert.ToInt32(row[Version2005_2006.DeviceIntensityIdx]);
                    int stepCount = Convert.ToInt32(row[Version2005_2006.StepCountIdx]);
                    var intencityCode = (IntensityCodes)Enum.Parse(typeof(IntensityCodes),
                                                                   row[Version2005_2006.IntencityCodeIdx]);

                    record.SetValues(subjectId: subjectId,
                                     reliabilityFlag: reliabilityFlag,
                                     calibrationFlag: calibrationFlag,
                                     dayOfWeek: dayOfWeek,
                                     sequenceId: sequenceId,
                                     hour: hour,
                                     minute: minute,
                                     deviceIntensity: deviceIntensity,
                                     stepCount: stepCount,
                                     intensityCode: intencityCode);

                    records.Add(record);
                }
            }

            return records;
        }

        private IEnumerable<NhanesRecord> ParseCsvVersion2003_2004(string filePath, int headerLines = 0)
        {
            var records = new List<NhanesRecord>();
            using (var streamReader = new StreamReader(filePath))
            {
                string line;

                //skip header
                for (int i = 0; i < headerLines; i++)
                {
                    streamReader.ReadLine();
                }

                while (!String.IsNullOrEmpty(line = streamReader.ReadLine()))
                {
                    string[] row = line.Split(',');
                    var record = new NhanesRecord();
                    int subjectId = Convert.ToInt32(row[Version2003_2004.SubjectIdIdx]);
                    byte reliabilityFlag = Convert.ToByte(row[Version2003_2004.ReliabilityFlagIdx]);
                    byte calibrationFlag = Convert.ToByte(row[Version2003_2004.CalibrationFlagIdx]);
                    byte dayOfWeek = Convert.ToByte(row[Version2003_2004.DayOfWeekIdx]);
                    int sequenceId = Convert.ToInt32(row[Version2003_2004.SequenceIdIdx]);
                    byte hour = Convert.ToByte(row[Version2003_2004.HourIdx]);
                    byte minute = Convert.ToByte(row[Version2003_2004.MinuteIdx]);
                    int deviceIntensity = Convert.ToInt32(row[Version2003_2004.DeviceIntensityIdx]);
                    var intencityCode = (IntensityCodes)Enum.Parse(typeof(IntensityCodes),
                                                                   row[Version2003_2004.IntencityCodeIdx]);

                    record.SetValues(subjectId: subjectId,
                                     reliabilityFlag: reliabilityFlag,
                                     calibrationFlag: calibrationFlag,
                                     dayOfWeek: dayOfWeek,
                                     sequenceId: sequenceId,
                                     hour: hour,
                                     minute: minute,
                                     deviceIntensity: deviceIntensity,
                                     stepCount: null,
                                     intensityCode: intencityCode);

                    records.Add(record);
                }
            }

            return records;
        }


        private Version GetVersion(string csvPath, int headerLines = 0)
        {
            if (!File.Exists(csvPath))
            {
                new FileNotFoundException(csvPath + " was not found");
            }

            using (var streamReader = new StreamReader(csvPath))
            {
                //skip header
                for (int i = 0; i < headerLines; i++)
                {
                    streamReader.ReadLine();
                }

                string line = streamReader.ReadLine();
                string[] row = line.Split(',');
                if (row.Length == 9)
                    return Version.Version2003_2004;
                if (row.Length == 10)
                    return Version.Version2005_2006;
                throw new NotSupportedException("File has incorrect number of columns, " +
                    "check NHanes database, files of the 2003-2004 and 2005-2006 only are supported. " +
                    "File: " + csvPath);
            }
        }
    }
}