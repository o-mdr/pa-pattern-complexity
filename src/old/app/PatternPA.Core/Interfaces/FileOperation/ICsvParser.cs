using System.Collections.Generic;
using PatternPA.Core.Model;

namespace PatternPA.Core.Interfaces.FileOperation
{
    public interface ICsvParser
    {
        IEnumerable<EventRecord> ParseCsv(string filePath, int skipFirstRows);
    }
}