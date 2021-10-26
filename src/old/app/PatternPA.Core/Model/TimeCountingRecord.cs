using System;

namespace PatternPA.Core.Model
{
    public class TimeCountingRecord : Record
    {
        public static string separator = ", ";

        public TimeSpan TotalSitTime { get; set; }
        public TimeSpan TotalUprightTime { get; set; }
        public TimeSpan TotalWalkTime { get; set; }

        public TimeCountingRecord()
        {
            TotalSitTime = new TimeSpan();
            TotalUprightTime = new TimeSpan();
            TotalWalkTime = new TimeSpan();
        }

        public override string ToString()
        {
            //Sun - 0, Mon - 1, Tue -2 ...
            return String.Concat((int)Date.DayOfWeek, separator,
                                 TotalSitTime.TotalSeconds, separator,
                                 TotalUprightTime.TotalSeconds, separator,
                                 TotalWalkTime.TotalSeconds);

        }

        public TimeCountingRecord Copy()
        {
            return new TimeCountingRecord
                       {
                           ActivityCode = ActivityCode,
                           Date = Date,
                           TotalSitTime = TotalSitTime,
                           TotalUprightTime = TotalUprightTime,
                           TotalWalkTime = TotalWalkTime,
                           dateFormat = dateFormat
                       };
        }
    }
}
