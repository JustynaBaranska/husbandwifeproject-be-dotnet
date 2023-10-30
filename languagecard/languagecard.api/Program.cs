using languagecard.api;
using languagecard.api.Models;

var builder = WebApplication.CreateBuilder(args);

var cloudMode = (Environment.GetEnvironmentVariable("PORT") ?? "false") != "false";
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
var url = string.Concat("http://0.0.0.0:", port);

if (cloudMode)
{
    builder.WebHost.UseUrls(url);
}

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



var db = new DatabaseThingy();

app.MapGet("/Cards", async () =>
{
    
    var cards = new List<Card>();
   
    cards = await db.Select<Card>("cards");

    return cards;
})
.WithOpenApi();

app.MapPost("/Cards", async (Card card) =>
{   
   
    await db.Insert("cards", card);

    return;
})
.WithOpenApi();

app.MapGet("/info", () => $"OK - {DateTime.UtcNow:s}");

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
