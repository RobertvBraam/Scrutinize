using System.Text.Json.Serialization;

namespace Vulnerabilities.Nuget;

internal class ProjectRecord
{
    [JsonPropertyName("frameworks")]
    public List<Framework> Frameworks { get; set; }
}