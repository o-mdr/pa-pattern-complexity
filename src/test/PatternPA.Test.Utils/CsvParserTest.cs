using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PatternPA.Core.Model;

namespace PatternPA.Test.Utils
{
    [TestFixture]
    public class CsvParserTest : AbstractUtilsTest
    {
        [Test]
        public void ParseCSVTest()
        {
            string filePath = @"Data\2003TrimmedEvents.csv";
            List<EventRecord> actual = csvParser.ParseCsv(filePath, 1).ToList();

            Assert.AreEqual(29073, actual.Count);
            Assert.AreEqual(1853646, actual[6432].DataCount);
        }
    }
}