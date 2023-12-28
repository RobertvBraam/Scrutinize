using CommandDotNet;

namespace Host.Console;

[Command(Description = "Scrutinize your cli tooling for vulnerabilities and licenses scanning")]
class Program
{
    static int Main(string[] args)
    {
        var appSettings = new AppSettings();
        var appRunner = new AppRunner<Program>(appSettings);
        appRunner.UseDefaultMiddleware();
        var exitcode = appRunner.Run(args);
            
        return exitcode;
    }
        
    [Subcommand]
    public Licenses Licenses { get; set; } = null!;

    [Subcommand]
    public Vulnerabilities Vulnerabilities { get; set; } = null!;
}

[Command(Description = "Scan the project for licenses")]
public class Licenses
{
    [DefaultCommand] 
    public void DefaultCommand(
        [Option('p', "path", Description = "Path to the root of the project")] string path)
    {
        System.Console.WriteLine(path);
    }
}


    
[Command(Description = "Scan the project for vulnerabilities")]
public class Vulnerabilities
{
    [DefaultCommand] 
    public void DefaultCommand(
        [Option('p', "path", Description = "Path to the root of the project")] string path)
    {
        System.Console.WriteLine(path);
    }
}