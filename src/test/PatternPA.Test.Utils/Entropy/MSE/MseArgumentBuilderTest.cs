using NUnit.Framework;
using PatternPA.Utils.Entropy.MSE;

namespace PatternPA.Test.Utils.Entropy.MSE
{
    [TestFixture]
    public class MseArgumentBuilderTest
    {
        private string _dataFile = @"Data\MSE_Sample.txt";
        
        
        [Test]
        public void BuildAllParamsTest()
        {
            const string expected = @"-m 2 -b 1 -M 20 -r 0.15 -c 0.05 -R 0.15 -i 0 -I 49 -n 20 -a 1 -F Data\MSE_Sample.txt";
            var builder = new MseArgumentBuilder();
            string actual = builder
                            .WithMinPatternLenght(2)
                            .HavingPatternLenghtStepOf(1)
                            .WithMaxPatternLenght(20)
                            .WithMinSimilarityCriterion(0.15)
                            .HavingSimilarityCriterionStepOf(0.05)
                            .WithMaxSimilarityCriterion(0.15)
                            .WithMinStartIndex(0)
                            .WithMaxStopIndex(49)
                            .WithMaxScaleFactor(20)
                            .HavingScaleWindowStepOf(1)
                            .ReadManyFilesFrom(_dataFile)
                            .Build();

            Assert.AreEqual(expected, actual);
        }
    }
}
