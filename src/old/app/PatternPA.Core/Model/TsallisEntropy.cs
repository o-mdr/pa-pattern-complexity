using System;
using System.Collections.Generic;
using System.Linq;

namespace PatternPA.Core.Model
{
    public class TsallisEntropy
    {
        /// <summary>
        /// Computes Tsallis entropy 
        /// </summary>
        /// <param name="probabilities">Array of probabilities</param>
        /// <param name="q">A measure of the non-extensitivity of the system</param>
        /// <returns></returns>
        public decimal Calculate(IEnumerable<decimal> probabilities, decimal q)
        {
            //arguments check
            if (probabilities == null)
            {
                throw new ArgumentNullException("probabilities");
            }

            if (q == 1)
            {
                throw new ArgumentException(
                    "Tsallis entropy becomes Shannon entropy. ", "q");
            }

            decimal sumProbabilities = probabilities.Sum();
            if (sumProbabilities != 1)
            {
                throw new ArgumentException(
                    "Sum of the probabilities does not equal to 1. " +
                    "Computed value was: " + sumProbabilities,
                    "probabilities");
            }

            //sum power of probabilities
            decimal sum = probabilities
                .Sum(p =>
                    Convert.ToDecimal(
                        Math.Pow(
                            Convert.ToDouble(p),
                            Convert.ToDouble(q))));

            decimal tsallisEntropy = (1 - sum) / (q - 1);

            return tsallisEntropy;
        }
    }
}
