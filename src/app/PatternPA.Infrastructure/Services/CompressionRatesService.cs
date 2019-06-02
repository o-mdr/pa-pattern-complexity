using System;
using PatternPA.Container;
using PatternPA.Core.Interfaces.FileOperation;
using PatternPA.Core.Model;

namespace PatternPA.Infrastructure.Services
{
    public class CompressionRatesService
    {
        private readonly ICsvFileWriter _csvWriter;

        public CompressionRatesService()
        {
            _csvWriter = ContainerService.Instance.Resolve<ICsvFileWriter>();
        }

        public string Save(CompressionRates compRate, string toFilePath = null)
        {
            string sData = String.Empty;
            compRate.Rates.ForEach(rate => sData = String.Concat(sData, rate, _csvWriter.Separator));
            sData = sData.TrimEnd(_csvWriter.Separator.ToCharArray());
            string record = String.Format("{0}{1}{2}", compRate.Person.Id, _csvWriter.Separator, sData);
            record = String.Concat(record, Environment.NewLine);

            string output = _csvWriter.WriteData(record, toFilePath, true, true);
            return output;
        }
    }
}