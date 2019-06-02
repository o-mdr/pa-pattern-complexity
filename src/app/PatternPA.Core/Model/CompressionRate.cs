using System.Collections.Generic;

namespace PatternPA.Core.Model
{
    public class CompressionRates
    {
        public CompressionRates()
        {
            Rates = new List<double>();
        }

        public List<double> Rates { get; set; }
        public Person Person { get; set; }
    }
}