using Domain.Results;

namespace Domain.Licenses;

public interface ILicenses
{
    Result<List<License>> Scan(string sourcePath);
}