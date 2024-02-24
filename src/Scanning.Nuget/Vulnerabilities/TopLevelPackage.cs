namespace Scanning.Nuget.Vulnerabilities;

internal record TopLevelPackage(string Id, string ResolvedVersion, List<VulnerabilityRecord> Vulnerabilities);