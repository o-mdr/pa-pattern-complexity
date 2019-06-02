using System.Configuration;
using System.IO;
using System.Linq;
using NUnit.Framework;
using PatternPA.Core.Extensions;
using PatternPA.Core.Interfaces.Factories;
using PatternPA.Core.Model;
using PatternPA.Infrastructure.Factories;
using PatternPA.Infrastructure.Services;
using System;
using System.Collections.Generic;


namespace PatternPA.Test.Integration.Nhanes.ArtificialGenerator
{
    [TestFixture]
    public class NhanesPersonTest : AbstractIntegrationTest
    {
        private readonly string subjectFile = ConfigurationManager.AppSettings["sampleNhanesFilePath"];
        private readonly IPersonFactory personFactory = new NhanesPersonFactory();
        private readonly NhanesService nhanesService = new NhanesService();
        private readonly NhanesPersonService nhanesPersonService = new NhanesPersonService();
        IEnumerable<int> availableEvents = new[] { 0, 1, 2, 3, 4, 5 };
        private static Random r = new Random();
        private static string outputFolder = "Artificial";

        [TestCase(0.2)]
        public void GenerateInNhanes2003FormatTest(double degreeOfRandomness)
        {
            int length = r.Next(7200, 14400);
            Person p = nhanesPersonService.GenerateInNhanes2003Format(
                availableEvents,
                length,
                degreeOfRandomness);

            Assert.IsNotNull(p);
            Assert.IsNotNull(p.NhanesRecords);
            Assert.AreEqual(length, p.NhanesRecords.Count());            
        }

        [TestCase(0.2)]
        public void SavePersonInNhanesFormatTest(double degreeOfRandomness)
        {
            int length = r.Next(7200, 14400);
            Person p = nhanesPersonService.GenerateInNhanes2003Format(
                availableEvents,
                length,
                degreeOfRandomness);

            outputFolder = outputFolder + "\\DegreeOfRandomness_" + degreeOfRandomness;

            string fName = nhanesPersonService.SavePersonInNhanesFormat(p,
                degreeOfRandomness, outputFolder);

            Assert.That(File.Exists(fName));
            Assert.That(File.ReadAllBytes(fName).Length > 0);
        }
    }
}
