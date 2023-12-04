namespace Domain.Results;

public class Result<T>
{
    public bool HasSucceeded { get; private set; }
    public bool HasFailed => !HasSucceeded;
    public static Result<T> Succeeded(T value)
    {
        var result = new Result<T>()
        {
            Value = value,
            HasSucceeded = true
        };
        return result;
    }

    public static Result<T> Failed(IDomainFailure failure)
    {
        var result = new Result<T>()
        {
            FailureReason = failure,
            HasSucceeded = false
        };
        return result;
    }

    public T Value { get; init; }
    public IDomainFailure FailureReason { get; init; }
}

public class Result
{
    public bool HasSucceeded { get; private set; }
    public bool HasFailed => !HasSucceeded;
    public static Result Succeeded()
    {
        var result = new Result()
        {
            HasSucceeded = true
        };
        return result;
    }

    public static Result Failed(IDomainFailure failure)
    {
        var result = new Result()
        {
            FailureReason = failure,
            HasSucceeded = false
        };
        return result;
    }
    public IDomainFailure FailureReason { get; init; }
}