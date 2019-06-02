using PatternPA.ServiceInfrastructure.Parameters;

namespace PatternPA.ServiceInfrastructure.Interfaces
{
    public interface IParameterConverter
    {
        Request ToRequest(ServiceInput input);
        ServiceOutput ToOutput(Request request);
    }
}