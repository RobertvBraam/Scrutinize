using Domain.Results;

namespace Domain.Models;

public interface ILicenses
{
    Result Initialize();
    Result<List<License>> Scan(string sourcePath);
}