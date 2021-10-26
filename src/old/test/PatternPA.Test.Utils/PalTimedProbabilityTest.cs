using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace PatternPA.Test.Utils
{
    [TestFixture]
    public class PalTimedProbabilityTest : AbstractUtilsTest
    {
        [Test]
        public void GetCheckPointsWithin_0()
        {
            var start = new TimeSpan(00, 52, 32);
            TimeSpan stop = start + TimeSpan.FromSeconds(5.9);
            int expectedCount = 0;
            var interval = new TimeSpan(0, 1, 0, 0);
            recordConverter.ActivityCheck = interval;

            List<TimeSpan> checkPoints = recordConverter.GetCheckPointsWithin(start, stop).ToList();
            Assert.AreEqual(expectedCount, checkPoints.Count);
        }

        /// <summary>
        ///   The get check points within_1.
        /// </summary>
        [Test]
        public void GetCheckPointsWithin_1()
        {
            var start = new TimeSpan(10, 59, 49);
            TimeSpan stop = start + TimeSpan.FromSeconds(14.8);
            int expectedCount = 1;

            var interval = new TimeSpan(0, 1, 0, 0);
            recordConverter.ActivityCheck = interval;
            List<TimeSpan> checkPoints = recordConverter.GetCheckPointsWithin(start, stop).ToList();
            Assert.AreEqual(expectedCount, checkPoints.Count);
        }

        /// <summary>
        ///   The get check points within_1_1.
        /// </summary>
        [Test]
        public void GetCheckPointsWithin_1_1()
        {
            var start = new TimeSpan(23, 59, 56);
            TimeSpan stop = start + TimeSpan.FromSeconds(5.1);
            int expectedCount = 1;

            var interval = new TimeSpan(0, 1, 0, 0);
            recordConverter.ActivityCheck = interval;
            List<TimeSpan> checkPoints = recordConverter.GetCheckPointsWithin(start, stop).ToList();
            Assert.AreEqual(expectedCount, checkPoints.Count);
        }

        /// <summary>
        ///   The get check points within_2.
        /// </summary>
        [Test]
        public void GetCheckPointsWithin_2()
        {
            var start = new TimeSpan(22, 40, 53);
            TimeSpan stop = start + TimeSpan.FromSeconds(7898.9);
            int expectedCount = 2;

            var interval = new TimeSpan(0, 1, 0, 0);
            recordConverter.ActivityCheck = interval;
            List<TimeSpan> checkPoints = recordConverter.GetCheckPointsWithin(start, stop).ToList();
            Assert.AreEqual(expectedCount, checkPoints.Count);
        }

        /// <summary>
        ///   The get check points within_3.
        /// </summary>
        [Test]
        public void GetCheckPointsWithin_3()
        {
            var start = new TimeSpan(20, 22, 27);
            TimeSpan stop = start + TimeSpan.FromSeconds(10501.4);
            int expectedCount = 3;

            var interval = new TimeSpan(0, 1, 0, 0);
            recordConverter.ActivityCheck = interval;
            List<TimeSpan> checkPoints = recordConverter.GetCheckPointsWithin(start, stop).ToList();
            Assert.AreEqual(expectedCount, checkPoints.Count);
        }

        /// <summary>
        ///   The get check points within_9.
        /// </summary>
        [Test]
        public void GetCheckPointsWithin_9()
        {
            var start = new TimeSpan(00, 53, 41);
            TimeSpan stop = start + TimeSpan.FromSeconds(29356.6);
            int expectedCount = 9;

            var interval = new TimeSpan(0, 1, 0, 0);
            recordConverter.ActivityCheck = interval;
            List<TimeSpan> checkPoints = recordConverter.GetCheckPointsWithin(start, stop).ToList();
            Assert.AreEqual(expectedCount, checkPoints.Count);
        }
    }
}