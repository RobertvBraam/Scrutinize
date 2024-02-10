using System.Text.Json;
using Domain.Dependencies;

namespace Scanning.Npm.Vulnerabilities;

internal class NpmVulnerabilities
{
    public NpmVulnerabilities(Dictionary<string, NpmVulnerability> vulnerabilities)
    {
        Vulnerabilities = vulnerabilities;
    }
    
    public int AuditReportVersion { get; init; }
    public Dictionary<string, NpmVulnerability> Vulnerabilities { get; init; }

    public IEnumerable<Dependency> ToDependencies()
    {
        foreach ((_, NpmVulnerability npmVulnerability) in Vulnerabilities)
        {
            foreach (var element in npmVulnerability.Via)
            {
                if (element.ValueKind == JsonValueKind.Object)
                {
                    var dependencyobject = element.Deserialize<NpmDependency>(new JsonSerializerOptions(){PropertyNameCaseInsensitive = true}) 
                        ?? throw new ArgumentNullException(nameof(element), "A dependency could not be deserialized.");

                    if (npmVulnerability.Effects.Any())
                    {
                        foreach (var effect in npmVulnerability.Effects)
                        {
                            yield return Dependency.Create(effect)
                                .AddVulnerability(npmVulnerability.Range, npmVulnerability.Severity, dependencyobject.Url);
                        }
                    }
                    else
                    {
                        yield return Dependency.Create(dependencyobject.Name)
                            .AddVulnerability(npmVulnerability.Range, npmVulnerability.Severity, dependencyobject.Url);
                    }
                }
            }
        }
    }
}