using System;
using System.Configuration;
using System.Linq;
using NUnit.Framework;
using PatternPA.Core.Interfaces.Factories;
using PatternPA.Infrastructure.Factories;
using PatternPA.Infrastructure.Services;

namespace PatternPA.Test.Integration.Subjects
{
    [TestFixture]
    public class PersonCoarseGrainingTest : AbstractIntegrationTest
    {
        private readonly IPersonFactory personFactory = new PersonFactory();
        private readonly string subjectFile = ConfigurationManager.AppSettings["sampleFilePath"];
        private readonly PersonCalculationService personCalcService = new PersonCalculationService();

        [TestCase("00:00:01", 60)]
        [TestCase("00:00:01", 30)]
        [TestCase("00:00:00.100", 18000)]
        //[TestCase("00:00:00.100", 36000)]
        public void GetCoarseGrainedDataTest(string timeSpan, int wordLength)
        {
            var ts = TimeSpan.Parse(timeSpan);

            var person = personFactory.Create(subjectFile);
            person.CheckpointRate = ts;
            var result = personCalcService.CalculateCoarseGraining(person, wordLength);

            log.Debug("Input lengh: " + person.Snapshots.Data.Count());
            log.Debug("Encoded length: " + result.Length);
        }
    }
}