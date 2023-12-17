using System.Diagnostics;
using System.Text.Json;
using Domain.Models;
using Domain.Results;

namespace Scanning.Licenses.Nuget;

public class LicenseScanning : ILicenses
{
    private readonly bool _isWindows;
    private readonly string filename = "nugetLicenses.json";

    public LicenseScanning(bool isWindows)
    {
        _isWindows = isWindows;
    }

    public Result<List<License>> Scan(string sourcePath)
    {
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
        process.StandardInput.WriteLine("dotnet restore");
        process.StandardInput.WriteLine($"dotnet-project-licenses -i ./ -j --outfile {filename}");
        process.StandardInput.WriteLine("exit");
        process.WaitForExit();
        
        var fileStream = File.OpenText($"{sourcePath}/{filename}");
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