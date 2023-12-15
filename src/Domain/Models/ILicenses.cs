using Domain.Results;

namespace Domain.Models;

public interface ILicenses
{
    Result<List<License>> Scan(string sourcePath);
}