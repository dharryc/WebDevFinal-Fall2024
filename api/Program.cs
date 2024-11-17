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

HashSet<User> ExistingUsers = [];

app.MapPost("/newUser", async (User user) =>
{
  ExistingUsers.Add(user);
  var userPath = Path.Combine(storageRoot, Convert.ToString(user.Id));
  Directory.CreateDirectory(userPath);

  var userFile = Path.Combine(userPath, "user.json");
  await File.WriteAllTextAsync(userFile, JsonSerializer.Serialize(user));
});

app.MapGet("/user/{userName}", (string userName) =>
{
  var userFolder = userName;
  var userDirectory = Path.Combine(storageRoot, userFolder, "user.json");

  if (!File.Exists(userDirectory))
    throw new Exception("User not found.");

  var user = JsonSerializer.Deserialize<User>(File.ReadAllText(userDirectory));
  if (user == null)
    throw new Exception("Blog not found");

  return user;
});

app.MapDelete("/user/{userName}/delete", (User user) =>{
  if(!ExistingUsers.Contains(user))
  {
    throw new Exception("User not found");
  }
  var blogFolder = user.UserName;
  var blogFile = Path.Combine(storageRoot, blogFolder, "user.json");
});
// var blogPath = Path.Combine(storageRoot, blog.Id.ToString());
// Directory.CreateDirectory(blogPath);

// var blogFile = Path.Combine(blogPath, "blog.json");
// await File.WriteAllTextAsync(blogFile, JsonSerializer.Serialize(blog));

app.Run();

public record User(string UserName, string[]? Links, string[]? Descriptions, ulong Id);