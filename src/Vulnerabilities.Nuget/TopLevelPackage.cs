namespace Vulnerabilities.Nuget;

internal class TopLevelPackage
{
    public string Id { get; set; }
    public List<VulnerabilityRecord> Vulnerabilities { get; set; }
}