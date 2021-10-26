using System;
using System.Collections;
using PatternPA.Core.Interfaces;

namespace PatternPA.Utils
{
    public class RandomBitGenerator : IRandomBitGenerator
    {
        public BitArray Generate(int lenght)
        {
            if (lenght <= 0)
                throw new ArgumentOutOfRangeException("lenght");

            var bitArray = new BitArray(lenght);
            var random = new Random();

            for (int i = 0; i < lenght; i++)
            {
                bool tmpValue = Convert.ToBoolean(random.Next(0, 2));
                bitArray.Set(i, tmpValue);
            }

            return bitArray;
        }
    }
}