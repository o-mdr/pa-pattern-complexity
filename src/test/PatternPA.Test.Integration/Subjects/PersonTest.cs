using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NUnit.Framework;
using PatternPA.Container;
using PatternPA.Core.Interfaces;
using PatternPA.Core.Interfaces.Factories;
using PatternPA.Core.Model;
using PatternPA.Infrastructure.Factories;
using PatternPA.Infrastructure.Services;
using PatternPA.Utils.Entropy.MSE;

namespace PatternPA.Test.Integration.Subjects
{
    [TestFixture]
    public class PersonTest : AbstractIntegrationTest
    {
        private readonly string subjectFile = ConfigurationManager.AppSettings["sampleFilePath"];
        private readonly IPersonFactory personFactory = new PersonFactory();
        private readonly PersonService personService = new PersonService();
        private readonly PersonCalculationService personCalcService = new PersonCalculationService();

        [Test]
        public void InitializeTest()
        {
            var person = personFactory.Create(subjectFile);
            Assert.AreEqual(29073, person.ActivePalEvents.Count());
        }

        [Test]
        public void SaveSnapshotsTest_1hour()
        {
            var person = personFactory.Create(subjectFile);
            person.CheckpointRate = new TimeSpan(0, 1, 0, 0);
            personService.SaveSnapshots(person, null);

            Assert.AreEqual(29073, person.ActivePalEvents.Count());
        }

        [Test]
        public void SaveSnapshotsForMseTest_1hour()
        {
            var person = personFactory.Create(subjectFile);
            person.CheckpointRate = new TimeSpan(0, 1, 0, 0);
            string output = personService.SaveSnapshotsForMse(person, null);

            Assert.IsTrue(File.Exists(output));
            Assert.AreEqual(363, File.ReadAllBytes(output).Length);
        }

        [Test]
        public void SaveSnapshotsTest_100ms()
        {
            var person = personFactory.Create(subjectFile);
            person.CheckpointRate = new TimeSpan(0, 0, 0, 0, 100);
            personService.SaveSnapshots(person, null);

            Assert.AreEqual(29073, person.ActivePalEvents.Count());
        }

        [Test]
        public void SaveCountingRecords_100ms()
        {
            var person = personFactory.Create(subjectFile);
            person.CheckpointRate = new TimeSpan(0, 0, 0, 0, 100);
            personService.SaveCountingRecords(person, null);

            Assert.AreEqual(29073, person.ActivePalEvents.Count());
        }

        [Test]
        public void SaveSnapshotsForDefinedTime_100ms()
        {
            var person = personFactory.Create(subjectFile);
            person.CheckpointRate = new TimeSpan(0, 0, 0, 0, 100);
            personService.SaveSnapshotsForDefinedTime(person);

            Assert.AreEqual(29073, person.ActivePalEvents.Count());
        }

        [Test]
        public void SaveDailyCountingRecords_100ms()
        {
            var person = personFactory.Create(subjectFile);
            person.CheckpointRate = new TimeSpan(0, 0, 0, 0, 100);
            personService.SaveDailyCountingRecords(person, null);

            Assert.AreEqual(29073, person.ActivePalEvents.Count());
        }

        [Test]
        public void SaveSnapshotsEntropyTest()
        {
            var person = personFactory.Create(subjectFile);
            personService.SaveSnapshotsEntropy(person, null);

            Assert.AreEqual(29073, person.ActivePalEvents.Count());
        }

        [Test]
        public void GetGZipCompressionRateTest()
        {
            var person = personFactory.Create(subjectFile);
            person.CheckpointRate = new TimeSpan(0, 0, 0, 0, 100);

            person.StartFrom = new DateTime(2007, 10, 27, 6, 0, 0);
            person.StopAt = new DateTime(2007, 10, 27, 22, 0, 0);
            personService.SaveCompressionRate(person, null);

            Assert.AreEqual(29073, person.ActivePalEvents.Count());
        }

