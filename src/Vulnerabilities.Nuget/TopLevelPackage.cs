namespace Vulnerabilities.Nuget;

internal record TopLevelPackage(string Id, List<VulnerabilityRecord> Vulnerabilities);