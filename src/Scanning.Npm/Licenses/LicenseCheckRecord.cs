using System.Text.Json.Serialization;
using Domain.Dependencies;

namespace Scanning.Npm.Licenses;

internal class LicenseCheckRecord
{
    [JsonPropertyName("licenses")]
    public string? Licenses { get; set; }

    public IEnumerable<Dependency> ToLicense(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName) == false
            && string.IsNullOrWhiteSpace(Licenses) == false)
        {
            foreach (var license in Licenses.Split("OR").Select(x => x.Trim('(', ')', ' ')))
            {
                var version = fullName.Split("@").Last();
                yield return Dependency.Create(fullName.Replace("@" + version, ""))
                    .AddLicense(version, license);
            }
        }
    }
}