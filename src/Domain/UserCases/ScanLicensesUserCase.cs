using Domain.Models;
using Domain.Results;

namespace Domain.UserCases;

public class ScanLicensesUserCase
{
    private readonly ILicenses _licenses;

    public ScanLicensesUserCase(ILicenses licenses)
    {
        _licenses = licenses;
    }
    
    public Result<List<License>> Execute(string sourcePath, bool shouldInitialize = true)
    {
        if (shouldInitialize || _licenses.Initialize().HasFailed)
        {
            return Result<List<License>>.Failed(InitializationFailed.Create());
        }
        
        var licenses = _licenses.Scan(sourcePath);
        return Result<List<License>>.Succeeded(licenses.Value);
    }
}