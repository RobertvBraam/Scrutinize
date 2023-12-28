namespace Vulnerabilities.Nuget;

internal class Framework
{
    internal List<TopLevelPackage> TopLevelPackages { get; set; }
    internal List<TransitivePackage> TransitivePackages { get; set; }
}