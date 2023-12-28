using Domain.Models;

namespace Scanning.Licenses.Nuget;

internal class LicenseCheckRecord
{
    internal string PackageName { get; set; }
    internal string PackageVersion { get; set; }
    internal string LicenseType { get; set; }

    internal License ToLicense()
    {
        return new License()
        {
            DependencyName = PackageName,
            Type = LicenseType,
            Version = PackageVersion
        };
    }
}