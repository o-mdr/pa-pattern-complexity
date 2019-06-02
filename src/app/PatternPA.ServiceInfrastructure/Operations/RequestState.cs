namespace APMM.ServiceInfrastructure.Operations
{
    public enum RequestState
    {
        Pending,
        OperationStarted,
        DataReadStarted,
        DataReadFinished,
        AnalysisStarted,
        AnalysisFinished,
        IterationFinished,
        OperationCompleted
    }
}