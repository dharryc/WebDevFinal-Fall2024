using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();

var app = builder.Build();
app.UseCors(c =>
    c.AllowAnyHeader()
     .AllowAnyMethod()
     .AllowAnyOrigin());

app.MapGet("/", () => "Hello World!");

app.Run();
