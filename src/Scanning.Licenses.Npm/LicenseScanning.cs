using System.Diagnostics;
using System.Text.Json;
using Domain.Models;
using Domain.Results;

namespace Scanning.Licenses.Npm;

public class LicenseScanning : ILicenses
{
    private readonly bool _isWindows;
    private bool IsInitialized { get; set; }

    public LicenseScanning(bool isWindows)
    {
        _isWindows = isWindows;
    }

    public Result Initialize()
    {
        if (IsInitialized)
        {
            return Result.Succeeded();
        }

        var projectDirectory = Directory.GetParent(Environment.CurrentDirectory);
        var psiNpmRunDist1 = new ProcessStartInfo
        {
            FileName = _isWindows ? "cmd" : "/bin/bash",
            WorkingDirectory = projectDirectory?.FullName,
            RedirectStandardInput = true,
            RedirectStandardOutput = false,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        var pNpmRunDist1 = Process.Start(psiNpmRunDist1)!;
        pNpmRunDist1.StandardInput.WriteLine(@"npm -g install license-checker");
        pNpmRunDist1.StandardInput.WriteLine("exit");
        pNpmRunDist1.WaitForExit();

        IsInitialized = true;

        return Result.Succeeded();
    }

    public Result<List<License>> Scan(string sourcePath)
    {
        if (!IsInitialized || !Directory.Exists(sourcePath))
        {
            return Result<List<License>>.Failed(InitializationFailed.Create());
        }

        var psiNpmRunDist2 = new ProcessStartInfo
        {
            FileName = _isWindows ? "cmd" : "/bin/bash",
            WorkingDirectory = sourcePath,
            RedirectStandardInput = true,
            RedirectStandardOutput = false,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        var pNpmRunDist2 = Process.Start(psiNpmRunDist2)!;
        // execute npm in a different directory
        pNpmRunDist2.StandardInput.WriteLine(@"npm i");
        pNpmRunDist2.StandardInput.WriteLine(@"npx license-checker --json --out licenses.json");
        pNpmRunDist2.StandardInput.WriteLine("exit");
        pNpmRunDist2.WaitForExit();
        
        var fileStream = File.OpenText(sourcePath + "/licenses.json");
        var records = JsonSerializer.Deserialize<Dictionary<string, LicenseCheckRecord>>(fileStream.ReadToEnd());
        var licenses = records
            .SelectMany(record => record.Value.ToLicense(record.Key))
            .ToList();

        return Result<List<License>>.Succeeded(licenses);
    }
}