using System;

namespace PatternPA.Core.Model
{
    public class EventRecord : Record, IEquatable<EventRecord>
    {
        public double ActivityScore { get; set; }
        public int CumulativeStepCount { get; set; }
        public int DataCount { get; set; }
        public TimeSpan Duration { get; set; }

        public EventRecord()
        {
        }

        public EventRecord(double activityScore, int cumulativeStepCount, int dataCount, TimeSpan duration)
        {
            ActivityScore = activityScore;
            CumulativeStepCount = cumulativeStepCount;
            DataCount = dataCount;
            Duration = duration;
        }

        public EventRecord(DateTime date, ActivityCodes activityCode, double activityScore, int cumulativeStepCount,
                           int dataCount, TimeSpan duration)
            : base(date, activityCode)
        {
            ActivityScore = activityScore;
            CumulativeStepCount = cumulativeStepCount;
            DataCount = dataCount;
            Duration = duration;
        }

        #region IEquatable<EventRecord>

        public bool Equals(EventRecord other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return base.Equals(other) && other.ActivityScore.Equals(ActivityScore) &&
                   other.CumulativeStepCount == CumulativeStepCount && other.DataCount == DataCount &&
                   other.Duration.Equals(Duration);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            return Equals(obj as EventRecord);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = base.GetHashCode();
                result = (result*397) ^ ActivityScore.GetHashCode();
                result = (result*397) ^ CumulativeStepCount;
                result = (result*397) ^ DataCount;
                result = (result*397) ^ Duration.GetHashCode();
                return result;
            }
        }

        public static bool operator ==(EventRecord left, EventRecord right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EventRecord left, EventRecord right)
        {
            return !Equals(left, right);
        }

        #endregion

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}