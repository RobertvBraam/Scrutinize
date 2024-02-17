using Domain.Dependencies;
using Domain.Persistence;
using Domain.Results;

namespace Persistence.LocalFile;

public class FileClient : IPersistence
{
    private readonly string _localFilePath;

    public FileClient(string localFilePath)
    {
        _localFilePath = localFilePath;
    }

    public Result SaveDependencies(List<Dependency> dependencies)
    {
        throw new NotImplementedException();
    }

    public Result<List<Dependency>> GetDependencies()
    {
        throw new NotImplementedException();
    }
}