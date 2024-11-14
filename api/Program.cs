using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();

var app = builder.Build();
app.UseCors(c =>
    c.AllowAnyHeader()
     .AllowAnyMethod()
     .AllowAnyOrigin());

app.MapGet("/", () => "Hello World!");

var storageRoot = "./storage";
if (!Directory.Exists(storageRoot)) Directory.CreateDirectory(storageRoot);

app.MapPost("/newUser", async (User user) =>
{
  var userPath = Path.Combine(storageRoot, user.UserName);
  Directory.CreateDirectory(userPath);
 
  var userFile = Path.Combine(userPath, "blog.json");
  await File.WriteAllTextAsync(userFile, JsonSerializer.Serialize(user));
});

app.Run();

public record User(string UserName, string Password, string[] Links, string[] Descriptions);