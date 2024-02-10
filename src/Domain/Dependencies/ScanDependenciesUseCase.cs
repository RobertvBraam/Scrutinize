using Domain.Licenses;
using Domain.Results;
using Domain.Vulnerabilities;

namespace Domain.Dependencies;

public class ScanDependenciesUseCase
{
    
    private readonly ILicenses _licenses;
    private readonly IVulnerabilities _vulnerabilities;

    public ScanDependenciesUseCase(ILicenses licenses, IVulnerabilities vulnerabilities)
    {
        _licenses = licenses;
        _vulnerabilities = vulnerabilities;
    }
    
    public Result<List<Dependency>> Execute(string sourcePath)
    {
        var licenses = _licenses.Scan(sourcePath);
        
        if (licenses.HasFailed)
        {
            return Result<List<Dependency>>.Failed(licenses.FailureReason);
        }
        
        var vulnerabilities = _vulnerabilities.Scan(sourcePath);

        if (vulnerabilities.HasFailed)
        {
            return Result<List<Dependency>>.Failed(licenses.FailureReason);
        }

        var dependencies = Dependency.MapDependencies(licenses.Value, vulnerabilities.Value);
        
        return Result<List<Dependency>>.Succeeded(dependencies);
    }
}