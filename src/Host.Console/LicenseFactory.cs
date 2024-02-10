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
        if (!Directory.Exists(path))
        {
            return Result<ILicenses>.Failed(IncorrectPathFailed.Create(path));
        }

        var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        
        if (Directory.GetFiles(path).Any(x => x.EndsWith(".csproj") || x.EndsWith(".sln")))
        {
            return Result<ILicenses>.Succeeded(new Nuget.LicenseScanning(isWindows));
        }
        
        if (path.EndsWith("package.json"))
        {
            return Result<ILicenses>.Succeeded(new Npm.LicenseScanning(isWindows, new Logger<Npm.LicenseScanning>()));
        }

        return Result<ILicenses>.Failed(IncorrectPathFailed.Create(path));
    }
}