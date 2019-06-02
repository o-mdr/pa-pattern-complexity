using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PatternPA.Core.Extensions;
using PatternPA.Core.Interfaces;
using PatternPA.Core.Interfaces.FileOperation;
using PatternPA.Core.Model;
using PatternPA.Utils;
using PatternPA.Utils._7zip;
using PatternPA.Utils.Correlation.Auto;
using PatternPA.Utils.Extension;
using PatternPA.Utils.Huffman.Impl;
using PatternPA.Utils.Huffman.Interface;

namespace PatternPA.Infrastructure.Services
{
    public class PersonCalculationService : AbstractService
    {
        public IntervalSnapshots CalculateSnapshots(Person person)
        {
            if (person.IsSnapshotsKnown)
            {
                return person.Snapshots;
            }

            //start on first day at 00:00:00
            if (person.StartFrom == new DateTime())
            {
                DateTime d = person.ActivePalEvents.ToList().First().Date;
                person.StartFrom = new DateTime(d.Year, d.Month, d.Day);
            }

            //end on the last day at 00:00:00
            if (person.StopAt == new DateTime())
            {
                DateTime d = person.ActivePalEvents.ToList().Last().Date;
                person.StopAt = new DateTime(d.Year, d.Month, d.Day);
            }

            //IRecordConverter converter = new PalRecordsConverter(CheckpointRate);
            IntervalSnapshots result = recordConverter
                .ToCheckpointRecords(
                    person.ActivePalEvents,
                    person.StartFrom,
                    person.StopAt,
                    person.CheckpointRate);

            person.Snapshots = result;

            person.IsSnapshotsKnown = true;

            return person.Snapshots;
        }

        public IEnumerable<IntervalSnapshots> CalculateDailySnapshots(
            Person person,
            TimeSpan startTime,
            TimeSpan stopTime,
            int skipFirsDays = 0,
            int skipLastDay = 0)
        {
            var daysSnapshots = new List<IntervalSnapshots>();

            //first record date
            DateTime date = person.ActivePalEvents.ToList().First().Date.AddDays(skipFirsDays);
            var startDate = new DateTime(date.Year, date.Month, date.Day,
                                         startTime.Hours, startTime.Minutes,
                                         startTime.Seconds, startTime.Milliseconds);

            var stopDate = new DateTime(date.Year, date.Month, date.Day,
                                        stopTime.Hours, stopTime.Minutes,
                                        stopTime.Seconds, stopTime.Milliseconds);

            //last date record
            date = person.ActivePalEvents.ToList().Last().Date;
            var fullStopDate = new DateTime(date.Year, date.Month, date.Day - skipLastDay);

            while (stopDate < fullStopDate)
            {
                //IRecordConverter converter = new PalRecordsConverter(CheckpointRate);
                IntervalSnapshots daySnap = recordConverter
                    .ToCheckpointRecords(
                        person.ActivePalEvents,
                        startDate,
                        stopDate,
                        person.CheckpointRate);
                daysSnapshots.Add(daySnap);

                startDate = startDate.AddDays(1);
                stopDate = stopDate.AddDays(1);
            }

            return daysSnapshots;
        }

        public IEnumerable<Record> CalculateSnapshotsForDefinedTime(Person person)
        {
            var allSnapshots = CalculateSnapshots(person);
            return TrimRecordsByDefinedTime(allSnapshots.Data);
        }

        public EntropyResult CalculateEntropy(Person person)
        {
            if (person.IsEntropyKnown)
            {
                return person.EntropyResult;
            }
            if (!person.IsSnapshotsKnown)
            {
                person.Snapshots = CalculateSnapshots(person);
            }

            var entropy = new Entropy();
            EntropyResult result = entropy.CalculateShannonEntropy(person.Snapshots, alphabet.GetAlphabet());
            person.EntropyResult = result;
            person.IsEntropyKnown = true;

            return result;
        }

