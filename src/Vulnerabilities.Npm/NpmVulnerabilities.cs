using System.Text.Json;
using Domain.Models;

namespace Vulnerabilities.Npm;

internal class NpmVulnerabilities
{
    public int AuditReportVersion { get; set; }
    public Dictionary<string, NpmVulnerability> Vulnerabilities { get; set; }

    public IEnumerable<Vulnerability> ToVulnerabilities()
    {
        foreach ((string dependencyName, NpmVulnerability vulnerability) in Vulnerabilities)
        {
            foreach (var element in vulnerability.Via)
            {
                if (element.ValueKind == JsonValueKind.Object)
                {
                    var dependency = element.Deserialize<Dependency>(new JsonSerializerOptions(){PropertyNameCaseInsensitive = true});
                    yield return new Vulnerability()
                    {
                        DependencyName = dependencyName,
                        Severity = vulnerability.Severity,
                        Source = dependency.Url
                    };
                }
            }
        }
    }
}