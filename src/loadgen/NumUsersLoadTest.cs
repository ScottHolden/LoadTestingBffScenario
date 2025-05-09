using System.Diagnostics;

public class NumUsersLoadTest : ILoadTest
{
    private const int ramp = 10000; // 10 second ramp up, TODO: make this configurable
    public async Task RunAsync(string target, int count, Stats reporter, CancellationToken cancellationToken = default)
        => await Task.WhenAll(
            Enumerable.Range(1, count)
                .Select(i => RunWorkerAsync($"worker-{i}", target, ramp / count * i, reporter, cancellationToken))
        );

    private async Task RunWorkerAsync(string id, string target, int preWait, Stats reporter, CancellationToken cancellationToken)
    {
        using HttpClient hc = new();
        await Task.Delay(preWait, cancellationToken);
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                reporter.IncrementRequestCount();
                Stopwatch sw = Stopwatch.StartNew();
                var response = await hc.GetAsync(target, cancellationToken);
                sw.Stop();
                reporter.IncrementSuccessCount();
                reporter.AddRequestTime((int)sw.ElapsedMilliseconds);
            }
            catch (Exception)
            {
                if (!cancellationToken.IsCancellationRequested)
                    reporter.IncrementErrorCount();
            }
        }
    }
}