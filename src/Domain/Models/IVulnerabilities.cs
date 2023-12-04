using Domain.Results;

namespace Domain.Models;

public interface IVulnerabilities
{
    Result Initialize();
    Result<List<Vulnerability>> Scan(string sourcePath);
}