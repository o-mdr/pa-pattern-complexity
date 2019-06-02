using System;
using System.Collections.Generic;
using PatternPA.Core.Interfaces;
using PatternPA.Core.Model;

namespace PatternPA.Utils
{
    public class CheckpointFactory : ICheckpointFactory
    {
        public IEnumerable<Record> CreateRecords(DateTime actualStart,
                                                    DateTime actualStop,
                                                    TimeSpan checkTime)
        {
            var result = new List<Record>();

            while (actualStart <= actualStop)
            {
                var r = new Record
                {
                    ActivityCode = ActivityCodes.NotDefined,
                    Date = actualStart
                };

                result.Add(r);

                actualStart = actualStart.Add(checkTime);
            }

            return result;
        }

        public IEnumerable<TimeSpan> CreateDayCheckpoints(TimeSpan checkTime)
        {
            var result = new List<TimeSpan>();
            TimeSpan currentSpan = checkTime;
            while (currentSpan <= TimeSpan.FromDays(1))
            {
                result.Add(currentSpan);
                currentSpan = currentSpan + checkTime;
            }

            return result;
        }
    }
}
