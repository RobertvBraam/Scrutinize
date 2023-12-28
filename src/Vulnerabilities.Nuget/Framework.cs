namespace Vulnerabilities.Nuget;

internal class Framework
{
    public List<TopLevelPackage> TopLevelPackages { get; set; }
    public List<TransitivePackage> TransitivePackages { get; set; }
}