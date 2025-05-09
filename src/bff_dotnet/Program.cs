using Microsoft.AspNetCore.Mvc;

Uri backend = new("http://localhost:5137/api/invoke");

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();

var app = builder.Build();

app.MapGet("/invoke", async ([FromServices] HttpClient hc) 
    => await hc.GetStringAsync(backend)
);

app.Run();
