using System.Text.Json.Serialization;
using Domain.Models;

namespace Vulnerabilities.Nuget;

internal class RootAnalysis
{
    [JsonPropertyName("projects")]
    internal List<ProjectRecord> Projects { get; set; }

    internal List<Vulnerability> ToVulnarabilties()
    {
        var vulnerabilities = new List<Vulnerability>();

        foreach (var project in Projects)
        {
            foreach (var framework in project.Frameworks)
            {
                vulnerabilities.AddRange(framework.TopLevelPackages
                    .SelectMany(package => package.Vulnerabilities
                        .Select(vulnerability => new Vulnerability()
                        {
                            DependencyName = package.Id,
                            Severity = vulnerability.Severity,
                            Source = vulnerability.Advisoryurl
                        })
                    )
                );
                
                vulnerabilities.AddRange(framework.TransitivePackages
                    .SelectMany(package => package.Vulnerabilities
                        .Select(vulnerability => new Vulnerability()
                        {
                            DependencyName = package.Id,
                            Severity = vulnerability.Severity,
                            Source = vulnerability.Advisoryurl
                        })
                    )
                );
            }
        }

        return vulnerabilities;
    }
}