using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using NUnit.Framework;
using PatternPA.Core.Model;

namespace PatternPA.Test.Integration.Subjects
{
    [TestFixture]
    public class Sample1 : SubjectTest
    {
        private string subjectFile = ConfigurationManager.AppSettings["sampleFilePath"];

        [Test]
        public void WriteDataTableTest_010000()
        {
            var result = DoConversion(subjectFile, new TimeSpan(0, 1, 0, 0));
            Assert.AreEqual(142, result.Data.Count());
        }

        private IntervalSnapshots DoConversion(string filePath, TimeSpan interval)
        {
            IEnumerable<EventRecord> events = csvParser.ParseCsv(filePath, 1);

            recordConverter.ActivityCheck = interval;

            return recordConverter.ToCheckpointRecords(
                                     events,
                                     events.First().Date,
                                     events.Last().Date, interval);
        }
    }
}