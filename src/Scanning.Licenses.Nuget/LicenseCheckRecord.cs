using Domain.Models;

namespace Scanning.Licenses.Nuget;

public class LicenseCheckRecord
{
    public string? FullName { get; set; }
    public string? License { get; set; }
    public string? Path { get; set; }

    public bool IsValid => string.IsNullOrWhiteSpace(FullName) == false
        && string.IsNullOrWhiteSpace(Path) == false
        && string.IsNullOrWhiteSpace(License) == false;

    public License ToLicense()
    {
        if (IsValid)
        {
            var license = new License()
            {
                DependencyName = FullName!
                    .Split(" (")
                    .First(),
                Version = FullName
                    .Split(" (")
                    .Last()
                    .TrimEnd(')'),
                Type = License!,
                Parents = Path!
                    .Replace(@"\", "")
                    .Split("node_modules")
                    .Where(parent => !string.IsNullOrWhiteSpace(parent) && !parent.Contains("package.json"))
                    .ToList()
            };
            return license;
        }
        
        return new License();
    }
}