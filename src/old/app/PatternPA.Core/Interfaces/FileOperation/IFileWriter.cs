using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace PatternPA.Core.Interfaces.FileOperation
{
    public interface IFileWriter
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        void WriteText<T>(IEnumerable<T> data);

        [MethodImpl(MethodImplOptions.Synchronized)]
        void WriteBinary(IEnumerable<int> data);
    }
}