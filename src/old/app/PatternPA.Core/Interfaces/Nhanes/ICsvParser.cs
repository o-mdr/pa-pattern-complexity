using System.Collections.Generic;
using PatternPA.Core.Model.Nhanes;

namespace PatternPA.Core.Interfaces.Nhanes
{
    public interface INhanesCsvParser
    {
        IEnumerable<NhanesRecord> ParseCsv(string filePath);
    }
}