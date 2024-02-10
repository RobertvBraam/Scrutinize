using Domain.Dependencies;
using Domain.Results;

namespace Domain.Vulnerabilities;

public interface IVulnerabilities
{
    Result<List<Dependency>> Scan(string sourcePath);
}