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
    
    public Result<List<License>> Execute(string sourcePath)
    {
        var licenses = _licenses.Scan(sourcePath);
        
        if (licenses.HasFailed)
        {
            return Result<List<License>>.Failed(licenses.FailureReason);
        }
        
        return Result<List<License>>.Succeeded(licenses.Value);
    }
}