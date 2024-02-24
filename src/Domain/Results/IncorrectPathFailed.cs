namespace Domain.Results;

public class IncorrectPathFailed : IDomainFailure
{
    private IncorrectPathFailed()
    {
        
    }
    public string IncorrectPath { get; private set; } = null!;
    public string Message => "Path provided does not exist or does not contain a supported project (package.json, *.sln or *.csproj)";
    public static IDomainFailure Create(string path)
    {
        return new IncorrectPathFailed()
        {
            IncorrectPath = path
        };
    }
}