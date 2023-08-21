using System.Diagnostics.Metrics;

namespace Metrics;

public class TransactionMetrics
{
    //Auth Counters
    private Counter<int> AuthorizationApprovedCounter { get; }
    private Counter<int> AuthorizationDeclinedCounter { get; }

    //Clear Counters

    public const string MetricName = "TransactionService";

    public TransactionMetrics()
    {
        var meter = new Meter(MetricName);

        AuthorizationApprovedCounter = meter.CreateCounter<int>("authorization-approved", "Authorization");
        AuthorizationDeclinedCounter = meter.CreateCounter<int>("authorization-declined", "Authorization");
    }
    
    //Auth meters
    public void AddApprovedAuthorization(SpendType spendType) => AuthorizationApprovedCounter.Add(1, new []{new KeyValuePair<string, object?>("SpendType", spendType)});
    public void AddDeclinedAuthorization(SpendType spendType) => AuthorizationDeclinedCounter.Add(1, new []{new KeyValuePair<string, object?>("SpendType", spendType)});
}