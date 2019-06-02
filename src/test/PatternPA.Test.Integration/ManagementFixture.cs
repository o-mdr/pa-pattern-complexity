using NUnit.Framework;
using PatternPA.Core.Logging;

namespace PatternPA.Test.Integration
{
    [SetUpFixture]
    public class ManagementFixture
    {
        [SetUp]
        public void SetUp()
        {
            MyLogManager.Configure();
        }
    }
}