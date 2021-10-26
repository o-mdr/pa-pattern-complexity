using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace PatternPA.Test.Utils
{
    [TestFixture]
    public class RandomEventGeneratorTest : AbstractUtilsTest
    {
        [Test]
        public void GenerateRandomEventsTest()
        {
            IEnumerable<int> availableEvents = new[] {0, 1, 2};
            int lenght = 100;
            IEnumerable<int> actual = rndEventGenerator.GenerateRandomEvents(availableEvents, lenght);

            int e0 = actual.ToList().Count(entry => entry == 0);
            int e1 = actual.ToList().Count(entry => entry == 1);
            int e2 = actual.ToList().Count(entry => entry == 2);
            int total = actual.Count();

            double p0 = (double) e0/total;
            double p1 = (double) e1/total;
            double p2 = (double) e2/total;
        }

        //[TestCase(0,0)]
        [TestCase(1, 0)]
        //[TestCase(0.33, 0)]
        //[TestCase(0.66, 0)]
        public void GenerateRandomEventsTest(double degreeOfRandomness, int nonRandomEvent)
        {
            IEnumerable<int> availableEvents = new[] { 0, 1, 2 };
            int lenght = 100;
            IEnumerable<int> actual = rndEventGenerator
                .GenerateRandomEventsWithDegreeOfRandomess(availableEvents, lenght,
                degreeOfRandomness, nonRandomEvent);            
        }

        [Test]
        public void RandomLineOfTest()
        {
            int sequenceLenght = 1024;
            IEnumerable<int> availableEvents = new[] {0, 1, 2};
            IEnumerable<int> sequence = rndEventGenerator.GenerateRandomEvents(availableEvents, sequenceLenght);
            csvWriter.OutputFilePath = "random.csv";
            csvWriter.WriteData(sequence, null, true);
        }
    }
}