        [Test]
        public void GetLzmaCompressionRateTest()
        {
            var person = personFactory.Create(subjectFile);
            person.CheckpointRate = new TimeSpan(0, 0, 1);

            person.StartFrom = new DateTime(2007, 10, 27, 6, 0, 0);
            person.StopAt = new DateTime(2007, 10, 27, 22, 0, 0);
            personService.SaveCompressionRate(person, null, CompressionType.Lzma);

            Assert.AreEqual(29073, person.ActivePalEvents.Count());
        }

        [Test]
        public void GetPpmdCompressionRateTest()
        {
            var person = personFactory.Create(subjectFile);
            person.CheckpointRate = new TimeSpan(0, 0, 0, 0, 100);

            person.StartFrom = new DateTime(2007, 10, 27, 6, 0, 0);
            person.StopAt = new DateTime(2007, 10, 27, 22, 0, 0);
            personService.SaveCompressionRate(person, null, CompressionType.Ppmd);

            Assert.AreEqual(29073, person.ActivePalEvents.Count());
        }

        [Test]
        public void GetAutocorrelationForOneDayTest()
        {
            var person = personFactory.Create(subjectFile);
            person.CheckpointRate = new TimeSpan(0, 0, 0, 0, 100);

            person.StartFrom = new DateTime(2007, 10, 27, 6, 0, 0);
            person.StopAt = new DateTime(2007, 10, 27, 22, 0, 0);
            personService.SaveCompressionRate(person, null, CompressionType.Ppmd);

            Assert.AreEqual(29073, person.ActivePalEvents.Count());
        }

        [Test]
        public void GetGZipCompressionRatesTest()
        {
            var person = personFactory.Create(subjectFile);
            person.CheckpointRate = new TimeSpan(0, 0, 0, 0, 100);
            personCalcService.CalculateCompressionRates(person,
                new TimeSpan(0, 6, 0, 0), new TimeSpan(0, 22, 0, 0));

            Assert.AreEqual(29073, person.ActivePalEvents.Count());
        }

        [Test]
        public void GetGZipCompressionRatesWithArtificialSedentryBehaviorTest()
        {
            int size = 7*24*60; //7 days 24 hours 60 minutes
            int max = 10;
            CompressionType type = CompressionType.Gzip;
            int[] artificialData = new int[size];

            double result = personCalcService.CalculateAveragedCompressionRate(artificialData,max, type);

            Trace.WriteLine("max: " + max);
            Trace.WriteLine("size: " + size);
            Trace.WriteLine("type: CompressionType.Gzip");
            Trace.WriteLine("Averaged compression: " + result);
        }

        [Test]
        public void GetLZMACompressionRatesWithArtificialSedentryBehaviorTest()
        {
            int size = 7 * 24 * 60; //7 days 24 hours 60 minutes
            int max = 10;
            CompressionType type = CompressionType.Lzma;
            int[] artificialData = new int[size];

            double result = personCalcService.CalculateAveragedCompressionRate(artificialData, max, type);

            Trace.WriteLine("max: " + max);
            Trace.WriteLine("size: " + size);
            Trace.WriteLine("type: CompressionType.Lzma");
            Trace.WriteLine("Averaged compression: " + result);
        }

        [Test]
        public void GetPPMDCompressionRatesWithArtificialSedentryBehaviorTest()
        {
            int size = 7 * 24 * 60; //7 days 24 hours 60 minutes
            int max = 10;
            CompressionType type = CompressionType.Ppmd;
            int[] artificialData = new int[size];

            double result = personCalcService.CalculateAveragedCompressionRate(artificialData, max, type);

            Trace.WriteLine("max: " + max);
            Trace.WriteLine("size: " + size);
            Trace.WriteLine("type: CompressionType.Ppmd");
            Trace.WriteLine("Averaged compression: " + result);
        }

        [Test]
        public void GetLzmaCompressionRatesTest()
        {
            var person = personFactory.Create(subjectFile);
            person.CheckpointRate = new TimeSpan(0, 0, 0, 0, 100);
            personCalcService.CalculateCompressionRates(person,
                new TimeSpan(0, 6, 0, 0), new TimeSpan(0, 22, 0, 0), CompressionType.Lzma);

            Assert.AreEqual(29073, person.ActivePalEvents.Count());
        }

