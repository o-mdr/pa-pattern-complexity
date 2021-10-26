using System;
using System.Collections.Generic;
using System.Linq;

namespace PatternPA.Core.Model
{
    public class TimeCountingRecords
    {
        private bool dummyEntryExist;

        public List<TimeCountingRecord> Records { get; set; }
        
        public TimeCountingRecords()
        {
            Records = new List<TimeCountingRecord> {new TimeCountingRecord()};
            dummyEntryExist = true;
        }

        /// <summary>
        /// create new entry in its inner storage, copies previous record, update correspondent time
        /// </summary>
        public void Add(Record record, TimeSpan checkTime)
        {
            var r = Records.LastOrDefault().Copy();

            if (dummyEntryExist)
            {
                Records.RemoveAt(0);
                dummyEntryExist = false;
            }
            
            r.Date = record.Date;
            r.ActivityCode= record.ActivityCode;

            switch (r.ActivityCode)
            {
                case ActivityCodes.Sit:
                    {
                        r.TotalSitTime += checkTime;
                        break;
                    }
                case ActivityCodes.Upright:
                    {
                        r.TotalUprightTime += checkTime;
                        break;
                    }
                case ActivityCodes.Walk:
                    {
                        r.TotalWalkTime += checkTime;
                        break;
                    }
            }
            
            Records.Add(r);
        }

        public void RemoveAt(int i)
        {
            Records.RemoveAt(i);
        }
    }
}
