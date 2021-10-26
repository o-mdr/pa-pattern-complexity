using System;
using System.Collections.Generic;
using System.Linq;

namespace PatternPA.Core.Model
{
    public class IntervalSnapshots
    {
        public IntervalSnapshots()
        {
            Data = new List<Record>();
        }

        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public IEnumerable<Record> Data { get; set; }
        public int CumulativeStepCount { get; set; }

        public void Clear()
        {
            Data = null;
            Data = new List<Record>();
        }

        public IEnumerable<ActivityCodes> GetActivityCodeSequence()
        {
            return from r in Data select r.ActivityCode;
        }
    }
}