        public double CalculateCompressionRate(Person person, CompressionType compressionType
                                                                    = CompressionType.Gzip)
        {
            if (person.CompressionRate != null)
            {
                return (double)person.CompressionRate;
            }

            IntervalSnapshots snapshots = CalculateSnapshots(person);
            string suffix = ConfigurationManager.AppSettings["snapshotTextOutput"];
            string toUncompressedPath = String.Concat(person.Id, suffix);
            double rate = GetCompressionRate(toUncompressedPath, snapshots.Data, compressionType);
            person.CompressionRate = rate;

            return rate;
        }

        public CompressionRates CalculateCompressionRates(Person person, TimeSpan start,
                                                          TimeSpan stop,
                                                          CompressionType type = CompressionType.Gzip)
        {
            var rates = new CompressionRates();

            IEnumerable<IntervalSnapshots> daysSnapshots = CalculateDailySnapshots(person, start, stop);

            foreach (IntervalSnapshots day in daysSnapshots)
            {
                string suffix = ConfigurationManager.AppSettings["snapshotTextOutput"];
                string sep = ConfigurationManager.AppSettings["fileNamesSeparator"];
                string dateFormat = ConfigurationManager.AppSettings["fileDateFormat"];
                string dateFormated = String.Format(dateFormat, day.StartDate.Date);

                string toUncompressedPath = String.Concat(person.Id, sep, dateFormated, sep, suffix);

                IEnumerable<ActivityCodes> activityCodes = day.Data.Select(a => a.ActivityCode);
                IEnumerable<int> intActivityCodes = activityCodes.Cast<int>();

                double rate = GetCompressionRate(toUncompressedPath, intActivityCodes, type);

                rates.Rates.Add(rate);
            }

            rates.Person = person;

            return rates;
        }

        public Dictionary<int, double> GetAutoCorrelationsForShifts(Person person,
            int takeEveryNIndex = 0, bool performDataRandomization = false)
        {
            var snapshots = CalculateSnapshots(person);

            var data = (from record in snapshots.Data
                        select record.ActivityCode)
                       .Cast<int>();

            if (performDataRandomization)
            {
                data = data.GetFisherYatesShuffle();
            }

            return InternalGetAutoCorrelationsForAllShifts(data, takeEveryNIndex);
        }

        public Dictionary<int, double> GetConcurrentAutoCorrelationsForShifts(Person person,
            int takeEveryNIndex = 0, bool performDataRandomization = false)
        {
            var snapshots = CalculateSnapshots(person);

            var data = (from record in snapshots.Data
                        select record.ActivityCode)
                       .Cast<int>();

            var result = InternalConcurrentGetAutoCorrelationsForAllShifts(data, takeEveryNIndex)
                .OrderBy(pair => pair.Key)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            return result;
        }

        public Dictionary<int, double> GetAutoCorrelationsForShifts(IEnumerable<int> data,
          int takeEveryNIndex = 0)
        {
            return InternalGetAutoCorrelationsForAllShifts(data, takeEveryNIndex);
        }

        /// <summary>
        /// Returns single value of autocorrelation function as it is a stable line,
        /// with precission defined as an input parameter
        /// </summary>
        /// <returns>Avaraged autocorelation value</returns>
        public double GetAutoCorrelationsForRandomlyShiftedData<T>(
            IEnumerable<T> data, int precission = 1000)
        {
            data = data.GetFisherYatesShuffle();

            var result = new List<double>();
            var autoCor = new Autocorrelation<T>(data.ToArray());
            int max = Math.Min(precission, data.Count());

            for (int i = 1; i <= max; i++)
            {
                double value = autoCor.Compute(i);
                result.Add(value);
            }

            return result.Average();
        }

