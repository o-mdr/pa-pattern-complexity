using System;
using System.Collections.Generic;
using System.Linq;
using PatternPA.Core.Interfaces;
using PatternPA.Core.Model;

namespace PatternPA.Utils
{
    /// <summary>
    ///   The pal timed probability.
    /// </summary>
    public class PalRecordsConverter : RecordsConverter
    {
        public PalRecordsConverter() { }

        public PalRecordsConverter(TimeSpan activityCheck)
            : base(activityCheck)
        {
        }

        public override IEnumerable<TimeSpan> GetCheckPointsWithin(TimeSpan start, TimeSpan end)
        {
            var result = new List<TimeSpan>();

            var dayStart = new TimeSpan(0, 0, 0, 0);
            var dayEnd = new TimeSpan(1, 0, 0, 0);

            var factory = new CheckpointFactory();

            var checkpoints = factory.CreateDayCheckpoints(ActivityCheck);

            // event goes to next day)
            if (end.Days >= 1)
            {
                // remove the day
                var midnight = new TimeSpan(1, 0, 0, 0);
                result.Add(midnight);
                end = end.Subtract(midnight);

                checkpoints.ToList().ForEach(
                    point =>
                    {
                        if ((point > dayStart && point < end) || (point > start && point < dayEnd))
                        {
                            result.Add(point);
                        }
                    });
            }
            else
            {
                // event occured within one day
                checkpoints.ToList().ForEach(
                    point =>
                    {
                        if (point > start && point < end)
                        {
                            result.Add(point);
                        }
                    });
            }

            return result;
        }
        
        public override IntervalSnapshots ToCheckpointRecords(IEnumerable<EventRecord> events,
                                                              DateTime start, DateTime stop, TimeSpan activityCheck)
        {
            var snapshots = new IntervalSnapshots();
            List<Record> data = snapshots.Data.ToList();

            //create a list of checkpoints from the start date to the final record
            ICheckpointFactory factory = new CheckpointFactory();
            var defaultCheckpoints = factory.CreateRecords(start, stop, activityCheck); 

            //TODO can be improved by trimming the whole list from sides
            List<EventRecord> palEvents = events.ToList();

            int pointer = 0;
            int palEventCount = events.Count();
            EventRecord firstIntervalRecord = null;
            EventRecord lastIntervalRecord = null;

            //for every default blunt checkpoint
            defaultCheckpoints.ToList().ForEach(
                checkPoint
                =>
                {
                    //get currentRecord date
                    DateTime checkPointDate = checkPoint.Date;

                    //while pointer is less the count
                    while (pointer < palEventCount)
                    {
                        EventRecord palEvent = palEvents[pointer];
                        DateTime end = palEvent.Date + palEvent.Duration;

                        //if event date stretches thoough the checkpoint
                        if (end > checkPointDate)
                        {
                            //set first record in the interval
                            if (firstIntervalRecord == null)
                            {
                                firstIntervalRecord = palEvent;
                            }

                            //will leave the last record after looping
                            lastIntervalRecord = palEvent;

                            //remember activity code
                            checkPoint.ActivityCode = palEvent.ActivityCode;
                            data.Add(checkPoint);
                            break;
                        }

                        pointer += 1;
                    }
                }
                );

            snapshots.Data = data;
            snapshots.StartDate = start;
            snapshots.FinishDate = stop;
            
            if (lastIntervalRecord != null && firstIntervalRecord != null)
            {
                snapshots.CumulativeStepCount = lastIntervalRecord.CumulativeStepCount -
                                                firstIntervalRecord.CumulativeStepCount;
            }

            return snapshots;
        }
    }
}