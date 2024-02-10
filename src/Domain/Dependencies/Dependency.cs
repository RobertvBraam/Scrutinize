using Domain.Licenses;
using Domain.Vulnerabilities;

namespace Domain.Dependencies;

public class Dependency
{
    
    public string Name { get; set; }
    public List<License> Licenses { get; set; }
    public List<Vulnerability> Vulnerabilities { get; set; }
    
    public static Dependency Create(string name)
    {
        return new Dependency()
        {
            Name = name,
            Licenses = new(),
            Vulnerabilities = new()
        };
    }
    
    public Dependency AddLicense(string version, string type)
    {
        Licenses.Add(new License(version, type));
        return this;
    }
    
    public Dependency AddVulnerability(string range, string severity, string source)
    {
        Vulnerabilities.Add(new Vulnerability(range, severity, source));
        return this;
    }

    public static List<Dependency> MapDependencies(List<Dependency> dependencies, List<Dependency> dependenciesToAdd)
    {
        var dependenciesMap = new List<Dependency>(dependencies);
        
        foreach (var dependency in dependenciesToAdd)
        {
            var existingDependency = dependenciesMap.FirstOrDefault(d => d.Name == dependency.Name);
            
            if (existingDependency == null)
            {
                dependenciesMap.Add(dependency);
                continue;
            }
            
            existingDependency.Licenses.AddRange(dependency.Licenses);
            existingDependency.Vulnerabilities.AddRange(dependency.Vulnerabilities);
        }
        
        return dependenciesMap;
    }

    private Dependency()
    {
        
    }
}