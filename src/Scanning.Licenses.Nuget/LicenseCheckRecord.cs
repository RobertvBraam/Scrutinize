using Domain.Models;

namespace Scanning.Licenses.Nuget;

internal class LicenseCheckRecord
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