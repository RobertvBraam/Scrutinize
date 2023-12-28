namespace Vulnerabilities.Nuget;

internal class TransitivePackage
{
    internal string Id { get; set; }
    internal List<VulnerabilityRecord> Vulnerabilities { get; set; }
}