using PatternPA.ServiceInfrastructure.Parameters;

namespace PatternPA.ServiceInfrastructure.Operations
{
    internal class AnalysisHandler : AbstractHandler
    {
        public override void HandleRequest(Request request)
        {
            if (request.State == RequestState.DataReadFinished)
            {
                //analyse data
            }
            else if (successor != null)
            {
                successor.HandleRequest(request);
            }
        }
    }
}