        [Test]
        public void GetPpmdCompressionRatesTest()
        {
            var person = personFactory.Create(subjectFile);
            person.CheckpointRate = new TimeSpan(0, 0, 0, 0, 100);
            personCalcService.CalculateCompressionRates(person,
                new TimeSpan(0, 6, 0, 0), new TimeSpan(0, 22, 0, 0), CompressionType.Ppmd);

            Assert.AreEqual(29073, person.ActivePalEvents.Count());
        }


        [Test]
        public void SaveGZipCompressionRatesTest()
        {
            var person = personFactory.Create(subjectFile);
            person.CheckpointRate = new TimeSpan(0, 0, 0, 0, 100);
            personService.SaveCompressionRates(person, null,
                                new TimeSpan(0, 6, 0, 0), new TimeSpan(0, 22, 0, 0));

            Assert.AreEqual(29073, person.ActivePalEvents.Count());
        }

        [Test]
        public void GetAutoCorrelationsForAllShiftsTest()
        {
            var person = personFactory.Create(subjectFile);
            person.CheckpointRate = new TimeSpan(0, 0, 0, 1);

            person.StartFrom = new DateTime(2007, 10, 27, 6, 0, 0);
            person.StopAt = new DateTime(2007, 10, 27, 10, 0, 0);

            var result = personCalcService.GetAutoCorrelationsForShifts(person);

            Assert.Pass();
        }

        [Test]
        public void GetConcurrentAutoCorrelationsForAllShiftsTest()
        {
            var person = personFactory.Create(subjectFile);
            person.CheckpointRate = new TimeSpan(0, 0, 0, 1);

            person.StartFrom = new DateTime(2007, 10, 27, 6, 0, 0);
            person.StopAt = new DateTime(2007, 10, 27, 10, 0, 0);

            var result = personCalcService.GetConcurrentAutoCorrelationsForShifts(person);

            Assert.Pass();
        }

        [Test]
        public void CompareConcurrentAndSequentialGetAutoCorrelationsForAllShiftsTest()
        {
            var person = personFactory.Create(subjectFile);
            person.CheckpointRate = new TimeSpan(0, 0, 0, 1);

            person.StartFrom = new DateTime(2007, 10, 27, 6, 0, 0);
            person.StopAt = new DateTime(2007, 10, 27, 10, 0, 0);

            var resultSequential = personCalcService.GetAutoCorrelationsForShifts(person);
            var resultConcurrent = personCalcService.GetConcurrentAutoCorrelationsForShifts(person);

            Assert.That(resultSequential.Keys, Is.EqualTo(resultConcurrent.Keys));
            Assert.That(resultSequential.Values, Is.EqualTo(resultConcurrent.Values));
        }

        [Test]
        public void GetAutoCorrelationsForDefinedShiftTest()
        {
            var person = personFactory.Create(subjectFile);
            //real value
            //person.CheckpointRate = new TimeSpan(0, 0, 0, 0, 100);
            //test value
            person.CheckpointRate = new TimeSpan(0, 10, 0);
            int shiftBy = 600; //shift 600 points = 1 min shift

            var result = personCalcService.GetAutoCorrelationsForShifts(person, shiftBy);

            Assert.Pass();
        }

        [Test]
        public void CompareConcurrentAndSequentialGetAutoCorrelationsForDefinedShiftTest()
        {
            var person = personFactory.Create(subjectFile);
            //real value
            //person.CheckpointRate = new TimeSpan(0, 0, 0, 0, 100);
            //int shiftBy = 600; //shift 600 points = 1 min shift

            //test value
            person.CheckpointRate = new TimeSpan(0, 10, 0);
            int shiftBy = 10;


            var resultSequential = personCalcService.GetAutoCorrelationsForShifts(person, shiftBy);
            var resultConcurrent = personCalcService.GetConcurrentAutoCorrelationsForShifts(person, shiftBy);

            Assert.AreEqual(resultSequential.Count, resultConcurrent.Count);
            Assert.That(resultSequential.Keys, Is.EqualTo(resultConcurrent.Keys));
            Assert.That(resultSequential.Values, Is.EqualTo(resultConcurrent.Values));
        }

