using System.Text.Json;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
XmlSerializer mySerializer = new(typeof(HashSet<string>));
StreamWriter myWriter = new StreamWriter("./storage/hashset/file.xml");

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

using var myFileStream = new FileStream("./storage/hashset/file.xml", FileMode.Open);
HashSet<string> ExistingUsers = (HashSet<string>)mySerializer.Deserialize(myFileStream);

app.MapPost("/newUser", async (User user) =>
{
  ExistingUsers.Add(user.UserName);
  mySerializer.Serialize(myWriter, ExistingUsers);
  myWriter.Close();
  var userPath = Path.Combine(storageRoot, Convert.ToString(user.Id));
  Directory.CreateDirectory(userPath);

  var userFile = Path.Combine(userPath, "user.json");
  await File.WriteAllTextAsync(userFile, JsonSerializer.Serialize(user));
});

app.MapGet("/userList", () =>
{
  string[] myUsers = new string[ExistingUsers.Count()];
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

app.MapDelete("/user/{userName}/delete", ([FromBody] User user) =>
{
  if (!ExistingUsers.Contains(user.UserName))
  {
    throw new Exception("User not found");
  }
  var blogFolder = user.UserName;
  var blogFile = Path.Combine(storageRoot, Convert.ToString(user.Id));
  // var blogPath = Path.Combine(storageRoot, id.ToString());
  // if (!Directory.Exists(blogPath))
  //   throw new Exception("Blog not found.");

  // Directory.Delete(blogPath, true);
});
// var blogPath = Path.Combine(storageRoot, blog.Id.ToString());
// Directory.CreateDirectory(blogPath);

// var blogFile = Path.Combine(blogPath, "blog.json");
// await File.WriteAllTextAsync(blogFile, JsonSerializer.Serialize(blog));

app.Run();

public record User(string UserName, string[]? Links, string[]? Descriptions, ulong Id);
