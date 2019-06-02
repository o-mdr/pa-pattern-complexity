using System;
using PatternPA.Core.Interfaces;

namespace PatternPA.Core.Model
{
    public class Record : IEquatable<Record>, IRecord
    {
        public string dateFormat = "{0:dd/MM/yyyy HH:mm:ss.fff}";

        public DateTime Date { get; set; }
        public ActivityCodes ActivityCode { get; set; }
        
        public Record()
        {
        }

        public Record(DateTime date, ActivityCodes activityCode)
        {
            Date = date;
            ActivityCode = activityCode;
        }

        #region IRecord

        public object GetValue()
        {
            return this;
        }

        #endregion

        #region IEquatable<Record>

        public bool Equals(Record other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return other.Date.Equals(Date) && Equals(other.ActivityCode, ActivityCode);
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
            if (obj.GetType() != typeof (Record))
            {
                return false;
            }
            return Equals((Record) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Date.GetHashCode()*397) ^ ActivityCode.GetHashCode();
            }
        }

        public static bool operator ==(Record left, Record right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Record left, Record right)
        {
            return !Equals(left, right);
        }

        #endregion
        
        public override string ToString()
        {
            return String.Concat(String.Format(dateFormat, Date), ", ", (int) ActivityCode);
            //return Convert.ToString((int) ActivityCode);
        }
    }
}