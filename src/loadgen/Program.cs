int parallelCount = 10;
string method = "users";

if (args.Length < 1)
{
    throw new Exception("Usage: loadgen <target> <parallelCount?> <method?>");
}

string target = args[0] switch {
    "direct" => "http://localhost:5137/api/invoke",
    "python" => "http://localhost:8000/invoke",
    "nodejs" => "http://localhost:3000/invoke",
    "dotnet" => "http://localhost:5119/invoke",
    _ => throw new Exception("Not valid target: " + args[0])
};
if (args.Length >= 2 && int.TryParse(args[1], out var pc))
{
    parallelCount = pc;
}
if (args.Length >= 3)
{
    method = args[2];
    if (method != "users" && method != "products")
    {
        throw new Exception("Not valid method: " + method);
    }
}

Console.WriteLine($"Target: {target}, Parallel Count: {parallelCount}, Method: {method}");

Stats stats = new();
ILoadTest loadTest = method switch
{
    "users" => new NumUsersLoadTest(),
    _ => throw new Exception("Not valid method: " + method)
};

_ = Task.Run(() =>
{
    while (true)
    {
        Console.WriteLine(stats.Display());
        Thread.Sleep(1000);
    }
});
stats.StartRps();
await loadTest.RunAsync(target, parallelCount, stats);