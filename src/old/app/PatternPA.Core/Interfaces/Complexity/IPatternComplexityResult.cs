using System.Collections.Generic;

namespace PatternPA.Core.Interfaces.Complexity
{
    public interface IPatternComplexityResult
    {
        double Value { get; set; }
        IEnumerable<double> Values { get; set; }
        ResultTypes Type { get; }
    }
}