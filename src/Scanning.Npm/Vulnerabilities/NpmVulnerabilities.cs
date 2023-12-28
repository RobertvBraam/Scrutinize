using System.Text.Json;
using Domain.Models;

namespace Scanning.Npm.Vulnerabilities;

internal class NpmVulnerabilities
{
    public NpmVulnerabilities(Dictionary<string, NpmVulnerability> vulnerabilities)
    {
        Vulnerabilities = vulnerabilities;
    }
    
    public int AuditReportVersion { get; init; }
    public Dictionary<string, NpmVulnerability> Vulnerabilities { get; init; }

    public IEnumerable<Vulnerability> ToVulnerabilities()
    {
        foreach ((string dependencyName, NpmVulnerability vulnerability) in Vulnerabilities)
        {
            foreach (var element in vulnerability.Via)
            {
                if (element.ValueKind == JsonValueKind.Object)
                {
                    var dependency = element.Deserialize<Dependency>(new JsonSerializerOptions(){PropertyNameCaseInsensitive = true});
                    yield return new Vulnerability(dependencyName, vulnerability.Severity, dependency?.Url ?? String.Empty);
                }
            }
        }
    }
}