        public TimeCountingRecords CalculateCountingRecords(Person person)
        {
            //get all snapshots
            var snapshots = CalculateSnapshots(person);
            var trimmedSnapshots = TrimRecordsByDefinedTime(snapshots.Data);

            //calculate total time spent in different activities
            var countingRecords = new TimeCountingRecords();

            trimmedSnapshots.ToList().ForEach(
                record =>
                    countingRecords.Add(record, person.CheckpointRate));

            return countingRecords;
        }

        public IEnumerable<TimeCountingRecord> CalculateDailyCountingRecords(Person person)
        {
            var snapshots = CalculateSnapshots(person); //get all snapshots
            var trimmedSnapshots = TrimRecordsByDefinedTime(snapshots.Data);

            //calculate daily time spent in different activities
            var dayCountingRecords = new List<TimeCountingRecord>();

            //calculate total time spent in different activities
            var countingRecords = new TimeCountingRecords();

            var dayDate = new DateTime();
            bool isDayDateSet = false;

            //get total daily time of activities
            trimmedSnapshots.ToList().ForEach(
                record =>
                {
                    //get current record date
                    DateTime recordDate = record.Date;

                    //check if we set current day date
                    if (!isDayDateSet)
                    {
                        dayDate = record.Date;
                        isDayDateSet = true;
                    }

                    //compare current day date and record date
                    if (AreDatesEqual(recordDate, dayDate))
                    {
                        countingRecords.Add(record, person.CheckpointRate);
                    }
                    else
                    {
                        dayCountingRecords.Add(countingRecords.Records.Last());

                        isDayDateSet = false;

                        countingRecords = null;
                        countingRecords = new TimeCountingRecords();
                    }
                });

            return dayCountingRecords;
        }

        /// <summary>
        /// Removes records that are not within the frame and sleeping period in the morning
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        private IEnumerable<Record> TrimRecordsByDefinedTime(IEnumerable<Record> records)
        {
            var trimmedSnapshots = new List<Record>(); //remove stuff that is not needed
            bool shouldCopy = false;

            records.ToList().ForEach(
                 record =>
                 {
                     shouldCopy = IsRecordInValidDefinedTime(record, ref shouldCopy);

                     if (shouldCopy)
                     {
                         trimmedSnapshots.Add(record);
                     }
                 });

            return trimmedSnapshots;
        }

        public double GetCompressionRate<T>(string toUncompressedPath,
                              IEnumerable<T> data,
                              CompressionType compressionType = CompressionType.Gzip)
        {
            switch (compressionType)
            {
                case CompressionType.Gzip:
                    {
                        return GetGzipCompressionRatio(toUncompressedPath, data);
                    }
                case CompressionType.Lzma:
                    {
                        return GetLzmaCompressionRatio(toUncompressedPath, data);
                    }
                case CompressionType.Ppmd:
                    {
                        return GetPpmdCompressionRatio(toUncompressedPath, data);
                    }
                default:
                    throw new NotSupportedException();
            }

        }

        /// <summary>
        /// Returns all possible autocorrelation shifted by 1
        /// </summary>
        /// <returns>Dictionary with the key meaning shift and value is symbolic autocorrelation</returns>
        public Dictionary<int, double> InternalGetAutoCorrelationsForAllShifts<T>(
            IEnumerable<T> data, int shiftBy = 0)
        {
            var result = new Dictionary<int, double>();
            var autoCor = new Autocorrelation<T>(data.ToArray());

            //use only half as the function is symmetric
            int max = GetCeiledHalf(data.Count());

            //perform all cyclic shift
            //takes long time
            if (shiftBy == 0)
            {
                //skip first and last index, as it will give 1 - full cyclic loop
                for (int i = 0; i < max; i++)
                {
                    double value = autoCor.Compute(i);
                    result.Add(i, value);
                }
            }
            //shift only on specified indeces
            else
            {
                //skip first and last index, as it will give 1 - full cyclic loop
                int i = 0;
                while (i <= max)
                {
                    double value = autoCor.Compute(i);
                    result.Add(i, value);
                    i += shiftBy;
                }
            }

            return result;
        }

