using System.Collections.Concurrent;
using System.Diagnostics;

public class Stats
{
    private int _requestCount;
    private int _successCount;
    private int _errorCount;
    // Todo, optimize
    private readonly ConcurrentBag<int> _requestTimes = [];
    private readonly Stopwatch _rpsSw = new();
    public void StartRps() => _rpsSw.Restart();
    public void IncrementRequestCount() => Interlocked.Increment(ref _requestCount);
    public void IncrementSuccessCount() => Interlocked.Increment(ref _successCount);
    public void IncrementErrorCount() => Interlocked.Increment(ref _errorCount);
    public void AddRequestTime(int time) => _requestTimes.Add(time);
    public string Display()
        => $"Requests: {_requestCount}, "
            + $"Successes: {_successCount}, "
            + $"Errors: {_errorCount}, "
            + $"Avg: {(!_requestTimes.IsEmpty ? _requestTimes.Average() : 0):0.0}ms, "
            + $"Min: {(!_requestTimes.IsEmpty ? _requestTimes.Min() : 0):0.0}ms, "
            + $"Max: {(!_requestTimes.IsEmpty ? _requestTimes.Max() : 0):0.0}ms, "
            + $"RPS: {_successCount / _rpsSw.Elapsed.TotalSeconds:0.00}";
}