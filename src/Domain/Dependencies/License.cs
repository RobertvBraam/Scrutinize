namespace Domain.Licenses;

public class License
{
    internal License(string version, string type)
    {
        Version = version;
        Type = type;
    }
    public string Version { get; set; }
    public string Type { get; set; }
}