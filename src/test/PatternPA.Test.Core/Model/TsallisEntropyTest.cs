using System;
using NUnit.Framework;
using PatternPA.Core.Model;

namespace PatternPA.Test.Core.Model
{
    [TestFixture]
    public class TsallisEntropyTest
    {
        #region Happy paths

        [Test]
        public void CalculateTestHappyPath1()
        {
            decimal[] probabilities = new[] { 0.5m, 0.5m };
            decimal q = 0.5m;
            decimal expected = 0.828427124746192m;

            var calculator = new TsallisEntropy();
            decimal actual = calculator.Calculate(probabilities, q);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalculateTestHappyPath2()
        {
            decimal [] probabilities = new [] { 0.4m, 0.3m, 0.2m, 0.1m };
            decimal q = 0.5m;
            decimal expected = 1.887238902111276m;

            var calculator = new TsallisEntropy();
            decimal actual = calculator.Calculate(probabilities, q);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Sad paths

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CalculateTestSadPath1()
        {
            decimal[] probabilities = null; //error due to null
            decimal q = 0.5m;

            var calculator = new TsallisEntropy();
            calculator.Calculate(probabilities, q);

            Assert.Fail("Should not come here");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CalculateTestSadPath2()
        {
            decimal[] probabilities = new[] { 0.5m, 0.5m, 0.2m }; //error due to sum > 1
            decimal q = 0.5m;

            var calculator = new TsallisEntropy();
            calculator.Calculate(probabilities, q);

            Assert.Fail("Should not come here");
        }

        [Test]
        public void CalculateTestSadPath3()
        {
            decimal[] probabilities = new[] { 0.5m, 0.5m };
            decimal q = 0; //order of 0
            decimal expected = 1;

            var calculator = new TsallisEntropy();
            decimal actual = calculator.Calculate(probabilities, q);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalculateTestSadPath4()
        {
            decimal[] probabilities = new[] { 0.4m, 0.3m, 0.2m, 0.1m };
            decimal q = 0; //order of 0
            decimal expected = 3;

            var calculator = new TsallisEntropy();
            decimal actual = calculator.Calculate(probabilities, q);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CalculateTestSadPath5()
        {
            decimal[] probabilities = new[] { 0.5m, 0.5m };
            decimal q = 1; //order of 1, Shannon entropy
            
            var calculator = new TsallisEntropy();
            calculator.Calculate(probabilities, q);

            Assert.Fail("Should not come here");
        }

        #endregion
    }
}
