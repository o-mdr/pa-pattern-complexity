using System.Collections.Generic;

namespace PatternPA.Core.Model
{
    public class EntropyResult
    {
        public EntropyResult()
        {
            ActivityProbability = new Dictionary<ActivityCodes, double>();
            EntropyValue = 0;
        }

        public Dictionary<ActivityCodes, double> ActivityProbability { get; set; }
        public double EntropyValue { get; set; }
    }
}