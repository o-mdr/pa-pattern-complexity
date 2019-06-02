using System.Configuration;
using System.IO;
using NUnit.Framework;
using PatternPA.Infrastructure.Services;

namespace PatternPA.Test.Integration.Nhanes
{
    [TestFixture]
    public class ProcessPamFiles
    {
        private readonly string fromDataDir = ConfigurationManager.AppSettings["nhanesPamRawData"];
        private readonly string toDataDir = ConfigurationManager.AppSettings["nhanesPeoplePamDataOutput"];
        private readonly NhanesService nhanesService = new NhanesService();

        [Test]
        [Ignore]
        public void SavePeoplePamAsSeparateCsvFilesTest()
        {
            nhanesService.SavePeoplePamAsSeparateCsvFiles(fromDataDir, toDataDir);
            Assert.That(Directory.Exists(toDataDir));
        }
    }
}