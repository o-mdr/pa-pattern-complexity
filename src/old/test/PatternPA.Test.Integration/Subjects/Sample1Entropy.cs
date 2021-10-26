using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using NUnit.Framework;
using PatternPA.Core.Model;

namespace PatternPA.Test.Integration.Subjects
{
    [TestFixture]
    public class Sample1Entropy : SubjectTest
    {
        private readonly string subjectFile = ConfigurationManager.AppSettings["sampleFilePath"];
        private static string entropyPath = "Entropy.csv";

        private EntropyResult CalculateEntropy(DateTime start, DateTime stop, TimeSpan interval)
        {
            recordConverter.ActivityCheck = interval;
            var events = GetEvents(subjectFile);
            IntervalSnapshots checkpoints = recordConverter.ToCheckpointRecords(
                                                                events, 
                                                                start, 
                                                                stop, 
                                                                interval);
            var entropy = new Entropy();

            return entropy.CalculateShannonEntropy(checkpoints, alphabet.GetAlphabet());
        }

        private EntropyResult CalculateEntropy(TimeSpan interval)
        {
            return CalculateEntropy(GetEvents(subjectFile).First().Date, GetEvents(subjectFile).Last().Date, interval);
        }

        private void SaveData<T>(string path, IEnumerable<T> data)
        {
            csvWriter.OutputFilePath = path;
            csvWriter.WriteData(data, null);
        }

        #region All week entropy

        [Test]
        public void TestEntropy_20PM_1hour()
        {
            var start = new TimeSpan(0, 6, 0, 0);
            var checkPointRate = new TimeSpan(0, 1, 0, 0);

            EntropyResult result = CalculateEntropy(checkPointRate);
            Assert.AreEqual(0.94681062804085736, result.EntropyValue);
        }

        [Test]
        public void TestEntropy_20PM_1minute()
        {
            var start = new TimeSpan(0, 6, 0, 0);
            var checkPointRate = new TimeSpan(0, 0, 1, 0);

            EntropyResult result = CalculateEntropy(checkPointRate);
            Assert.AreEqual(0.93538061199532574, result.EntropyValue);
        }

        [Test]
        public void TestEntropy_20PM_1sec()
        {
            var start = new TimeSpan(0, 6, 0, 0);
            var checkPointRate = new TimeSpan(0, 0, 0, 1);

            EntropyResult result = CalculateEntropy(checkPointRate);
            Assert.AreEqual(0.94035920104979653, result.EntropyValue);
        }

        [Test]
        public void TestEntropy_20PM_01sec()
        {
            var start = new TimeSpan(0, 6, 0, 0);
            var checkPointRate = new TimeSpan(0, 0, 0, 0, 100);

            EntropyResult result = CalculateEntropy(checkPointRate);
            Assert.AreEqual(0.94022441944856294, result.EntropyValue);
        }

        #endregion

        #region One day entropy

        [Test]
        public void TestEntropy_Day3Shift_1sec()
        {
            var shiftedStart = new DateTime(2007, 10, 27, 6, 0, 0);
            var shiftedStop = new DateTime(2007, 10, 28, 6, 0, 0);
            var shiftBy = new TimeSpan(0, 0, 0, 0, 100);
            var checkPointRate = new TimeSpan(0, 0, 1, 0);

            //var fullStop = shiftedStop;
            var fullStop = new DateTime(2007, 10, 27, 6, 1, 0);

            var results = new List<double>();

            while (shiftedStart < fullStop)
            {
                EntropyResult result = CalculateEntropy(shiftedStart, shiftedStop, checkPointRate);
                results.Add(result.EntropyValue);
                shiftedStart = shiftedStart.Add(shiftBy);
                shiftedStop = shiftedStop.Add(shiftBy);
            }

            SaveData(String.Format(entropyPath), results);
            Assert.AreEqual(600, results.Count);
        }

        [Test]
        public void TestEntropy_Day3_01sec()
        {
            var start = new DateTime(2007, 10, 27, 6, 0, 0);
            var stop = new DateTime(2007, 10, 28, 6, 0, 0);
            var checkPointRate = new TimeSpan(0, 0, 0, 0, 100);

            var results = new List<double>();

            EntropyResult result = CalculateEntropy(start, stop, checkPointRate);
            results.Add(result.EntropyValue);

            SaveData(String.Format(entropyPath), results);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(0.84262640775174968, results[0]);
        }

        #endregion
    }
}