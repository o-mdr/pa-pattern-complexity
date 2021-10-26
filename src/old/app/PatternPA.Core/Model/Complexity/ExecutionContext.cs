using PatternPA.Core.Interfaces;
using PatternPA.Core.Interfaces.Complexity;

namespace PatternPA.Core.Model.Complexity
{
    public class ExecutionContext : IPatternComplexity
    {
        private readonly IPatternComplexity _strategy;

        public ExecutionContext(IPatternComplexity strategy)
        {
            _strategy = strategy;
        }

        public IPatternComplexityResult ComputePatternComplexity()
        {
            return _strategy.ComputePatternComplexity();
        }
    }
}
