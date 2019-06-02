using PatternPA.ServiceInfrastructure.Parameters;

namespace PatternPA.ServiceInfrastructure.Operations
{
    public abstract class AbstractHandler
    {
        protected AbstractHandler successor;

        public void SetSuccessor(AbstractHandler successor)
        {
            this.successor = successor;
        }

        public abstract void HandleRequest(Request request);
    }
}