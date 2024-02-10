namespace Domain;

public class Logger<T> : ILogger<T>
{
    public void Information(string message)
    {
        var oldColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss zz}-{nameof(T)}: {message}");
        Console.ForegroundColor = oldColor;
    }
    
    public void Warning(string message)
    {
        var oldColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss zz}-{nameof(T)}: {message}");
        Console.ForegroundColor = oldColor;
    }
    
    public void Error(string message)
    {
        var oldColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss zz}-{nameof(T)}: {message}");
        Console.ForegroundColor = oldColor;
    }
}