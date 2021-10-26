using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PatternPA.Core.Model.Nhanes;

namespace PatternPA.Core.Model
{
    public class Person : ActivePalEntity, IDisposable
    {
        public Person(string filePath)
            : this(filePath, DefaultCheckpoint)
        {
        }

        public Person(string filePath, TimeSpan checkpointRate)
            : base(checkpointRate)
        {
            DataFilePath = filePath;
        }

        public double? CompressionRate { get; set; }
        public bool IsEntropyKnown { get; set; }
        public bool IsSnapshotsKnown { get; set; }
        
        public IntervalSnapshots Snapshots { get; set; }
        public EntropyResult EntropyResult { get; set; }
        public BitArray CoarseGrainedSequence { get; set; }
        public DateTime StartFrom { get; set; }
        public DateTime StopAt { get; set; }
        public IEnumerable<EventRecord> ActivePalEvents { get; set; }
        public IEnumerable<NhanesRecord> NhanesRecords { get; set; }
        public IDictionary<string, decimal> WordsProbability { get; set; }

        // doesn't clean up unmanaged resources, but this guy holds lots of data
        // would be nice to get it collected asap
        public void Dispose()
        {
            Snapshots = null;
            EntropyResult = null;
            ActivePalEvents = null;
            CoarseGrainedSequence = null;
        }

        public string GetNhanceSequenceString()
        {
            if (NhanesRecords == null || NhanesRecords.Count() == 0)
            {
                return String.Empty;
            }

            var codes = from record in NhanesRecords
                        select record.IntencityCode;
            var sb = new StringBuilder(codes.Count());
            foreach (var code in codes)
            {
                sb.Append((byte)code);
            }

            return sb.ToString();
        }
    }
}