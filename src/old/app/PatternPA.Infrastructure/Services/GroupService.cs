using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PatternPA.Core.Model;
using PatternPA.Utils;
using PatternPA.Utils.Entropy.MSE;

namespace PatternPA.Infrastructure.Services
{
    public class GroupService : AbstractService
    {
        private readonly PersonCalculationService _personCalcService;
        private readonly PersonService _personService;
        private readonly AutocorrelationService _correlationService;

        public GroupService()
        {
            _personService = new PersonService();
            _personCalcService = new PersonCalculationService();
            _correlationService = new AutocorrelationService();
        }

        public void Initialize(Group group)
        {
            var di = new DirectoryInfo(group.DataFolderPath);
            FileInfo[] fileInfos = di.GetFiles();

            group.People = fileInfos.Select(
                fileInfo =>
                {
                    var p = new Person(fileInfo.FullName, group.CheckpointRate);
                    _personService.Initialize(p);

                    return p;
                });
        }


        /// <summary>
        /// Saves snapshots for all people files found in folder
        /// Creates sepparate file for all the people
        /// </summary>
        public void SaveSnapshotsSeparetely(Group group, string toFilePath)
        {
            Parallel.ForEach(
                group.People,
                person =>
                _personService.SaveSnapshots(person, toFilePath));
        }

        public void SaveSnapshotsForDefinedTime(Group group)
        {
            Parallel.ForEach(group.People, person =>
                        _personService.SaveSnapshotsForDefinedTime(person));
        }

        public void SaveCountingRecords(Group group, string toFilePath)
        {
            foreach (var person in group.People)
            {
                person.CheckpointRate = group.CheckpointRate;
                _personService.SaveCountingRecords(person, toFilePath);
                person.Dispose();
            }
        }

        public void SaveDayCountingRecords(Group group, string toFilePath)
        {
            foreach (var person in group.People)
            {
                person.CheckpointRate = group.CheckpointRate;
                _personService.SaveDailyCountingRecords(person, toFilePath);
                person.Dispose();
            }
        }

        public IEnumerable<string> SaveCompressionRates(Group group, TimeSpan start,
                                         TimeSpan stop,
                                         string toFile = null,
                                         CompressionType type = CompressionType.Gzip)
        {
            var bag = new ConcurrentBag<string>();

            Parallel.ForEach(group.People,
                person =>
                {
                    string result = _personService
                                        .SaveCompressionRates(person, toFile, start, stop,
                                        type);
                    bag.Add(result);
                });

            return bag.AsEnumerable();
        }

        public void SaveCumulativeStepCounts(Group group,
                                             string toFilePath,
                                             TimeSpan start,
                                             TimeSpan stop)
        {
            Parallel.ForEach(group.People, person =>
            {
                IEnumerable<IntervalSnapshots> snapshots = _personCalcService.CalculateDailySnapshots(
                    person, start, stop);

                if (String.IsNullOrEmpty(toFilePath))
                {
                    toFilePath = ConfigurationManager.AppSettings["cumulativeStepsSuffix"];
                }

                string sData = String.Empty;
                string data = sData;
                snapshots.ToList()
                    .ForEach(
                        snapshot =>
                        data = String.Concat(
                            data, snapshot.CumulativeStepCount, csvWriter.Separator));

                sData = sData.TrimEnd(csvWriter.Separator.ToCharArray());
                string record = String.Format("{0}{1}{2}", person.Id, csvWriter.Separator, sData);
                record = String.Concat(record, Environment.NewLine);

                csvWriter.WriteData(record, toFilePath, true, true);
            }
        );
        }



        public IEnumerable<string> SaveAutocorrelations(Group group,
                                             int shiftBy = 0,
                                             int degreeOfParallelism = 2,
                                             bool performDataRandomization = false)
        {
            var bag = new ConcurrentBag<string>();

            var options = new ParallelOptions
                              {
                                  MaxDegreeOfParallelism = degreeOfParallelism
                              };

            Parallel.ForEach(group.People, options, person =>
            {
                string toFilePath = String.Concat(person.Id,
                                        "Autocorrelations_",
                                        shiftBy, "_shiftPoints_",
                                        person.CheckpointRate.ToReadableString(),
                                        ".csv");
                var result = _personCalcService.GetAutoCorrelationsForShifts(
                                person, shiftBy, performDataRandomization);
                string output = _correlationService.SaveOne(result, toFilePath);
                bag.Add(output);
            });

            return bag.AsEnumerable();
        }

        public void SaveSnapshotsEntropies(Group group, string toFilePath)
        {
            Parallel.ForEach(
                group.People,
                person =>
                _personService.SaveSnapshotsEntropy(person, toFilePath));
        }

        public IEnumerable<string> SaveMseCalculations(Group group, string args)
        {
            var bag = new ConcurrentBag<string>();

            Parallel.ForEach(
                group.People,
                person =>
                {
                    if (String.IsNullOrEmpty(args))
                    {
                        var calculationService = new PersonCalculationService();
                        calculationService.CalculateSnapshots(person);
                        var lenght = person.Snapshots.Data.Count();

                        var builder = new MseArgumentBuilder();
                        args = builder
                            .WithMaxStopIndex(lenght - 1)
                            .Build();
                    }

                    string toFilePath = String.Concat(person.Id,
                                        "MseResult", ".txt");

                    string output = _personService.SaveMseCalculations(person, null, args);
                    bag.Add(output);
                }
                );

            return bag.AsEnumerable();
        }
    }
}