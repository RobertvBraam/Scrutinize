using Domain.Dependencies;
using Domain.Results;

namespace Domain.Persistence;

public class PersistDependenciesUseCase
{
    private readonly IPersistence _persistence;

    public PersistDependenciesUseCase(IPersistence persistence)
    {
        _persistence = persistence;
    }

    public Result Execute(List<Dependency> dependencies)
    {
        var result = _persistence.SaveDependencies(dependencies);
        return result;
    }
}