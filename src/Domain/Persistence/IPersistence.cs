using Domain.Dependencies;
using Domain.Results;

namespace Domain.Persistence;

public interface IPersistence
{
    Result SaveDependencies(List<Dependency> dependencies);
    Result<List<Dependency>> GetDependencies();
}