using System.Collections;

namespace PatternPA.Core.Interfaces
{
    public interface IRandomBitGenerator
    {
        BitArray Generate(int lenght);
    }
}