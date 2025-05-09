public interface ILoadTest
{
    Task RunAsync(string target, int count, Stats reporter, CancellationToken cancellationToken = default);
}