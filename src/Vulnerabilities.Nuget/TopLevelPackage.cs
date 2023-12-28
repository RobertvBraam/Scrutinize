namespace Vulnerabilities.Nuget;

internal class TopLevelPackage
{
    internal string Id { get; set; }
    internal List<VulnerabilityRecord> Vulnerabilities { get; set; }
}