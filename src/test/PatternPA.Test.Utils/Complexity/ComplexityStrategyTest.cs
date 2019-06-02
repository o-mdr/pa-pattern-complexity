using System.Linq;
using NUnit.Framework;
using PatternPA.Core.Interfaces.Complexity;
using PatternPA.Core.Model;
using PatternPA.Core.Model.Complexity;
using ExecutionContext = PatternPA.Core.Model.Complexity.ExecutionContext;

namespace PatternPA.Test.Utils.Complexity
{
    [TestFixture]
    public class ComplexityStrategyTest
    {
        [Test]
        public void GzipStrategyCreationTest()
        {
            var person = new Person(null);
            var concreteStrategy = new GZipStrategy(person);
            var context = new ExecutionContext(concreteStrategy);
            IPatternComplexityResult result =  context.ComputePatternComplexity();
            
            Assert.AreEqual(ResultTypes.SingleValue, result.Type);
            Assert.AreEqual(0, result.Value);
            Assert.AreEqual(0, result.Values.Count());
        }
    }
}
