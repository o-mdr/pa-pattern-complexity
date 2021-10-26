using System;
using System.Configuration;
using NUnit.Framework;

namespace PatternPA.Test.Integration.Nhanes.LongRunningManualTests
{
    public class LongRunningSetUpFixture
    {
        [SetUpFixture]
        public class Config : AbstractGroupTest
        {
            [SetUp]
            public void SetUp()
            {
                try
                {
                    string from = ConfigurationManager.AppSettings["longRunningNhanesData"];
                    NhanesGroupTest.TestedGroup = nhanesGroupFactory.Create(from);
                }
                catch (Exception ex)
                {
                    throw;
                }
                
            }

            [TearDown]
            public void TearDown()
            {
            }
        }

    }
}