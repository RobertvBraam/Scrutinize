using Domain.Models;

namespace Scanning.Licenses.Nuget;

internal class LicenseCheckRecord
{
    public LicenseCheckRecord(string packageName, string packageVersion, string licenseType)
    {
        PackageName = packageName;
        PackageVersion = packageVersion;
        LicenseType = licenseType;
    }

    public string PackageName { get; set; }
    public string PackageVersion { get; set; }
    public string LicenseType { get; set; }

    public License ToLicense() => new(PackageName, PackageVersion, LicenseType);
}