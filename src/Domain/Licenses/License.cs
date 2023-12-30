namespace Domain.Licenses;

public class License
{
    public License(string dependencyName, string version, string type)
    {
        DependencyName = dependencyName;
        Version = version;
        Type = type;
    }
    
    public string DependencyName { get; set; }
    public string Version { get; set; }
    public string Type { get; set; }
}