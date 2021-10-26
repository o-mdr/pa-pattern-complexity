using System.Collections.Generic;
using PatternPA.Core.Model;

namespace PatternPA.Core.Interfaces.Convertor
{
    public interface IBinaryConverter
    {
        byte[] ToBinArray(IEnumerable<int> data);
        byte[] ToBinArray(IEnumerable<Record> data);
    }
}