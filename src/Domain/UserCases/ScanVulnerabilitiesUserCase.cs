using Domain.Models;
using Domain.Results;

namespace Domain.UserCases;

public class ScanVulnerabilitiesUserCase
{
    private readonly IVulnerabilities _vulnerabilities;

    public ScanVulnerabilitiesUserCase(IVulnerabilities vulnerabilities)
    {
        _vulnerabilities = vulnerabilities;
    }
    
    public Result<List<Vulnerability>> Execute(string sourcePath)
    {
        var vulnerabilities = _vulnerabilities.Scan(sourcePath);
        
        if (vulnerabilities.HasFailed)
        {
            return Result<List<Vulnerability>>.Failed(vulnerabilities.FailureReason);
        }
        
        return Result<List<Vulnerability>>.Succeeded(vulnerabilities.Value);
    }
}