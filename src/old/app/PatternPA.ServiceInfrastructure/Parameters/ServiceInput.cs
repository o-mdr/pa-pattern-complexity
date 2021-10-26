using System.Runtime.Serialization;
using PatternPA.ServiceInfrastructure.Operations.Data;

namespace PatternPA.ServiceInfrastructure.Parameters
{
    [DataContract]
    public class ServiceInput : AbstractParameter
    {
        public string SubjectFilePath { get; set; }
        public DataSources DataSource { get; set; }
        public AnalysisTypes AnalysisType { get; set; }
        public int WindowLenght { get; set; }
    }
}