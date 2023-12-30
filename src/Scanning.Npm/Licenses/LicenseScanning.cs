using System.Diagnostics;
using System.Text.Json;
using Domain.Licenses;
using Domain.Results;

namespace Scanning.Npm.Licenses;

public class LicenseScanning : ILicenses
{
    private readonly bool _isWindows;
    private const string Filename = "npmLicenses.json";

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
        var licenseCheckProcess = StartCmdProcess(sourcePath);
        
        licenseCheckProcess.StandardInput.WriteLine("npm install");
        licenseCheckProcess.StandardInput.WriteLine($"npx license-checker --json --out {filePath}");
        licenseCheckProcess.StandardInput.WriteLine("exit");
        licenseCheckProcess.WaitForExit();

        if (File.Exists(filePath))
        {
            var fileStream = File.OpenText(filePath);
            var records = JsonSerializer.Deserialize<Dictionary<string, LicenseCheckRecord>>(fileStream.ReadToEnd());
            var licenses = records?
                .SelectMany(record => record.Value.ToLicense(record.Key))
                .ToList() ?? new List<License>();

            return Result<List<License>>.Succeeded(licenses);
        }
        
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