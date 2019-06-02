using System;
using PatternPA.Core.Interfaces.Complexity;

namespace PatternPA.Core.Model.Complexity
{
    public abstract class AbstractStrategy : IPatternComplexity
    {
        public Person AppliedTo { get; private set; }

        protected AbstractStrategy(Person person)
        {
            AppliedTo = person;
        }

        public abstract IPatternComplexityResult ComputePatternComplexity();
    }
}