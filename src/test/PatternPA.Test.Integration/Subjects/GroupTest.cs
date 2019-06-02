using System;
using System.Configuration;
using System.IO;
using System.Linq;
using NUnit.Framework;
using PatternPA.Core.Model;
using PatternPA.Utils.Entropy.MSE;

namespace PatternPA.Test.Integration.Subjects
{
    [TestFixture]
    public class GroupTest : AbstractGroupTest
    {
        [Test]
        public void SaveSnapshotsSeparetelyTest()
        {
            string from = ConfigurationManager.AppSettings["contrastFloder"];
            var group = groupFactory.Create(from);
            groupService.SaveSnapshotsSeparetely(group, null);
            Assert.AreEqual(2, group.People.ToList().Count);
        }

        [Test]
        public void SaveSnapshotsSeparetelyContrast()
        {
            var group = groupFactory.Create(null);
            group.CheckpointRate = new TimeSpan(0, 0, 1);
            groupService.SaveSnapshotsSeparetely(group, null);
            Assert.AreEqual(10, group.People.ToList().Count);
        }

        [Test]
        public void SaveCountingRecordsTest()
        {
            string groupPath = ConfigurationManager.AppSettings["sampleGroup"];
            var group = groupFactory.Create(groupPath);
            group.CheckpointRate = new TimeSpan(0, 0, 10);
            groupService.SaveCountingRecords(group, null);

            Assert.AreEqual(10, group.People.ToList().Count);
        }

        [Test]
        public void SaveDailyCountingRecordsTest()
        {
            string groupPath = ConfigurationManager.AppSettings["sampleGroup"];
            var group = groupFactory.Create(groupPath);
            group.CheckpointRate = new TimeSpan(0, 0, 10);
            groupService.SaveDayCountingRecords(group, null);

            Assert.AreEqual(10, group.People.ToList().Count);
        }

        [Test]
        public void SaveSnapshotsTest()
        {
            var group = groupFactory.Create(null);
            group.CheckpointRate = new TimeSpan(0, 0, 1);
            groupService.SaveCompressionRates(group, new TimeSpan(0, 6, 0, 0), new TimeSpan(0, 22, 0, 0));

            Assert.AreEqual(10, group.People.ToList().Count);
        }

        [Test]
        public void SaveSnapshotsForDefinedTimeTest()
        {
            string from = ConfigurationManager.AppSettings["allDataFolder"];
            var group = groupFactory.Create(from);
            group.CheckpointRate = new TimeSpan(0, 0, 10);
            groupService.SaveSnapshotsForDefinedTime(group);

            Assert.AreEqual(10, group.People.ToList().Count);
        }

        [Test]
        public void SaveSnapshotsForDefinedTimeTestContrast()
        {
            string from = ConfigurationManager.AppSettings["contrastFloder"];
            var group = groupFactory.Create(from);
            group.CheckpointRate = new TimeSpan(0, 0, 1);
            groupService.SaveSnapshotsForDefinedTime(group);

            Assert.AreEqual(2, group.People.ToList().Count);
        }

        [Test]
        public void SaveCompressionRatesGzipAll()
        {
            string from = ConfigurationManager.AppSettings["allDataFolder"];
            var group = groupFactory.Create(from);
            group.CheckpointRate = new TimeSpan(0, 0, 1);
            groupService.SaveCompressionRates(  group, 
                                                new TimeSpan(0, 6, 0, 0), 
                                                new TimeSpan(0, 22, 0, 0),
                                                 "CompressionRates.GZip.csv");

            Assert.AreEqual(10, group.People.ToList().Count);
        }

        [Test]
        public void SaveCompressionRatesLzmaAll()
        {
            string from = ConfigurationManager.AppSettings["allDataFolder"];
            var group = groupFactory.Create(from);
            group.CheckpointRate = new TimeSpan(0, 0, 1);
            groupService.SaveCompressionRates(  group, 
                                                new TimeSpan(0, 6, 0, 0),
                                                new TimeSpan(0, 22, 0, 0), 
                                                 "CompressionRates.Lzma.csv",
                                                type: CompressionType.Lzma);

            Assert.AreEqual(10, group.People.ToList().Count);
        }

        [Test]
        public void SaveCompressionRatesPpmdAll()
        {
            string from = ConfigurationManager.AppSettings["allDataFolder"];
            var group = groupFactory.Create(from);
            group.CheckpointRate = new TimeSpan(0, 0, 1);
            groupService.SaveCompressionRates(  group, 
                                                new TimeSpan(0, 6, 0, 0),
                                                new TimeSpan(0, 22, 0, 0),
                                                "CompressionRates.Ppmd.csv", 
                                                type: CompressionType.Ppmd);

            Assert.AreEqual(10, group.People.ToList().Count);
        }

        [Test]
        public void SaveCumulativeStepCountsTest()
        {
            string from = ConfigurationManager.AppSettings["allDataFolder"];
            var group = groupFactory.Create(from);
            group.CheckpointRate = new TimeSpan(0, 0, 1);
            groupService.SaveCumulativeStepCounts(group, null, new TimeSpan(0, 6, 0, 0), new TimeSpan(0, 22, 0, 0));

            Assert.AreEqual(10, group.People.ToList().Count);
        }

        [Test]
        public void SaveSnapshotsEntropiesTest()
        {
            var group = groupFactory.Create(null);
            groupService.SaveSnapshotsEntropies(group, null);

            Assert.AreEqual(10, group.People.ToList().Count);
        }

        [Test]
        public void SaveAutoCorrelationsWithNoShiftTest()
        {
            string from = ConfigurationManager.AppSettings["allDataFolder"];
            var group = groupFactory.Create(from);
            group.CheckpointRate = new TimeSpan(1, 0, 0);
            groupService.SaveAutocorrelations(group);

            Assert.AreEqual(10, group.People.ToList().Count);
        }

        [Test]
        public void SaveAutoCorrelationsForDefinedShiftsTest()
        {
            string from = ConfigurationManager.AppSettings["allDataFolder"];
            var group = groupFactory.Create(from);
            group.CheckpointRate = new TimeSpan(1, 0, 0);
            groupService.SaveAutocorrelations(group, 600);
            Assert.AreEqual(10, group.People.ToList().Count);
        }

        [Test]
        public void SaveMseCalculationsTest_1min_WithArgs()
        {
            string from = ConfigurationManager.AppSettings["allDataFolder"];
            var group = groupFactory.Create(from);
            group.CheckpointRate = new TimeSpan(1, 0, 0);

            var builder = new MseArgumentBuilder();
            string args = builder
                .WithMaxPatternLenght(3)
                .WithMaxSimilarityCriterion(0.3)
                .Build();

            var outputFiles = groupService.SaveMseCalculations(group, args);

            Assert.IsNotNull(outputFiles);
            foreach (string outputFile in outputFiles)
            {
                Assert.IsTrue(File.Exists(outputFile));
                Assert.AreEqual(1874, File.ReadAllBytes(outputFile).Length);
            }
            
            Assert.AreEqual(10, group.People.ToList().Count);
        }
    }
}