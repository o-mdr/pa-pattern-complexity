using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NUnit.Framework;
using PatternPA.Core.Interfaces.Nhanes;
using PatternPA.Core.Model.Nhanes;

namespace PatternPA.Test.Core.Model
{
    [TestFixture]
    public class NhanesCsvParserTest
    {
        private string file = "Data\\resultArray0.csv";
        //private string file2003_2004Format = "Data\\resultArray0.csv";

        [SetUp]
        public void SetUp()
        {
            if (!File.Exists(file))
                throw new FileNotFoundException("Cannot load test file", file);
        }

        [Test]
        public void ParseCsvTest()
        {
            var time = TimeAction(() =>
            {
                INhanesCsvParser parser = new NhanesCsvParser();

                var records = parser.ParseCsv(file);

                Assert.IsNotNull(records);
                Assert.AreEqual(1000000, records.Count());
            });
            Trace.WriteLine("Time: " + time);
        }

        //[Test]
        //public void ParseCsvSelect2003_2004FormatTest()
        //{
        //    throw new NotImplementedException("This test must be implemented for reading per person csv file in different format 2003-2004 and 2005-2006");
        //    INhanesCsvParser parser = new NhanesCsvParser();

        //    var records = parser.ParseCsv(file);

        //    Assert.IsNotNull(records);
        //    Assert.AreEqual(1000000, records.Count());
        //}

        private static TimeSpan TimeAction(Action action)
        {
            var sw = new Stopwatch();
            sw.Start();
            action.Invoke();
            sw.Stop();
            return sw.Elapsed;
        }
    }
}