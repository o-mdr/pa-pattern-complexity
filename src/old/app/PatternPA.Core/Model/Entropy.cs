using System;
using System.Collections.Generic;
using System.Linq;
using PatternPA.Core.Interfaces;

namespace PatternPA.Core.Model
{
    public class Entropy : IEntropy
    {
        public static double LogBase = 2;

        #region IEntropy Members

        public EntropyResult CalculateShannonEntropy(IntervalSnapshots snapshots, IEnumerable<ActivityCodes> alphabet)
        {
            var result = new EntropyResult();
            int snapshotCount = snapshots.Data.Count();

            alphabet.ToList().ForEach(
                letter
                =>
                    {
                        int count = (from record in snapshots.Data
                                     where record.ActivityCode == letter
                                     select record).Count();

                        result.ActivityProbability.Add(letter, (double) count/snapshotCount);
                    });


            result.ActivityProbability.ToList().ForEach(
                pair
                => { result.EntropyValue -= pair.Value*Math.Log(pair.Value, LogBase); });
            return result;
        }

        #endregion
    }
}