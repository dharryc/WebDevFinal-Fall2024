using System.Text.Json;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
XmlSerializer mySerializer = new
XmlSerializer(typeof(HashSet<string>));

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


// var myHashSetThing = File.ReadAllText("./storage/hashset.json");
//   var thisIsATest = JsonSerializer.Serialize(ExistingUsers);
//   File.WriteAllText("./storage/hashset.json", thisIsATest);

HashSet<string> ExistingUsers = [];

app.MapPost("/newUser", async (User user) =>
{
  ExistingUsers.Add(user.UserName);
  var userPath = Path.Combine(storageRoot, Convert.ToString(user.UserName));
  Directory.CreateDirectory(userPath);

  var userFile = Path.Combine(userPath, "user.json");
  await File.WriteAllTextAsync(userFile, JsonSerializer.Serialize(user));
});

app.MapGet("/userList", () =>
{
  string[]? myUsers = new string[ExistingUsers.Count()];
  ExistingUsers.CopyTo(myUsers);
  return JsonSerializer.Serialize(myUsers);
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

app.MapDelete("/user/{userName}/delete", (string userName) =>
{
  if (!ExistingUsers.Contains(userName))
  {
    throw new Exception("User not found");
  }
  var blogFolder = userName;
  var blogFile = Path.Combine(storageRoot, Convert.ToString(userName));
  File.Delete(blogFile);
});
// var blogPath = Path.Combine(storageRoot, blog.Id.ToString());
// Directory.CreateDirectory(blogPath);

// var blogFile = Path.Combine(blogPath, "blog.json");
// await File.WriteAllTextAsync(blogFile, JsonSerializer.Serialize(blog));

app.Run();

public record User(string UserName, string[]? Links, string[]? Descriptions, ulong Id);
