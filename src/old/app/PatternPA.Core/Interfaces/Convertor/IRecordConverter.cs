using System;
using System.Collections.Generic;
using PatternPA.Core.Model;

namespace PatternPA.Core.Interfaces.Convertor
{
    public interface IRecordConverter
    {
        TimeSpan ActivityCheck { get; set; }

        IntervalSnapshots ToCheckpointRecords(IEnumerable<EventRecord> events,
                                              DateTime start,
                                              DateTime stop,
                                              TimeSpan activityCheck);

        IEnumerable<TimeSpan> GetCheckPointsWithin(TimeSpan start, TimeSpan end);
    }
}