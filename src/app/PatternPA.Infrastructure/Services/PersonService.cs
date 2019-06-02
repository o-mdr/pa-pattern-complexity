using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using PatternPA.Core.Model;
using PatternPA.Utils;
using PatternPA.Utils.Entropy.MSE;

namespace PatternPA.Infrastructure.Services
{
    public class PersonService : AbstractService
    {
        private readonly PersonCalculationService calculationService;
        private readonly CompressionRatesService compRateService;
        private readonly RecordService<Record> recordService;
        private readonly EntropyResultService entropyService;
        private readonly IntervalSnapshotsService snapService;
        private readonly AutocorrelationService correlationService;

        public PersonService()
        {
            calculationService = new PersonCalculationService();
            snapService = new IntervalSnapshotsService();
            entropyService = new EntropyResultService();
            compRateService = new CompressionRatesService();
            recordService = new RecordService<Record>();
            correlationService = new AutocorrelationService();
        }

        /// <summary>
        /// Use person factory to create and init a person
        /// </summary>
        /// <param name="person"></param>
        internal void Initialize(Person person)
        {
            var info = new FileInfo(person.DataFilePath);
            int idx = info.Name.IndexOf(
                ConfigurationManager.AppSettings["peronDataFileSuffix"]);

            person.Id = info.Name.Remove(idx);

            int skipRows = Convert.ToInt32(ConfigurationManager.AppSettings["skipHeaderCount"]);
            person.ActivePalEvents = csvParser.ParseCsv(person.DataFilePath, skipRows);
        }

        public string SaveSnapshots(Person person, string toFilePath,
                                    bool asOneLineString = false,
                                    bool saveJustEventCodes = false,
                                    string separator = ", ")
        {
            if (String.IsNullOrEmpty(toFilePath))
            {
                toFilePath = String.Concat(person.Id, "outputSnapshots.csv");
            }

            IntervalSnapshots snapshots = calculationService.CalculateSnapshots(person);
            string output = snapService.Save(snapshots, toFilePath, asOneLineString,
                saveJustEventCodes, separator: separator);

            return output;
        }

        public string SaveSnapshotsForMse(Person person, string toFilePath)
        {
            return SaveSnapshots(person, toFilePath, saveJustEventCodes: true, separator: ""); ;
        }

        public string SaveMseCalculations(Person person, string toFilePath, string mseArgs = "")
        {
            string mseInputFile = SaveSnapshotsForMse(person, toFilePath);
            var entropy = new SingleFileMse(mseInputFile);
            string output = entropy.Compute(mseArgs);

            return output;
        }

        public void SaveSnapshotsForDefinedTime(Person person)
        {

            string toFilePathTemplate = String.Concat(person.Id, "_{0}", ConfigurationManager.AppSettings["CSVExtention"]);


            var trimmedSnapshots = calculationService.CalculateSnapshotsForDefinedTime(person);

            var dayRecords = new List<Record>(); //store daily records

            //var countingRecords = new TimeCountingRecords();
            var dayDate = new DateTime();
            bool isDayDateSet = false;

            //save records on a daily basis
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
                    //dates the same, add to temp storage
                    if (AreDatesEqual(recordDate, dayDate))
                    {
                        dayRecords.Add(record);
                    }
                    else
                    {
                        //dates differ => we move to the next day
                        //save
                        string toFilePath = String.Format(toFilePathTemplate, (int)dayDate.DayOfWeek);
                        recordService.Save(dayRecords, toFilePath);

                        isDayDateSet = false;
                        dayRecords.Clear();
                    }
                });
        }

        public void SaveCountingRecords(Person person, string toFilePath)
        {
            if (String.IsNullOrEmpty(toFilePath))
            {
                toFilePath = String.Concat(person.Id, ConfigurationManager.AppSettings["CSVExtention"]);
            }

            var countingRecords = calculationService.CalculateCountingRecords(person);
            recordService.Save(countingRecords.Records, toFilePath);
        }

        public void SaveDailyCountingRecords(Person person, string toFilePath)
        {
            if (String.IsNullOrEmpty(toFilePath))
            {
                toFilePath = String.Concat(person.Id, ConfigurationManager.AppSettings["CSVExtention"]);
            }

            var dailyRecords = calculationService.CalculateDailyCountingRecords(person);
            recordService.Save(dailyRecords, toFilePath);
        }

        public void SaveSnapshotsEntropy(Person person, string toFilePath)
        {
            if (String.IsNullOrEmpty(toFilePath))
            {
                toFilePath = String.Concat(person.Id, "outputSnapshotsEntropy.csv");
            }

            EntropyResult result = calculationService.CalculateEntropy(person);
            entropyService.Save(result, toFilePath);
        }

        public void SaveCompressionRate(Person person, string toFilePath, CompressionType type =
                                                                            CompressionType.Gzip)
        {
            if (String.IsNullOrEmpty(toFilePath))
            {
                toFilePath = String.Concat(person.Id, "CompressionRate.txt");
            }

            double rate = calculationService.CalculateCompressionRate(person, type);

            string txt = String.Format("Snaphot count: {0}. Compression rate {1}",
                                       person.Snapshots.Data.Count(), rate);
            fileWriter.WriteText(txt);
        }

        public string SaveCompressionRates(Person person, string toFilePath,
                                        TimeSpan start, TimeSpan stop,
                                        CompressionType type = CompressionType.Gzip)
        {
            if (String.IsNullOrEmpty(toFilePath))
            {
                toFilePath = "CompressionRates.csv";
            }

            CompressionRates rates = calculationService.CalculateCompressionRates(person, start, stop, type);
            string output = compRateService.Save(rates, toFilePath);
            return output;
        }

        public void SaveAutocorrelations(Person person, string toFilePath = null, int shiftBy = 0)
        {
            if (String.IsNullOrEmpty(toFilePath))
            {
                toFilePath = String.Concat(person.Id, "Autocorrelations_",
                                            shiftBy, "_shiftPoints_",
                                            person.CheckpointRate.ToReadableString(),
                                            ".csv");
            }

            var result = calculationService.GetAutoCorrelationsForShifts(person, shiftBy);
            correlationService.SaveOne(result, toFilePath);
        }

        public void SaveAutocorrelations(IEnumerable<int> data, string toFilePath = null,
           int shiftBy = 0)
        {
            if (String.IsNullOrEmpty(toFilePath))
            {
                toFilePath = String.Concat("RandomgeneratedData", "Autocorrelations_",
                                            shiftBy, "_shiftPoints", ".csv");
            }

            var result = calculationService.GetAutoCorrelationsForShifts(data, shiftBy);
            correlationService.SaveOne(result, toFilePath);
        }

        public void SaveAutocorrelationsWithRandomShift(Person person,
            string toFilePath = null, int precission = 1000)
        {
            if (String.IsNullOrEmpty(toFilePath))
            {
                toFilePath = String.Concat(person.Id,
                    "Autocorrelations_RandomlyShiftedAveragedData", ".txt");
            }

            var snapshots = calculationService.CalculateSnapshots(person);

            var data = (from record in snapshots.Data
                        select record.ActivityCode)
                       .Cast<int>();

            var result = calculationService.GetAutoCorrelationsForRandomlyShiftedData(data, precission);
            correlationService.SaveRandomlyShiftedData(result, precission, toFilePath);
        }

    }
}