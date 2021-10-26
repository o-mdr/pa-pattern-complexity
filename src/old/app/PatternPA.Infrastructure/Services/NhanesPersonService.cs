using System;
using System.Configuration;
using System.IO;
using PatternPA.Core.Interfaces.Nhanes;
using PatternPA.Core.Model;
using PatternPA.Core.Model.Nhanes;
using System.Collections.Generic;
using PatternPA.Utils;
using System.Linq;
using PatternPA.Core.Interfaces;

namespace PatternPA.Infrastructure.Services
{
    public class NhanesPersonService : AbstractService
    {
        public static int startId = 0;
        public const string PersonPamSuffix = ".Nhanes.csv";

        public void Initialize(Person person)
        {
            var info = new FileInfo(person.DataFilePath);
            int idx = info.Name.IndexOf(ConfigurationManager.AppSettings["nhanesPeronDataFileSuffix"]);
            person.Id = info.Name.Remove(idx);

            INhanesCsvParser parser = new NhanesCsvParser();
            var records = parser.ParseCsv(person.DataFilePath);
            person.NhanesRecords = records;
        }

        public Person GenerateInNhanes2003Format(
            IEnumerable<int> availableEvents, int length,
            double degreeOfRandomness, int nonRandomEvent = 0)
        {
            var person = new Person(null);
            person.Id = startId;
            startId++;

            IRandomEventGenerator rndEventGenerator = new RandomEventGenerator();
            List<int> randomEvents = rndEventGenerator
               .GenerateRandomEventsWithDegreeOfRandomess(availableEvents, length,
               degreeOfRandomness, nonRandomEvent).ToList();   
            
            var records = new List<NhanesRecord>();
            for (int i = 0; i < length; i++)
			{			                                
                var record = new NhanesRecord();

                int subjectId = (int)person.Id;
                byte reliabilityFlag = 1; //reliable
                byte calibrationFlag = 1; //calibrated
                byte dayOfWeek = 0; //don't set this for generated data
                int sequenceId = 0; //don't set this for generated data
                byte hour = 0;      //don't set this for generated data
                byte minute = 0;    //don't set this for generated data                
                int deviceIntensity = 0; //don't set this for generated data                
                var intencityCode = (IntensityCodes)Enum.Parse(typeof(IntensityCodes), 
                    randomEvents[i].ToString());

                record.SetValues(subjectId: subjectId,
                                    reliabilityFlag: reliabilityFlag,
                                    calibrationFlag: calibrationFlag,
                                    dayOfWeek: dayOfWeek,
                                    sequenceId: sequenceId,
                                    hour: hour,
                                    minute: minute,
                                    deviceIntensity: deviceIntensity,
                                    stepCount: null,
                                    intensityCode: intencityCode);

                records.Add(record);
			}
            
            person.NhanesRecords = records;

            return person;
        }

        public string SavePersonInNhanesFormat(Person p, double degreeOfRandomness, string ouputFolder)
        {
            //set file name
            string fName = Path.Combine(ouputFolder, p.Id + PersonPamSuffix);

            if (!Directory.Exists(ouputFolder))
            {
                Directory.CreateDirectory(ouputFolder);
            }

            //save to new file or append to existing
            using (var writer = new StreamWriter(fName))
            {
                foreach (var record in p.NhanesRecords)
                {
                    writer.Write(record + Environment.NewLine);
                }
            }

            return fName;
        }
    }
}