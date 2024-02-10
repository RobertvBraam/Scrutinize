namespace Domain.Results;

public class Result<T> : Result
{
    public static Result<T> Succeeded(T value)
    {
        var result = new Result<T>()
        {
            _value = value,
            HasSucceeded = true
        };
        return result;
    }
    
    public static Result<T> NotFound()
    {
        var result = new Result<T>()
        {
            HasSucceeded = true
        };
        return result;
    }

    public new static Result<T> Failed(IDomainFailure failure)
    {
        var result = new Result<T>()
        {
            _failureReason = failure,
            HasSucceeded = false
        };
        return result;
    }
    
    public T Value => HasSucceeded && _value is not null ? _value : throw new InvalidOperationException("Cannot access value of failed result");
    private T? _value;

    private Result()
    {
        
    }
}

public class Result
{
    public bool HasSucceeded { get; protected set; }
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
            _failureReason = failure,
            HasSucceeded = false
        };
        return result;
    }
    public IDomainFailure FailureReason => HasFailed ? _failureReason! : throw new InvalidOperationException("Cannot access failure reason of successful result");
    // ReSharper disable once InconsistentNaming
    protected IDomainFailure? _failureReason;

    protected Result()
    {
        
    }
}