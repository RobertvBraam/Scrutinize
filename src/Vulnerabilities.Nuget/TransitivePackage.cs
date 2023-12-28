namespace Vulnerabilities.Nuget;

internal record TransitivePackage(string Id, List<VulnerabilityRecord> Vulnerabilities);