using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using NUnit.Framework;
using PatternPA.Utils.Correlation.Auto;

namespace PatternPA.Test.Utils.Correlation.Auto
{
    [TestFixture]
    public class AutocorrelationTest
    {
        private static ILog log = LogManager.GetLogger("UnitTestLogger");
        
        [TestCase(0, 1)]
        [TestCase(1, 0.2)]
        [TestCase(2, 0.5)]
        [TestCase(3, 0.2)]
        [TestCase(4, 0.3)]
        [TestCase(5, 0.4)]
        [TestCase(6, 0.3)]
        [TestCase(7, 0.2)]
        [TestCase(8, 0.5)]
        [TestCase(9, 0.2)]
        [TestCase(10, 1)]
        public void ComputeTestByteData(int shiftIdx, double expResult)
        {
            log.Debug("Shift: " + shiftIdx);
            var data = new byte[] { 0, 0, 0, 1, 0, 1, 0, 2, 1, 2 };
            var autoCor = new Autocorrelation<byte>(data);
            double actual = autoCor.Compute(shiftIdx);
            Assert.AreEqual(expResult, actual);
        }

        [TestCase(0, 1)]
        [TestCase(1, 0.2)]
        [TestCase(2, 0.5)]
        [TestCase(3, 0.2)]
        [TestCase(4, 0.3)]
        [TestCase(5, 0.4)]
        [TestCase(6, 0.3)]
        [TestCase(7, 0.2)]
        [TestCase(8, 0.5)]
        [TestCase(9, 0.2)]
        [TestCase(10, 1)]
        public void ComputeTestStringData(int shiftIdx, double expResult)
        {
            log.Debug("Shift: " + shiftIdx);
            const string data = "0001010212";
            var autoCor = new Autocorrelation<char>(data.ToCharArray());
            double actual = autoCor.Compute(shiftIdx);
            Assert.AreEqual(expResult, actual);
        }

        [TestCase(10)]
        public void ComputeTestByteDataParallel(int parallelTasksToCreat)
        {
            //shall be in power of 10
            if (parallelTasksToCreat % 10 != 0)
            {
                throw new ArgumentException(
                    "must be a paw of 10 for test to calculate exp result");
            }

            int expArraySize = parallelTasksToCreat / 10;

            //hardcoded data
            var data = new byte[] { 0, 0, 0, 1, 0, 1, 0, 2, 1, 2 };

            //correspondent autocorrelation for a single round rotation
            var singleExpectedUnorderedResult = new[] {1, 0.2, 0.5, 0.2, 
                                                  0.3, 0.4, 0.3, 0.2, 
                                                  0.5, 0.2};

            //for each full circle add single expected array
            var expectedResult = new List<double>();
            for (int i = 0; i < expArraySize; i++)
            {
                expectedResult.AddRange(singleExpectedUnorderedResult);
            }

            int shiftBy = -1;
            var results = new ConcurrentBag<Task<double>>();

            //create parallel tasks to shift data
            for (int i = 0; i < parallelTasksToCreat; i++)
            {
                Task<double> result = Task<double>.Factory.StartNew(() =>
                {
                    var autoCor = new Autocorrelation<byte>(data);
                    Interlocked.Increment(ref shiftBy);
                    return autoCor.Compute(shiftBy);
                });

                results.Add(result);
            }

            //wait for all to compute
            Task.WaitAll(results.ToArray());

            //assert
            var actual = (from r in results
                          select r.Result).ToArray();

            Assert.That(expectedResult, Is.EquivalentTo(actual));
        }
    }
}
