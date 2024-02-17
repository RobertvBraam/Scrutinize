using Domain.Persistence;
using Domain.Results;
using Persistence.LocalFile;

namespace Host.Console;

internal class PersistenceFactory
{
    public static Result<IPersistence> Create(StorageTypes storageTypes)
    {
        return storageTypes switch
        {
            StorageTypes.None => Result<IPersistence>.NotFound(),
            StorageTypes.LocalFile => Result<IPersistence>.Succeeded(new FileClient("scan/dependencies.json")),
            _ => Result<IPersistence>.Failed(InitializationFailed.Create())
        };
    }
}