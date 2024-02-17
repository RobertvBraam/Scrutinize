using System.Runtime.InteropServices;
using Domain;
using Domain.Licenses;
using Domain.Results;
using Npm = Scanning.Npm.Licenses;
using Nuget = Scanning.Nuget.Licenses;

namespace Host.Console;

public class LicenseFactory
{
    public static Result<ILicenses> Create(string path)
    {
        var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        
        if (File.Exists(path))
        {
            if (path.EndsWith(".csproj") || path.EndsWith(".sln"))
            {
                return Result<ILicenses>.Succeeded(new Nuget.LicenseScanning(isWindows));
            }
        
            if (path.EndsWith("package.json"))
            {
                return Result<ILicenses>.Succeeded(new Npm.LicenseScanning(isWindows, new Logger<Npm.LicenseScanning>()));
            }
        }
        
        if (Directory.Exists(path))
        {
            if (Directory.GetFiles(path).Any(file => file.EndsWith(".csproj") || file.EndsWith(".sln")))
            {
                return Result<ILicenses>.Succeeded(new Nuget.LicenseScanning(isWindows));
            }
            
            if (Directory.GetFiles(path).Any(file => file.EndsWith("package.json")))
            {
                return Result<ILicenses>.Succeeded(new Npm.LicenseScanning(isWindows, new Logger<Npm.LicenseScanning>()));
            }
        }

        return Result<ILicenses>.Failed(IncorrectPathFailed.Create(path));
    }
}