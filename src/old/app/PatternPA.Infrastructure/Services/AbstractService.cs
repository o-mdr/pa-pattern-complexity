using System;
using log4net;
using PatternPA.Container;
using PatternPA.Core.Interfaces;
using PatternPA.Core.Interfaces.Convertor;
using PatternPA.Core.Interfaces.FileOperation;

namespace PatternPA.Infrastructure.Services
{
    public abstract class AbstractService
    {
        protected IAlphabet alphabet;
        protected ICsvParser csvParser;
        protected ICsvFileWriter csvWriter;
        protected IFileWriter fileWriter;
        protected IArchiver gzip;
        protected IRecordConverter recordConverter;
        protected ILog log;

        protected AbstractService()
        {
            csvParser = ContainerService.Instance.Resolve<ICsvParser>();
            recordConverter = ContainerService.Instance.Resolve<IRecordConverter>();
            csvWriter = ContainerService.Instance.Resolve<ICsvFileWriter>();
            fileWriter = ContainerService.Instance.Resolve<IFileWriter>();
            gzip = ContainerService.Instance.Resolve<IArchiver>();
            alphabet = ContainerService.Instance.Resolve<IAlphabet>();
            log = LogManager.GetLogger("UnitTestLogger");
        }

        protected bool AreDatesEqual(DateTime first, DateTime second)
        {
            return first.Year == second.Year &&
                   first.Month == second.Month &&
                   first.Day == second.Day;
        }
    }
}