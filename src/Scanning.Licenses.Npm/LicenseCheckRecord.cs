using System.Text.Json.Serialization;
using Domain.Models;

namespace Scanning.Licenses.Npm;

internal class LicenseCheckRecord
{
    [JsonPropertyName("licenses")]
    public string? Licenses { get; set; }

    public IEnumerable<License> ToLicense(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName) == false
            && string.IsNullOrWhiteSpace(Licenses) == false)
        {
            foreach (var license in Licenses.Split("OR").Select(x => x.Trim('(', ')', ' ')))
            {
                var version = fullName.Split("@").Last();
                yield return new License(fullName.Replace("@" + version, ""), version, license);
            }
        }
    }
}