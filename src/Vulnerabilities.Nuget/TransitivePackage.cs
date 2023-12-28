namespace Vulnerabilities.Nuget;

internal class TransitivePackage
{
    public string Id { get; set; }
    public List<VulnerabilityRecord> Vulnerabilities { get; set; }
}