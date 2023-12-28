namespace Vulnerabilities.Nuget;

internal record Framework(List<TopLevelPackage> TopLevelPackages, List<TransitivePackage> TransitivePackages);