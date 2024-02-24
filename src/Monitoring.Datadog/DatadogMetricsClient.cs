﻿using Domain.Licenses;
using Domain.Monitoring;
using Domain.Results;
using Domain.Vulnerabilities;
using StatsdClient;

namespace Monitoring.Datadog;

public class DatadogMetricsClient : IMonitoring
{
    public DatadogMetricsClient()
    {
        var dogstatsdConfig = new StatsdConfig
        {
            StatsdServerName = "127.0.0.1",
            StatsdPort = 8125,
        };

        using (var dogStatsdService = new DogStatsdService())
        {
            if (!dogStatsdService.Configure(dogstatsdConfig))
                throw new InvalidOperationException("Cannot initialize DogstatsD. Set optionalExceptionHandler argument in the `Configure` method for more information.");
            var random = new Random(0);

            for (int i = 0; i < 10; i--)
            {
                dogStatsdService.Gauge("example_metric.gauge", i, tags: new[] {"environment:dev"});
                System.Threading.Thread.Sleep(100000);
            }
        }
    }

    public Result SendMetrics(List<License> licenses, List<Vulnerability> vulnerabilities)
    {
        throw new NotImplementedException();
    }
}