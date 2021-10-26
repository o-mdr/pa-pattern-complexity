using System;

namespace PatternPA.Core.Model.Nhanes
{
    /// <summary>
    /// <see cref="http://www.cdc.gov/nchs/nhanes/nhanes2003-2004/PAXRAW_C.htm"/>
    /// <see cref="http://www.cdc.gov/nchs/nhanes/nhanes2005-2006/PAXRAW_D.htm"/>
    /// </summary>
    public class NhanesRecord
    {
        public int SubjectId { get; private set; }
        public byte ReliabilityFlag { get; private set; }
        public byte CalibrationFlag { get; private set; }
        public byte DayOfWeek { get; private set; }
        public int SequenceId { get; private set; }
        public byte Hour { get; private set; }
        public byte Minute { get; private set; }
        public int DeviceIntensity { get; private set; }
        public int? StepCount { get; private set; }
        public IntensityCodes IntencityCode { get; private set; }

        public void SetValues(int subjectId, byte reliabilityFlag, byte calibrationFlag,
                                byte dayOfWeek, int sequenceId, byte hour,
                                byte minute, int deviceIntensity, 
                                int? stepCount, IntensityCodes intensityCode)
        {
            SubjectId = subjectId;
            ReliabilityFlag = reliabilityFlag;
            CalibrationFlag = calibrationFlag;
            DayOfWeek = dayOfWeek;
            SequenceId = sequenceId;
            Hour = hour;
            Minute = minute;
            DeviceIntensity = deviceIntensity;
            StepCount = stepCount;
            IntencityCode = intensityCode;
        }

        public override string ToString()
        {
            return String.Concat(SubjectId, ",",
                                 ReliabilityFlag, ",",
                                 CalibrationFlag, ",",
                                 DayOfWeek, ",",
                                 SequenceId, ",",
                                 Hour, ",",
                                 Minute, ",",
                                 DeviceIntensity, ",",
                                 StepCount.HasValue ? StepCount.Value + "," : String.Empty,
                                 (int)IntencityCode);
        }
    }
}