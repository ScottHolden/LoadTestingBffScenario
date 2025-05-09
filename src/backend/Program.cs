var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/api/invoke", async () =>
{
    Console.Write(".");
    await Task.Delay(10000);
    Console.Write("!");
    return new
    {
        status = "ok"
    };
});

app.Run();
