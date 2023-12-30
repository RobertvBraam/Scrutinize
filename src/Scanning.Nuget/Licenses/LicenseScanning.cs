using System.Diagnostics;
using System.Text.Json;
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

    public Result<List<License>> Scan(string sourcePath)
    {
        if (!Directory.Exists(sourcePath))
        {
            return Result<List<License>>.Failed(InitializationFailed.Create());
        }
        var filePath = $"{sourcePath}/{Filename}";
        var process = StartCmdProcess(sourcePath);  
        
        process.StandardInput.WriteLine("dotnet restore");
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
            var licenses = records?
                .Select(record => record.ToLicense())
                .ToList() ?? new List<License>();

            return Result<List<License>>.Succeeded(licenses);
        }
        
        //TODO: Setup a good Error object
        return Result<List<License>>.Failed(InitializationFailed.Create());
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