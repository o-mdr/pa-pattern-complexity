using NUnit.Framework;
using PatternPA.Core.Interfaces.Factories;
using PatternPA.Core.Model.Nhanes;
using PatternPA.Infrastructure.Factories;

namespace PatternPA.Test.Core.Model
{
    [TestFixture]
    public class NhanesPersonActivityStatsGeneratorTest
    {
        private readonly IPersonFactory personFactory = new NhanesPersonFactory();
        private string file = "Data\\21031.Nhanes.csv";

        [Test]
        public void GenerateTest()
        {
            var person = personFactory.Create(file);

            var g = new NhanesPersonActivityStatsGenerator();
            var stats = g.Generate(person.NhanesRecords);
            Assert.IsNotNull(stats);
        }
    }
}