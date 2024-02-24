using Domain.Dependencies;
using Domain.Results;

namespace Domain.Licenses;

public interface ILicenses
{
    Result<List<Dependency>> Scan(string sourcePath);
}