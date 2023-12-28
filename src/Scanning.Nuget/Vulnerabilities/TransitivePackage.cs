namespace Scanning.Nuget.Vulnerabilities;

internal record TransitivePackage(string Id, List<VulnerabilityRecord> Vulnerabilities);