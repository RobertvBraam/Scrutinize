namespace Scanning.Npm.Vulnerabilities;

internal record NpmDependency(string Name, string Url, string Severity, string Range);