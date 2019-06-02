using System.Collections.Generic;
using PatternPA.ServiceInfrastructure.Interfaces;
using PatternPA.ServiceInfrastructure.Parameters;
using PatternPA.Container;
using PatternPA.Core.Interfaces;

namespace PatternPA.ServiceInfrastructure.Operations
{
    internal class DataReaderHandler : AbstractHandler
    {
        public static IDataReader defaultDataReader = ContainerService.Instance.Resolve<IDataReader>();

        public IDataReader reader;

        public DataReaderHandler() : this(defaultDataReader)
        {
        }

        public DataReaderHandler(IDataReader dataReader)
        {
            reader = dataReader;
        }

        public override void HandleRequest(Request request)
        {
            if (request.State == RequestState.Pending ||
                request.State == RequestState.IterationFinished)
            {
                request.State = RequestState.DataReadStarted;

                request.Records = GetIterationRecords(request);

                request.State = RequestState.DataReadFinished;
            }
            else if (successor != null)
            {
                successor.HandleRequest(request);
            }
        }

        private IEnumerable<IRecord> GetIterationRecords(Request request)
        {
            ulong startRecord = request.CurrentReaderWindow;
            ulong stopRecord = request.CurrentReaderWindow + request.WindowSize;

            request.CurrentReaderWindow += request.WindowSize;

            return reader.Read(startRecord, stopRecord);
        }
    }
}