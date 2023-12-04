namespace Domain.Results;

public class InitializationFailed : IDomainFailure
{
    private InitializationFailed()
    {
        
    }
    public string Message => "Initialization failed";
    public static IDomainFailure Create()
    {
        return new InitializationFailed();
    }
}