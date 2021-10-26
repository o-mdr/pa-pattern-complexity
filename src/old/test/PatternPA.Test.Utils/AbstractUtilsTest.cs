using PatternPA.Container;
using PatternPA.Core.Interfaces;
using PatternPA.Core.Interfaces.Convertor;
using PatternPA.Core.Interfaces.FileOperation;

namespace PatternPA.Test.Utils
{
    public abstract class AbstractUtilsTest
    {
        protected IAlphabet alphabet;
        protected IBinaryConverter binaryConverter;
        protected IBinaryFileWriter binaryFileWriter;
        protected ICheckpointFactory checkpointFactory;
        protected ICsvParser csvParser;
        protected ICsvFileWriter csvWriter;
        protected IFileWriter fileWriter;
        protected IArchiver gzip;
        protected IRecordConverter recordConverter;
        protected IRandomBitGenerator rndBitGenerator;
        protected IRandomEventGenerator rndEventGenerator;

        protected AbstractUtilsTest()
        {
            csvParser = ContainerService.Instance.Resolve<ICsvParser>();
            recordConverter = ContainerService.Instance.Resolve<IRecordConverter>();
            csvWriter = ContainerService.Instance.Resolve<ICsvFileWriter>();
            fileWriter = ContainerService.Instance.Resolve<IFileWriter>();
            gzip = ContainerService.Instance.Resolve<IArchiver>();
            alphabet = ContainerService.Instance.Resolve<IAlphabet>();
            checkpointFactory = ContainerService.Instance.Resolve<ICheckpointFactory>();
            binaryConverter = ContainerService.Instance.Resolve<IBinaryConverter>();
            rndEventGenerator = ContainerService.Instance.Resolve<IRandomEventGenerator>();
            rndBitGenerator = ContainerService.Instance.Resolve<IRandomBitGenerator>();
            binaryFileWriter = ContainerService.Instance.Resolve<IBinaryFileWriter>();
        }
    }
}