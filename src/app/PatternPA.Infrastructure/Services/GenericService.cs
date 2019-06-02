using System.Collections.Generic;

namespace PatternPA.Infrastructure.Services
{
    public class RecordService<T> : AbstractService
    {
        public void Save(IEnumerable<T> data, string toFilePath)
        {
            csvWriter.WriteData(data, toFilePath);
        }
    }
}
