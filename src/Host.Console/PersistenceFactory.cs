using Domain.Persistence;
using Domain.Results;
using Persistence.LocalFile;

namespace Host.Console;

internal class PersistenceFactory
{
    public static Result<IPersistence> Create(string localFilePath)
    {
        if (!String.IsNullOrWhiteSpace(localFilePath) && Directory.Exists(localFilePath))
        {
            return Result<IPersistence>.Succeeded(new FileClient());
        }
        
        return Result<IPersistence>.Failed(InitializationFailed.Create());
    }
}