namespace Scanning.Nuget.Vulnerabilities;

internal record TransitivePackage(string Id, string ResolvedVersion, List<VulnerabilityRecord> Vulnerabilities);