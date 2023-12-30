using Domain.Results;

namespace Domain.Vulnerabilities;

public interface IVulnerabilities
{
    Result<List<Vulnerability>> Scan(string sourcePath);
}