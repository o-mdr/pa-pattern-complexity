using System;
using System.Collections.Generic;
using System.Text;
using PatternPA.Container;
using PatternPA.Core.Interfaces.FileOperation;

namespace PatternPA.Infrastructure.Services
{
    public class AutocorrelationService
    {
        private readonly ICsvFileWriter _csvWriter;

        public AutocorrelationService()
        {
            _csvWriter = ContainerService.Instance.Resolve<ICsvFileWriter>();
        }

        public string SaveOne(Dictionary<int, double> data, string toFilePath = null)
        {
            var sb = new StringBuilder();

            foreach (var item in data)
            {
                sb.Append(item.Key + _csvWriter.Separator + item.Value);
                sb.Append(Environment.NewLine);
            }

            string header = "Shift index, Symbolic autocorrelation value " + 
                             Environment.NewLine;
            _csvWriter.WriteData(header, toFilePath, true);
            string output = _csvWriter.WriteData(sb.ToString(), toFilePath, true);

            return output;
        }

        public string SaveRandomlyShiftedData(double autocorrelation, 
            int precission, string toFilePath = null)
        {
            string header = "Autocorrelation for randomly shifted data averaged by " +
                             precission + " shifts" +
                             Environment.NewLine;
            _csvWriter.WriteData(header, toFilePath, true);
            string output = _csvWriter.WriteData(
                autocorrelation.ToString(), toFilePath, true);

            return output;
        }
    }
}
