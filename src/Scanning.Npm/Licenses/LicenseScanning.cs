using System.Diagnostics;
using System.Text.Json;
using Domain;
using Domain.Dependencies;
using Domain.Licenses;
using Domain.Results;

namespace Scanning.Npm.Licenses;

public class LicenseScanning : ILicenses
{
    private readonly bool _isWindows;
    private readonly ILogger<LicenseScanning> _logger;
    private const string Filename = "npmLicenses.json";

    public LicenseScanning(bool isWindows, ILogger<LicenseScanning> logger)
    {
        _isWindows = isWindows;
        _logger = logger;
    }

    public Result<List<Dependency>> Scan(string sourcePath)
    {
        if (!Directory.Exists(sourcePath))
        {
            return Result<List<Dependency>>.Failed(InitializationFailed.Create());
        }
        
        var filePath = $"{sourcePath}/{Filename}";
        var licenseCheckProcess = StartCmdProcess(sourcePath);
        _logger.Information("License check process started.");
        licenseCheckProcess.StandardInput.WriteLine("npm install");
        licenseCheckProcess.StandardInput.WriteLine($"npx license-checker --json --out {filePath} --direct");
        licenseCheckProcess.StandardInput.WriteLine("exit");
        licenseCheckProcess.WaitForExit();
        _logger.Information("License check process finished.");

        if (File.Exists(filePath))
        {
            var fileStream = File.OpenText(filePath);
            var records = JsonSerializer.Deserialize<Dictionary<string, LicenseCheckRecord>>(fileStream.ReadToEnd()) 
                ?? new Dictionary<string, LicenseCheckRecord>();
            _logger.Information($"{records.Count} License records deserialized.");
            
            var dependencies = records
                .SelectMany(record => record.Value.ToLicense(record.Key))
                .ToList();
            _logger.Information($"{dependencies.Count} Licenses found.");
            

            foreach (var dependency in dependencies)
            {
                foreach (var license in dependency.Licenses)
                {
                    _logger.Information($"- {dependency.Name} {license.Version} {license.Type}");
                }
            }
            
            return Result<List<Dependency>>.Succeeded(dependencies);
        }
        
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