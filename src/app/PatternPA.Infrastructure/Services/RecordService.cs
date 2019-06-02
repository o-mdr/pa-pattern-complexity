
using APMM.Core.Model;

namespace APMM.Infrastructure.Services
{
    public class RecordService : AbstractService
    {
        public void SaveCountingRecords(TimeCountingRecords records, string toFilePath)
        {
            csvWriter.WriteData(records.Records, toFilePath);
        }
    }
}
