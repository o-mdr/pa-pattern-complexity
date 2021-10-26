using System.Collections.Generic;
using System.IO;
using System.Linq;
using PatternPA.Core.Model;

namespace PatternPA.Infrastructure.Services
{
    public class IntervalSnapshotsService : AbstractService
    {
        public string Save(IntervalSnapshots snapshots,
                         string toFilePath,
                         bool asOneLineString = false,
                         bool saveJustEventCodes = false,
                         bool shoulAppend = false,
                         string separator = ", ")
        {
            IEnumerable<int> activityCodes = snapshots.Data.Select(
                    record =>
                    (int)record.ActivityCode);

            csvWriter.Separator = separator;

            if (asOneLineString)
            {
                csvWriter.WriteData(activityCodes, toFilePath, true);
            }
            else
            {
                if (saveJustEventCodes)
                {
                    csvWriter.WriteData(activityCodes, toFilePath);
                }
                else
                {
                    csvWriter.WriteData(snapshots.Data, toFilePath);
                }
            }

            return Path.GetFullPath(toFilePath);
        }
    }
}