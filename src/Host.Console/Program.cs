using System.Text.Json;
using System.Text.Json.Serialization;
using CommandDotNet;
using Domain.Licenses;

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

[Command("licenses", Description = "Scan the project for licenses")]
public class Licenses
{
    [DefaultCommand] 
    public void DefaultCommand(
        [Option('p', "path", Description = "Path to the root of the project")] string path = "")
    {
        if (String.IsNullOrWhiteSpace(path))
        {
            path = Environment.CurrentDirectory;
        }
        var licenses = LicenseFactory.Create(path);

        if (licenses.HasFailed)
        {
            System.Console.WriteLine(licenses.FailureReason.Message);
            return;
        }
        
        var scannedLicenses = licenses.Value.Scan(path);

        if (scannedLicenses.HasFailed)
        {
            System.Console.WriteLine(scannedLicenses.FailureReason.Message);
            return;
        }
        
        var resultLicenses = JsonSerializer.Serialize(scannedLicenses.Value);
        System.Console.WriteLine(resultLicenses);
    }
}
    
[Command("vulnerabilities", Description = "Scan the project for vulnerabilities")]
public class Vulnerabilities
{
    [DefaultCommand] 
    public void DefaultCommand(
        [Option('p', "path", Description = "Path to the root of the project")] string path = "")
    {
        if (String.IsNullOrWhiteSpace(path))
        {
            path = Environment.CurrentDirectory;
        }
        var vulnerabilities = VulnerabilityFactory.Create(path);

        if (vulnerabilities.HasFailed)
        {
            System.Console.WriteLine(vulnerabilities.FailureReason.Message);
            return;
        }
        
        var scannedVulnerabilities = vulnerabilities.Value.Scan(path);

        if (scannedVulnerabilities.HasFailed)
        {
            System.Console.WriteLine(scannedVulnerabilities.FailureReason.Message);
            return;
        }
        
        var resultVulnerabilities = JsonSerializer.Serialize(scannedVulnerabilities.Value);
        System.Console.WriteLine(resultVulnerabilities);
    }
}