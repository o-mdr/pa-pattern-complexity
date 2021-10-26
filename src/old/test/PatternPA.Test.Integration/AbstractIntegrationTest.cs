using System.Diagnostics;
using log4net;
using PatternPA.Container;
using PatternPA.Core.Interfaces;
using PatternPA.Core.Interfaces.Convertor;
using PatternPA.Core.Interfaces.FileOperation;
using PatternPA.Core.Interfaces.Nhanes;

namespace PatternPA.Test.Integration
{
    public abstract class AbstractIntegrationTest
    {
        protected IAlphabet alphabet;
        protected ICsvParser csvParser;
        protected INhanesCsvParser nhanesCsvParser;
        protected ICsvFileWriter csvWriter;
        protected IFileWriter fileWriter;
        protected IArchiver gzip;
        protected IRecordConverter recordConverter;
        protected ICheckpointFactory checkpointFactory;
        protected ILog log;

        protected AbstractIntegrationTest()
        {
            csvParser = ContainerService.Instance.Resolve<ICsvParser>();
            nhanesCsvParser = ContainerService.Instance.Resolve<INhanesCsvParser>();
            recordConverter = ContainerService.Instance.Resolve<IRecordConverter>();
            csvWriter = ContainerService.Instance.Resolve<ICsvFileWriter>();
            fileWriter = ContainerService.Instance.Resolve<IFileWriter>();
            gzip = ContainerService.Instance.Resolve<IArchiver>();
            alphabet = ContainerService.Instance.Resolve<IAlphabet>();
            checkpointFactory = ContainerService.Instance.Resolve<ICheckpointFactory>();
            log = LogManager.GetLogger("UnitTestLogger");
        }
    }
}
