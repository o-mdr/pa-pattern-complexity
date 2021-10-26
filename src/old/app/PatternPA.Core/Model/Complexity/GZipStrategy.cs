using System;
using System.Collections.Generic;
using PatternPA.Core.Interfaces;
using PatternPA.Core.Interfaces.Complexity;

namespace PatternPA.Core.Model.Complexity
{
    public class GZipStrategy : AbstractStrategy
    {
        private readonly IPatternComplexityResult _result;

        public GZipStrategy(Person person)
            : base(person)
        {
            _result = new PatternComplexityResult(ResultTypes.SingleValue);
        }

        public override IPatternComplexityResult ComputePatternComplexity()
        {
            //TODO implement this
            return _result;
        }
    }
}