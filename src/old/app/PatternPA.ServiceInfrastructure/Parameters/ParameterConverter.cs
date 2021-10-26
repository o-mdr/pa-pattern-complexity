using System;
using PatternPA.ServiceInfrastructure.Interfaces;

namespace PatternPA.ServiceInfrastructure.Parameters
{
    public class ParameterConverter : IParameterConverter
    {
        #region IParameterConverter Members

        public Request ToRequest(ServiceInput input)
        {
            var request = new Request();
            request.State = RequestState.Pending;
            //request. input.SubjectFilePath

            return request;
        }

        public ServiceOutput ToOutput(Request request)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}