﻿using Domain.Dependencies;
using Domain.Persistence;
using Domain.Results;

namespace Persistence.LocalFile;

public class FileClient : IPersistence
{
    public Result SaveDependencies(List<Dependency> dependencies)
    {
        throw new NotImplementedException();
    }

    public Result<List<Dependency>> GetDependencies()
    {
        throw new NotImplementedException();
    }
}