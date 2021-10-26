using System.Collections.Generic;
using PatternPA.Core.Model;

namespace PatternPA.Core.Interfaces
{
    public interface IEntropy
    {
        EntropyResult CalculateShannonEntropy(IntervalSnapshots snapshots, IEnumerable<ActivityCodes> alphabet);
    }
}