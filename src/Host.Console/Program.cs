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
        [Option('s', "storage", Description = "Save the results of the scan")] StorageTypes storageType = StorageTypes.None,
        [Option('d', "diff", Description = "Only return new licenses or vulnerabilities based on previous runs (stored)")] bool diffFromStorage = false)
    {
        var scanResult = ScanForDependencies(path);
        var dependencies = scanResult.Value;
        
        ConsoleWriteDependencies(dependencies);
        
        var persistResult = PersistDependencies(dependencies, storageType);
        
        if (persistResult.HasFailed)
        {
            System.Console.WriteLine(persistResult.FailureReason.Message);
            return;
        }
        
        var sendDependenciesResult = SendDependenciesToMonitoring(dependencies);
        
        if (sendDependenciesResult.HasFailed)
        {
            System.Console.WriteLine(sendDependenciesResult.FailureReason.Message);
            return;
        }

        System.Console.WriteLine("Scan completed.");
    }

    private Result SendDependenciesToMonitoring(List<Dependency> dependencies)
    {
        return Result.Succeeded();
    }

    private Result PersistDependencies(List<Dependency> dependencies, StorageTypes storageType)
    {
        if (storageType is StorageTypes.None)
        {
            return Result<List<Dependency>>.Succeeded(dependencies);
        }
        
        var persistence = PersistenceFactory.Create(storageType);
        
        if (persistence.HasFailed)
        {
            return Result<List<Dependency>>.Failed(persistence.FailureReason);
        }
        
        var useCase = new PersistDependenciesUseCase(persistence.Value);
        
        return useCase.Execute(dependencies);
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
            return Result<List<Dependency>>.Failed(licensesResult.FailureReason);
        }

        if (vulnerabilitiesResult.HasFailed)
        {
            return Result<List<Dependency>>.Failed(vulnerabilitiesResult.FailureReason);
        }

        var useCase = new ScanDependenciesUseCase(licensesResult.Value, vulnerabilitiesResult.Value);
        var scannedDependencies = useCase.Execute(path);

        if (scannedDependencies.HasFailed)
        {
            return Result<List<Dependency>>.Failed(scannedDependencies.FailureReason);
        }
        
        return scannedDependencies;
    }

    private static void ConsoleWriteDependencies(List<Dependency> dependencies)
    {
        var jsonResult = JsonSerializer.Serialize(dependencies, new JsonSerializerOptions()
        {
            WriteIndented = true
        });
        
        System.Console.WriteLine(jsonResult);
    }
}

public enum StorageTypes
{
    None,
    LocalFile,
    CloudStorage
}