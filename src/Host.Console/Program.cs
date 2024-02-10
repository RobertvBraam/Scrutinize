using System.Text.Json;
using CommandDotNet;
using CommandDotNet.Diagnostics;
using Domain.Dependencies;
using Domain.Persistence;
using Domain.Results;
using Domain.Vulnerabilities;
using Serilog;

namespace Host.Console;

[Command(Description = "Scrutinize your cli tooling for vulnerabilities and licenses scanning")]
public class Program
{
    public static int Main(string[] args)
    {
        var loggerConfiguration = new LoggerConfiguration()
            .WriteTo.Console();
        
        #if DEBUG 
        loggerConfiguration
            .WriteTo.Debug()
            .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)
            .MinimumLevel.Debug();
        #else
        loggerConfiguration
            .MinimumLevel.Information();
        #endif
            
        Log.Logger = loggerConfiguration
            .CreateLogger();
        var appSettings = new AppSettings();
        var appRunner = new AppRunner<Program>(appSettings);
            
        appRunner.UseDefaultMiddleware();
        appRunner.UseTypoSuggestions();
        
        var exitcode = appRunner.Run(args);
            
        return exitcode;
    }
    
    public Task<int> Interceptor(InterceptorExecutionDelegate next, CommandContext ctx,
        [Option(
            Description = "Output the command with arguments and system info", 
            BooleanMode = BooleanMode.Implicit)] bool logcmd)
    {
        if (logcmd)
        {
            CommandLogger.Log(ctx);
        }
        return next();
    }
        
    [Subcommand]
    public Scanning Scanning { get; set; } = null!;
}

[Command("scan", Description = "Scan the project for licenses and vulnerabilities")]
public class Scanning
{
    [DefaultCommand] 
    public void DefaultCommand(
        [Option('p', "path", Description = "Path to the root of the project")] string path = "",
        //TODO: Check if it's possible to set a default value
        [Option('l', "local-file-storage", Description = "Save results in a local file")] string localFilePath = "",
        [Option('d', "diff-from-storage", Description = "Only send out new licenses based on storage")] bool diffFromStorage = false)
    {
        var scanResult = ScanForDependencies(path);
        var dependencies = scanResult.Value;
        var persistResult = PersistDependencies(dependencies, localFilePath);
        
        //TODO: Implement Diff Feature
        if (diffFromStorage)
        {
            dependencies = persistResult.Value;
        }
        
        SendDependenciesToMonitoring(dependencies);
    }

    private Result SendDependenciesToMonitoring(List<Dependency> dependencies)
    {
        
        
        return Result.Succeeded();
    }

    private Result<List<Dependency>> PersistDependencies(List<Dependency> dependencies, string localFilePath)
    {
        var persistence = PersistenceFactory.Create(localFilePath);
        var usercase = new PersistDependenciesUseCase(persistence.Value);
        return Result<List<Dependency>>.Succeeded(new List<Dependency>());
    }

    private static Result<List<Dependency>> ScanForDependencies(string path)
    {
        if (String.IsNullOrWhiteSpace(path))
        {
            path = Environment.CurrentDirectory;
        }
        
        var licensesResult = LicenseFactory.Create(path);
        var vulnerabilitiesResult = VulnerabilityFactory.Create(path);

        if (licensesResult.HasFailed)
        {
            System.Console.WriteLine(licensesResult.FailureReason.Message);
            return Result<List<Dependency>>.Failed(licensesResult.FailureReason);
        }

        if (vulnerabilitiesResult.HasFailed)
        {
            System.Console.WriteLine(vulnerabilitiesResult.FailureReason.Message);
            return Result<List<Dependency>>.Failed(vulnerabilitiesResult.FailureReason);
        }

        var useCase = new ScanDependenciesUseCase(licensesResult.Value, vulnerabilitiesResult.Value);
        var scannedDependencies = useCase.Execute(path);

        if (scannedDependencies.HasFailed)
        {
            System.Console.WriteLine(scannedDependencies.FailureReason.Message);
            return Result<List<Dependency>>.Failed(scannedDependencies.FailureReason);
        }
        
        var resultLicenses = JsonSerializer.Serialize(scannedDependencies.Value);
        System.Console.WriteLine(resultLicenses);
        return scannedDependencies;
    }

    private static Result<IVulnerabilities> ScanForVulnerabilities(string path)
    {
        if (String.IsNullOrWhiteSpace(path))
        {
            path = Environment.CurrentDirectory;
        }
        
        var vulnerabilities = VulnerabilityFactory.Create(path);

        if (vulnerabilities.HasFailed)
        {
            System.Console.WriteLine(vulnerabilities.FailureReason.Message);
            return vulnerabilities;
        }
        
        var scannedVulnerabilities = vulnerabilities.Value.Scan(path);

        if (scannedVulnerabilities.HasFailed)
        {
            System.Console.WriteLine(scannedVulnerabilities.FailureReason.Message);
            return vulnerabilities;
        }
        
        var resultVulnerabilities = JsonSerializer.Serialize(scannedVulnerabilities.Value);
        System.Console.WriteLine(resultVulnerabilities);
        return vulnerabilities;
    }
}