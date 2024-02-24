namespace Domain;

public interface ILogger<T>
{
    void Information(string message);
    void Warning(string message);
    void Error(string message);
}