using PatternPA.ServiceInfrastructure.Parameters;

namespace PatternPA.ServiceInfrastructure.Operations
{
    internal class ResultHandler : AbstractHandler
    {
        public override void HandleRequest(Request request)
        {
            if (request.State == RequestState.IterationFinished)
            {
                //append result
            }
            else if (successor != null)
            {
                successor.HandleRequest(request);
            }
        }
    }
}