        private int GetCeiledHalf(int count)
        {
            double ceiledHalfLenght = Math.Ceiling((double)count / 2);
            return Convert.ToInt32(ceiledHalfLenght);
        }

        /// <summary>
        /// Returns all possible autocorrelation shifted by 1
        /// </summary>
        /// <returns>Dictionary with the key meaning shift and value is symbolic autocorrelation</returns>
        protected ConcurrentDictionary<int, double> InternalConcurrentGetAutoCorrelationsForAllShifts<T>(
            IEnumerable<T> data, int shiftBy = 0, int concurrencyLevel = 1)
        {
            int max = GetCeiledHalf(data.Count());
            var result = new ConcurrentDictionary<int, double>(concurrencyLevel, max);
            var autoCor = new Autocorrelation<T>(data.ToArray());

            //perform all cyclic shift
            //takes long time
            if (shiftBy == 0)
            {
                var options = new ParallelOptions
                                  {
                                      MaxDegreeOfParallelism = concurrencyLevel
                                  };

                Parallel.For(0, max, options,
                    i =>
                    {
                        double value = autoCor.Compute(i);
                        bool succesful = result.TryAdd(i, value);

                        if (!succesful)
                            throw new InvalidOperationException(
                                "Failed to add an item to concurrent dictionary. " +
                                "This index is already taken: " + i);
                    });

            }
            //shift only on specified indeces
            else
            {
                var options = new ParallelOptions
                {
                    MaxDegreeOfParallelism = concurrencyLevel
                };

                Parallel.ForEach(
                    Enumerable.Range(0, max) /*to include 0 shift*/
                    .Where(i => i % shiftBy == 0) /*select shifted points*/,
                    options,
                    (i, lo) =>
                    {
                        double value = autoCor.Compute(i);
                        bool succesful = result.TryAdd(i, value);

                        if (!succesful)
                            throw new InvalidOperationException(
                                "Failed to add an item to concurrent dictionary. " +
                                "This index is already taken: " + i);
                    });
            }

            return result;
        }

        private double GetPpmdCompressionRatio<T>(string toUncompressedPath, IEnumerable<T> data)
        {
            //write uncompressed
            IFileWriter fWriter = new FileWriter(toUncompressedPath);
            fWriter.WriteText(data);

            //var data = BinaryConverter.ToBinArray(day.Data);
            //BinaryFileWriter.Write(toUncompressedPath, FileMode.CreateNew, data);

            //compress 
            IArchiver archiver = new PPMDArchiver();
            var toBeEncodedFileInfo = new FileInfo(toUncompressedPath);
            string encodedPath = archiver.Compress(toBeEncodedFileInfo);
            var fiUncompressed = new FileInfo(toUncompressedPath);
            var fiCompressed = new FileInfo(encodedPath);

            //compression rate
            double rate = (double)fiUncompressed.Length / fiCompressed.Length;

            //remove the files
            fiUncompressed.Delete();
            fiCompressed.Delete();

            return rate;
        }

        private double GetLzmaCompressionRatio<T>(string toUncompressedPath, IEnumerable<T> data)
        {
            //write uncompressed
            IFileWriter fWriter = new FileWriter(toUncompressedPath);
            fWriter.WriteText(data);

            //var data = BinaryConverter.ToBinArray(day.Data);
            //BinaryFileWriter.Write(toUncompressedPath, FileMode.CreateNew, data);

            //compress 
            IArchiver archiver = new LZMAArchiver();
            var toBeEncodedFileInfo = new FileInfo(toUncompressedPath);
            string encodedPath = archiver.Compress(toBeEncodedFileInfo);
            var fiUncompressed = new FileInfo(toUncompressedPath);
            var fiCompressed = new FileInfo(encodedPath);

            //compression rate
            double rate = (double)fiUncompressed.Length / fiCompressed.Length;

            //remove the files
            fiUncompressed.Delete();
            fiCompressed.Delete();

            return rate;
        }
        public double CalculateAveragedCompressionRate<T>(IEnumerable<T> data, int iterations, CompressionType type)
        {
            var average = new BlockingDoubleAverage();
            for (int i = 0; i < iterations; i++)
            {
                string fName = "temp.txt";
                double rate = GetCompressionRate<T>(fName, data, type);
                average.Add(rate);
                File.Delete(fName);
            }

            return average.GetCurrentAverage();
        }


