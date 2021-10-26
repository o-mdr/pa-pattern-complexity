using System.Collections.Generic;
using PatternPA.Core.Model;

namespace PatternPA.Core.Interfaces
{
    public interface IAlphabet
    {
        IEnumerable<ActivityCodes> GetAlphabet();
    }
}