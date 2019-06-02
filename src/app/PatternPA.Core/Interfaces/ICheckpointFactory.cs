using System;
using System.Collections.Generic;
using PatternPA.Core.Model;

namespace PatternPA.Core.Interfaces
{
    public interface ICheckpointFactory
    {
        IEnumerable<Record> CreateRecords(DateTime actualStart,
                                          DateTime actualStop,
                                          TimeSpan checkTime);

        IEnumerable<TimeSpan> CreateDayCheckpoints(TimeSpan checkTime);
    }
}