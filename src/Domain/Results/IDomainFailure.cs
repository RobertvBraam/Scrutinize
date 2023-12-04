namespace Domain.Results;

public interface IDomainFailure
{
    string Message { get; }
    static abstract IDomainFailure Create();
}