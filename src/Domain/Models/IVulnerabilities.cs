using Domain.Results;

namespace Domain.Models;

public interface IVulnerabilities
{
    Result<List<Vulnerability>> Scan(string sourcePath);
}