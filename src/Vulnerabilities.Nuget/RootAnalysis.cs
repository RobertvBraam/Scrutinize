using Domain.Models;

namespace Vulnerabilities.Nuget;

internal class RootAnalysis
{
    public RootAnalysis(List<ProjectRecord> projects)
    {
        Projects = projects;
    }

    public List<ProjectRecord> Projects { get; set; }

    public List<Vulnerability> ToVulnerabilities()
    {
        var vulnerabilities = new List<Vulnerability>();

        foreach (var project in Projects)
        {
            foreach (var framework in project.Frameworks)
            {
                vulnerabilities.AddRange(framework.TopLevelPackages
                    .SelectMany(package => package.Vulnerabilities
                        .Select(vulnerability => new Vulnerability(package.Id, vulnerability.Severity, vulnerability.Advisoryurl))
                    )
                );
                
                vulnerabilities.AddRange(framework.TransitivePackages
                    .SelectMany(package => package.Vulnerabilities
                        .Select(vulnerability => new Vulnerability(package.Id, vulnerability.Severity, vulnerability.Advisoryurl))
                    )
                );
            }
        }

        return vulnerabilities;
    }
}