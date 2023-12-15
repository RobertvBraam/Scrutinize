using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using CsvHelper;
using CsvHelper.Configuration;
using Domain.Models;
using Domain.Results;

namespace Scanning.Licenses.Nuget;

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
        pNpmRunDist1.StandardInput.WriteLine(@"dotnet tool install --global dotnet-project-licenses");
        pNpmRunDist1.StandardInput.WriteLine("exit");
        pNpmRunDist1.WaitForExit();

        IsInitialized = true;

        return Result.Succeeded();
    }

    public Result<List<License>> Scan(string sourcePath)
    {
        if (!IsInitialized)
        {
            return Result<List<License>>.Failed(InitializationFailed.Create());
        }

        var processStartInfo = new ProcessStartInfo
        {
            FileName = "powershell",
            WorkingDirectory = sourcePath,
            RedirectStandardInput = true,
            RedirectStandardOutput = false,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        var process = Process.Start(processStartInfo)!;
        // execute npm in a different directory
        process.StandardInput.WriteLine(@"dotnet restore");
        process.StandardInput.WriteLine(@"dotnet-project-licenses -i ./ -j --outfile nugetLicenses.json");
        process.StandardInput.WriteLine("exit");
        process.WaitForExit();
        
        var fileStream = File.OpenText(sourcePath + "/nugetLicenses.json");
        var records = JsonSerializer.Deserialize<List<LicenseCheckRecord>>(fileStream.ReadToEnd(), 
            new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });
        var licenses = records
            .Select(record => record.ToLicense())
            .ToList();

        return Result<List<License>>.Succeeded(licenses);
    }
    
    public class LicenseCheckRecord
    {
        public string PackageName { get; set; }
        public string PackageVersion { get; set; }
        public string LicenseType { get; set; }

        public License ToLicense()
        {
            return new License()
            {
                DependencyName = PackageName,
                Type = LicenseType,
                Version = PackageVersion
            };
        }
    }   
}