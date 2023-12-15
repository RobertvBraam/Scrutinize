﻿using System.Diagnostics;
using System.Text.Json;
using Domain.Models;
using Domain.Results;

namespace Scanning.Licenses.Npm;

public class LicenseScanning : ILicenses
{
    private readonly bool _isWindows;

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

        var processStartInfo = new ProcessStartInfo
        {
            FileName = _isWindows ? "cmd" : "/bin/bash",
            WorkingDirectory = sourcePath,
            RedirectStandardInput = true,
            RedirectStandardOutput = false,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        var process = Process.Start(processStartInfo)!;
        // execute npm in a different directory
        process.StandardInput.WriteLine(@"npm i");
        process.StandardInput.WriteLine(@"npx license-checker --json --out npmLicenses.json");
        process.StandardInput.WriteLine("exit");
        process.WaitForExit();
        
        var fileStream = File.OpenText(sourcePath + "/npmLicenses.json");
        var records = JsonSerializer.Deserialize<Dictionary<string, LicenseCheckRecord>>(fileStream.ReadToEnd());
        var licenses = records
            .SelectMany(record => record.Value.ToLicense(record.Key))
            .ToList();

        return Result<List<License>>.Succeeded(licenses);
    }
}