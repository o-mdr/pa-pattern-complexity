using System.Collections.Generic;
using PatternPA.Core.Interfaces;

namespace PatternPA.ServiceInfrastructure.Interfaces
{
    public interface IDataReader
    {
        /// <summary>
        /// Read data from the source
        /// </summary>
        /// <param name="startIdx">include</param>
        /// <param name="stopIdx">exclude</param>
        IEnumerable<IRecord> Read(ulong startIdx, ulong stopIdx);
    }
}