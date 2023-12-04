namespace Domain.Models;

public class License
{
    public string DependencyName { get; set; }
    public string Version { get; set; }
    public string Type { get; set; }
    public List<string> Parents { get; set; }
}