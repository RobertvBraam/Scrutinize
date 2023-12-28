using System.Text.Json.Serialization;

namespace Vulnerabilities.Nuget;

internal class ProjectRecord
{
    [JsonPropertyName("frameworks")]
    internal List<Framework> Frameworks { get; set; }
}