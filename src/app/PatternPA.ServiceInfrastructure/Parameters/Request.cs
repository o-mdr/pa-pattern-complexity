using System.Collections.Generic;
using PatternPA.Core.Interfaces;

namespace PatternPA.ServiceInfrastructure.Parameters
{
    public class Request : ServiceInput
    {
        public static uint defaultWindowSize = 1024;

        public RequestState State { get; set; }
        public uint WindowSize { get; private set; }
        public uint CurrentReaderWindow { get; set; }
        public IEnumerable<IRecord> Records { get; set; }
        
        public Request()
            : this(defaultWindowSize)
        {
        }

        public Request(uint windowSize)
        {
            WindowSize = windowSize;
            //  CurrentReaderShift = 0;
            CurrentReaderWindow = 0;
        }

        
    }
}