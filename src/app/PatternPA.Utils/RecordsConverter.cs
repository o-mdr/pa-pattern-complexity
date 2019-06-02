using System;
using System.Collections.Generic;
using PatternPA.Core.Interfaces.Convertor;
using PatternPA.Core.Model;

namespace PatternPA.Utils
{
    public abstract class RecordsConverter : IRecordConverter
    {
        protected static TimeSpan defaultActivityCheck = new TimeSpan(0, 0, 0, 0, 100);

        protected RecordsConverter() : this(defaultActivityCheck) { }

        protected RecordsConverter(TimeSpan activityCheck)
        {
            ActivityCheck = activityCheck;
            CheckpointRecords = new List<Record>();
        }

        public TimeSpan ActivityCheck { get; set; }
        public IEnumerable<Record> CheckpointRecords { get; set; }

        #region IRecordConverter Members

        public abstract IntervalSnapshots ToCheckpointRecords(
            IEnumerable<EventRecord> events, DateTime start,
            DateTime stop, TimeSpan activityCheck);
        
        public abstract IEnumerable<TimeSpan> GetCheckPointsWithin(TimeSpan start, TimeSpan end);

        #endregion
    }
}