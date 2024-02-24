using Domain.Licenses;
using Domain.Results;
using Domain.Vulnerabilities;

namespace Domain.Monitoring;

public interface IMonitoring
{
    Result SendMetrics(List<License> licenses, List<Vulnerability> vulnerabilities);
}