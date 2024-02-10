using Domain.Dependencies;

namespace Scanning.Nuget.Vulnerabilities;

internal class RootAnalysis
{
    public RootAnalysis(List<ProjectRecord> projects)
    {
        Projects = projects;
    }

    public List<ProjectRecord> Projects { get; set; }

    public List<Dependency> ToDependencies()
    {
        var vulnerabilities = new List<Dependency>();

        foreach (var project in Projects)
        {
            foreach (var framework in project.Frameworks)
            {
                vulnerabilities.AddRange(framework.TopLevelPackages
                    .SelectMany(package => package.Vulnerabilities
                        .Select(vulnerability =>
                        {
                            var name = package.Id;
                            var version = package.ResolvedVersion;
                            return Dependency.Create(name)
                                .AddVulnerability(version, vulnerability.Severity, vulnerability.Advisoryurl);
                        })
                    )
                );
                
                vulnerabilities.AddRange(framework.TransitivePackages
                    .SelectMany(package => package.Vulnerabilities
                        .Select(vulnerability =>
                        {
                            var name = package.Id;
                            var version = package.ResolvedVersion;
                            return Dependency.Create(name)
                                .AddVulnerability(version, vulnerability.Severity, vulnerability.Advisoryurl);
                        })
                    )
                );
            }
        }

        return vulnerabilities;
    }
}