        private double GetGzipCompressionRatio<T>(string toUncompressedPath, IEnumerable<T> data)
        {
            IFileWriter fWriter = new FileWriter(toUncompressedPath);
            fWriter.WriteText(data);

            ////write uncompressed
            //var data = BinaryConverter.ToBinArray(day.Data);
            //BinaryFileWriter.Write(toUncompressedPath, FileMode.CreateNew, data);

            //compress 
            var fiUncompressed = new FileInfo(toUncompressedPath);
            string toCompressedPath = gzip.Compress(fiUncompressed);
            var fiCompressed = new FileInfo(toCompressedPath);

            //compression rate
            double rate = (double)fiUncompressed.Length / fiCompressed.Length;

            //remove the files
            fiUncompressed.Delete();
            fiCompressed.Delete();

            return rate;
        }

        private bool IsRecordInValidDefinedTime(Record record, ref bool previousState)
        {
            var startTime = new TimeSpan(04, 0, 0);
            var stopTime = new TimeSpan(22, 0, 0);

            var rd = record.Date;

            //copy date from records and time is predifined
            var startDate = new DateTime(rd.Year, rd.Month, rd.Day,
                                        startTime.Hours, startTime.Minutes, startTime.Seconds,
                                        startTime.Milliseconds);

            var stopDate = new DateTime(rd.Year, rd.Month, rd.Day,
                                        stopTime.Hours, stopTime.Minutes, stopTime.Seconds,
                                        stopTime.Milliseconds);

            return IsRecordInValidTime(startDate, stopDate, record, ref previousState);
        }

        /// <summary>
        /// Used to see if record is in the needed time frame and the person is not sleeping in the morning
        /// </summary>
        /// <param name="startDate">Start at first upright posture after longest period of lying down/sleeping</param>
        /// <param name="stopDate">when to stop</param>
        /// <param name="record">record to see if it is sleeping or not</param>
        /// <param name="previousState">we use it to switch it ON, once we detect any movement after some early morning time</param>
        /// <returns></returns>
        private bool IsRecordInValidTime(DateTime startDate, DateTime stopDate, Record record, ref bool previousState)
        {
            bool currentState = previousState;

            //between start and end 
            if (record.Date < startDate || record.Date > stopDate)
            {
                currentState = false;
            }
            else
            {
                //once we detect movement, we can have many sitting time
                //this is however not sleeping during the night
                //once reach nightime should copy will be reset again
                if (!currentState)
                {
                    //if sleepping - skip, otherwise record is in valid time
                    if (record.ActivityCode != ActivityCodes.Sit)
                    {
                        //not sleeping start copy till the end of the day
                        currentState = true;
                    }
                }
            }

            return currentState;
        }

        public BitArray CalculateCoarseGraining(Person person, int wordLength)
        {
            //calculate snapshots
            var snapshots = CalculateSnapshots(person);

            //convert snapshots to a long string of data
            string sequence = snapshots
                                .GetActivityCodeSequence()
                                .Cast<int>()
                                .AllToString();

            IHuffmanFixedLengthCoding c = new HuffmanFixedLengthCoding();
            c.Build(sequence, wordLength);
            var result = c.Encode();
            person.CoarseGrainedSequence = result;

            log.Debug("Total numer of words in sequence: " + c.SequenceInWords.Count());
            result.TraceEncoded();
            c.TraceWords();
            

            return result;
        }
    }
}