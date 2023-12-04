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
    
    public Result<List<Vulnerability>> Execute(string sourcePath, bool shouldInitialize = true)
    {
        if (shouldInitialize || _vulnerabilities.Initialize().HasFailed)
        {
            return Result<List<Vulnerability>>.Failed(InitializationFailed.Create());
        }
        
        var vulnerabilities = _vulnerabilities.Scan(sourcePath);
        return Result<List<Vulnerability>>.Succeeded(vulnerabilities.Value);
    }
}