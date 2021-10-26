using System;
using System.IO;
using PatternPA.Core.Model;

namespace PatternPA.Infrastructure.Services
{
    public class EntropyResultService
    {
        public void Save(EntropyResult result, string toFilePath)
        {
            using (var writer = new StreamWriter(toFilePath))
            {
                writer.WriteLine(String.Concat("EntropyValue", ": ", result.EntropyValue));

                foreach (var record in result.ActivityProbability)
                {
                    writer.WriteLine(String.Concat(record.Key, " - ", record.Value));
                }
            }
        }
    }
}