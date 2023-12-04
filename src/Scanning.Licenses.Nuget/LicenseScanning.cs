using System.Diagnostics;
using System.Globalization;
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

        var psiNpmRunDist2 = new ProcessStartInfo
        {
            FileName = "powershell",
            WorkingDirectory = sourcePath,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        var pNpmRunDist2 = Process.Start(psiNpmRunDist2)!;
        // execute npm in a different directory
        pNpmRunDist2.StandardInput.WriteLine(@"npm i");
        pNpmRunDist2.StandardInput.WriteLine(@"npx licensecheck --tsv");
        pNpmRunDist2.StandardInput.WriteLine("exit");
        var output = pNpmRunDist2.StandardOutput.ReadToEnd();

        var readerConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = "\t",
            HasHeaderRecord = false,
            MissingFieldFound = null,
            BadDataFound = null,
            IgnoreBlankLines = true
        };
        var csvHelper = new CsvReader(new StringReader(output), readerConfiguration);

        csvHelper.Read();

        var records = csvHelper.GetRecords<LicenseCheckRecord>().ToList();
        var licenses = records
            .Where(record => record.IsValid)
            .Select(record => record.ToLicense())
            .ToList();

        return Result<List<License>>.Succeeded(licenses);
    }
}