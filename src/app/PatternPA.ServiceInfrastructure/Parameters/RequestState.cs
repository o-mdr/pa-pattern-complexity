namespace PatternPA.ServiceInfrastructure.Parameters
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