using System;
using System.Collections.Generic;
using PatternPA.Core.Interfaces.Complexity;

namespace PatternPA.Core.Model.Complexity
{
    public class PatternComplexityResult : IPatternComplexityResult
    {
        private double _value;
        private IEnumerable<double> _values;

        public double Value
        {
            get { return _value; }
            set
            {
                ValidateSingle();
                _value = value;
            }
        }

        public IEnumerable<double> Values
        {
            get { return _values; }
            set
            {
                ValidateMultiple();
                _values = value;
            }
        }

        public ResultTypes Type { get; private set; }

        public PatternComplexityResult(ResultTypes type)
        {
            Type = type;
            _value = 0;
            _values = new List<double>();
        }

        private void ValidateSingle()
        {
            if (Type == ResultTypes.MultipleValues)
            {
                throw new InvalidOperationException(
                    "Cannot set multiple values as the result is of single value type");
            }
        }

        private void ValidateMultiple()
        {
            if (Type == ResultTypes.SingleValue)
            {
                throw new InvalidOperationException(
                    "Cannot set single value as the result is of multiple vaslues type");
            }
        }
    }
}