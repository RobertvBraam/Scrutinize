using System.Diagnostics;
using System.Text.Json;
using Domain.Dependencies;
using Domain.Licenses;
using Domain.Results;

namespace Scanning.Nuget.Licenses;

public class LicenseScanning : ILicenses
{
    private readonly bool _isWindows;
    private const string Filename = "nugetLicenses.json";

    public LicenseScanning(bool isWindows)
    {
        _isWindows = isWindows;
    }

    public Result<List<Dependency>> Scan(string sourcePath)
    {
        if (!Directory.Exists(sourcePath))
        {
            return Result<List<Dependency>>.Failed(InitializationFailed.Create());
        }
        var filePath = $"{sourcePath}/{Filename}";
        var process = StartCmdProcess(sourcePath);  
        
        process.StandardInput.WriteLine("dotnet restore");
        //TODO: Add a flag to include transitive dependencies (at the moment it's broken in the tool)
        process.StandardInput.WriteLine($"dotnet tool run dotnet-project-licenses -i {sourcePath} -j --outfile {filePath}");
        process.StandardInput.WriteLine("exit");
        process.WaitForExit();

        if (File.Exists(filePath))
        {
            var fileStream = File.OpenText(filePath);
            var records = JsonSerializer.Deserialize<List<LicenseCheckRecord>>(fileStream.ReadToEnd(), 
                new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });
            var dependencies = records?
                .Select(record => record.ToDependency())
                .ToList() ?? new List<Dependency>();

            return Result<List<Dependency>>.Succeeded(dependencies);
        }
        
        //TODO: Setup a good Error object
        return Result<List<Dependency>>.Failed(InitializationFailed.Create());
    }

    private Process StartCmdProcess(string processDirectory)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = _isWindows ? "cmd" : "/bin/bash",
            WorkingDirectory = processDirectory,
            RedirectStandardInput = true,
            RedirectStandardOutput = false,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        var process = Process.Start(processStartInfo)!;
        return process;
    }
}