using System.Collections.Generic;
using PatternPA.Core.Model;

namespace PatternPA.Test.Integration.Subjects
{
    public abstract class SubjectTest : AbstractIntegrationTest
    {
        private IEnumerable<EventRecord> events;

        public IEnumerable<EventRecord> GetEvents(string dataFilePath)
        {
            return events ?? (events = csvParser.ParseCsv(dataFilePath, 1));
        }
    }
}
