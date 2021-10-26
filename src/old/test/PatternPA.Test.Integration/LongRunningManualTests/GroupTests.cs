using System;
using System.Configuration;
using System.IO;
using System.Linq;
using NUnit.Framework;
using PatternPA.Core.Model;
using PatternPA.Utils.Entropy.MSE;

namespace PatternPA.Test.Integration.LongRunningManualTests
{
    [TestFixture]
    [Ignore("This set of tests are long running and are triggered only manually")]
    public class GroupTests : AbstractGroupTest
    {
        private string _fromDirectory;
        private TimeSpan _checkpointRate;
        private TimeSpan _fromTime;
        private TimeSpan _toTime;
        private int _shiftByPoints;

        [SetUp]
        public void SetUp()
        {
            _fromDirectory = ConfigurationManager.AppSettings["longRunningData"];
            Assert.IsNotNullOrEmpty(_fromDirectory);
            Assert.IsTrue(Directory.Exists(_fromDirectory));
            Assert.IsTrue(Directory.EnumerateFiles(_fromDirectory).Count() > 0);

             _checkpointRate = new TimeSpan(0, 0, 1);
            _fromTime = new TimeSpan(0, 0, 1);
            _toTime = new TimeSpan(23, 59, 59);
            _shiftByPoints = 600;
        }

        [Test]
        public void SaveCompressionRatesGzipAll()
        {
            var group = groupFactory.Create(_fromDirectory);
            group.CheckpointRate = _checkpointRate;
            var outputFiles = groupService.SaveCompressionRates(group,
                                                _fromTime,
                                                _toTime,
                                                 "AllCompressionRates.GZip.csv");

            Assert.IsNotNull(outputFiles);
            foreach (string outputFile in outputFiles)
            {
                Assert.IsTrue(File.Exists(outputFile));
                Assert.Greater(File.ReadAllBytes(outputFile).Length, 0);
            }
        }

        [Test]
        public void SaveCompressionRatesLzmaAll()
        {
            var group = groupFactory.Create(_fromDirectory);
            group.CheckpointRate = _checkpointRate;
            var outputFiles =  groupService.SaveCompressionRates(group,
                                                _fromTime,
                                                _toTime,
                                                 "AllCompressionRates.Lzma.csv",
                                                type: CompressionType.Lzma);

            Assert.IsNotNull(outputFiles);
            foreach (string outputFile in outputFiles)
            {
                Assert.IsTrue(File.Exists(outputFile));
                Assert.Greater(File.ReadAllBytes(outputFile).Length, 0);
            }
        }

        [Test]
        public void SaveCompressionRatesPpmdAll()
        {
            var group = groupFactory.Create(_fromDirectory);
            group.CheckpointRate = _checkpointRate;
            var outputFiles = groupService.SaveCompressionRates(group,
                                                _fromTime,
                                                _toTime,
                                                 "AllCompressionRates.Ppmd.csv",
                                                type: CompressionType.Ppmd);

            Assert.IsNotNull(outputFiles);
            foreach (string outputFile in outputFiles)
            {
                Assert.IsTrue(File.Exists(outputFile));
                Assert.Greater(File.ReadAllBytes(outputFile).Length, 0);
            }
        }

        [Test]
        public void SaveAutoCorrelationsForDefinedShiftsTest()
        {
            var group = groupFactory.Create(_fromDirectory);
            group.CheckpointRate = _checkpointRate;
            var outputFiles = groupService.SaveAutocorrelations(group, _shiftByPoints);

            Assert.IsNotNull(outputFiles);
            foreach (string outputFile in outputFiles)
            {
                Assert.IsTrue(File.Exists(outputFile));
                Assert.Greater(File.ReadAllBytes(outputFile).Length, 0);
            }
        }

        [Test]
        public void SaveAutoCorrelationsForDefinedShiftsWithShuffleTest()
        {
            var group = groupFactory.Create(_fromDirectory);
            group.CheckpointRate = _checkpointRate;
            var outputFiles = groupService.SaveAutocorrelations(group, _shiftByPoints,
                performDataRandomization:true);

            Assert.IsNotNull(outputFiles);
            foreach (string outputFile in outputFiles)
            {
                Assert.IsTrue(File.Exists(outputFile));
                Assert.Greater(File.ReadAllBytes(outputFile).Length, 0);
            }
        }

        [Test]
        public void SaveMseCalculationsTest_1min_WithArgs()
        {
            var group = groupFactory.Create(_fromDirectory);
            group.CheckpointRate = _checkpointRate;

            var builder = new MseArgumentBuilder();
            string args = builder.Build();

            var outputFiles = groupService.SaveMseCalculations(group, args);

            Assert.IsNotNull(outputFiles);
            foreach (string outputFile in outputFiles)
            {
                Assert.IsTrue(File.Exists(outputFile));
            }
        }
    }
}