        [Test]
        public void GetAutoCorrelationsForDefinedShiftAndCompareTorandomizedTest()
        {
            var person = personFactory.Create(subjectFile);
            person.CheckpointRate = new TimeSpan(0, 0, 1, 0);
            int shiftBy = 600; //shift 600 points = 1 min shift

            var resultReal = personCalcService.GetAutoCorrelationsForShifts(person,
                            shiftBy);

            var resulShuffled = personCalcService.GetAutoCorrelationsForShifts(person,
                            shiftBy, true);

            Assert.That(resultReal.Values, Is.Not.EquivalentTo(resulShuffled.Values));
            Assert.That(resultReal.Values, Is.Not.EqualTo(resulShuffled.Values));
        }

        [Test]
        public void SaveAutoCorrelationsForAllShiftsTest()
        {
            var person = personFactory.Create(subjectFile);
            person.CheckpointRate = new TimeSpan(0, 0, 0, 1);
            person.StartFrom = new DateTime(2007, 10, 27, 6, 0, 0);
            person.StopAt = new DateTime(2007, 10, 27, 6, 10, 0);

            personService.SaveAutocorrelations(person);

            Assert.Pass();
        }

        [Test]
        public void SaveAutoCorrelationsForDefinedMinuteShiftsTest()
        {
            var person = personFactory.Create(subjectFile);
            //real value
            //person.CheckpointRate = new TimeSpan(0, 0, 0, 0, 100);
            //test value
            person.CheckpointRate = new TimeSpan(0, 10, 0);
            int shiftByValue = 600; //shift 600 points = 1 min shift

            personService.SaveAutocorrelations(person, shiftBy: shiftByValue);

            Assert.Pass();
        }

        [Test]
        public void SaveAutoCorrelationsForDefinedMinuteShiftsWithDataRandomlyshiftedTest()
        {
            var person = personFactory.Create(subjectFile);
            //real value
            //person.CheckpointRate = new TimeSpan(0, 0, 0, 0, 100);
            //test value
            person.CheckpointRate = new TimeSpan(0, 1, 0);
            personService.SaveAutocorrelationsWithRandomShift(person, precission: 10);
            Assert.Pass();
        }

        [Test]
        public void SaveAutoCorrelationsForRandomDataWithDefinedMinuteShiftsTest()
        {
            var rndEventGenerator = ContainerService.Instance.Resolve<IRandomEventGenerator>();

            //int sequenceLenght = 4320000;
            int sequenceLenght = 24000;
            IEnumerable<int> availableEvents = new[] { 0, 1, 2 };
            IEnumerable<int> sequence = rndEventGenerator.GenerateRandomEvents(availableEvents,
                                                                               sequenceLenght);
            personService.SaveAutocorrelations(sequence);
            Assert.Pass();
        }

        [Test]
        public void SaveMseCalculationsTest_1hour()
        {
            var person = personFactory.Create(subjectFile);
            person.CheckpointRate = new TimeSpan(1, 0, 0);
            string output = personService.SaveMseCalculations(person, null);

            Assert.IsTrue(File.Exists(output));
            Assert.AreEqual(236, File.ReadAllBytes(output).Length);
        }

        [Test]
        public void SaveMseCalculationsTest_1min_WithArgs()
        {
            var person = personFactory.Create(subjectFile);
            person.CheckpointRate = new TimeSpan(0, 1, 0);

            var calculationService = new PersonCalculationService();
            calculationService.CalculateSnapshots(person);
            var lenght = person.Snapshots.Data.Count();

            var builder = new MseArgumentBuilder();
            string args = builder
                .WithMaxPatternLenght(9)
                .WithMaxSimilarityCriterion(0.3)
                .WithMaxStopIndex(lenght - 1)
                .Build();

            string output = personService.SaveMseCalculations(person, null, args);

            Assert.IsTrue(File.Exists(output));
            Assert.AreEqual(7490, File.ReadAllBytes(output).Length);
        }
    }
}