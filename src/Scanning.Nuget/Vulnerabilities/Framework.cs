namespace Scanning.Nuget.Vulnerabilities;

internal record Framework(List<TopLevelPackage> TopLevelPackages, List<TransitivePackage> TransitivePackages);