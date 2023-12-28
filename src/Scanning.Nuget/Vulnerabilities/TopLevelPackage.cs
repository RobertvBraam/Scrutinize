namespace Scanning.Nuget.Vulnerabilities;

internal record TopLevelPackage(string Id, List<VulnerabilityRecord> Vulnerabilities);