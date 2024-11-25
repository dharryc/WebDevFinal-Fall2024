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
var hashSetRoot = "./hash";

if (!Directory.Exists(storageRoot)) Directory.CreateDirectory(storageRoot);

HashSet<string> ExistingUsers = [];
if (!Directory.Exists(hashSetRoot))
{
  Directory.CreateDirectory(hashSetRoot);
}
else
{
  var myHashSetThing = File.ReadAllText(hashSetRoot + "/hashObj.json");
  ExistingUsers = JsonSerializer.Deserialize<HashSet<string>>(myHashSetThing);
}

app.MapPost("/newUser", async (User user) =>
{
  if (!ExistingUsers.Contains(user.UserName))
  {
    ExistingUsers.Add(user.UserName);
    File.WriteAllText(hashSetRoot + "/hashObj.json", JsonSerializer.Serialize(ExistingUsers));
    var userPath = Path.Combine(storageRoot, Convert.ToString(user.UserName));
    Directory.CreateDirectory(userPath);

    var userFile = Path.Combine(userPath, "user.json");
    await File.WriteAllTextAsync(userFile, JsonSerializer.Serialize(user));
  }
});

app.MapGet("/userList", () =>
{
  string[]? myUsers = new string[ExistingUsers.Count()];
  ExistingUsers.CopyTo(myUsers);
  return myUsers;
});

app.MapGet("/user/{userName}", (string userName) =>
{
  var userFolder = userName;
  var userDirectory = Path.Combine(storageRoot, userFolder, "user.json");

  if (!File.Exists(userDirectory))
    throw new Exception("User not found.");

  var user = JsonSerializer.Deserialize<User>(File.ReadAllText(userDirectory));
  if (user == null)
    throw new Exception("user not found");

  return user;
});

app.MapGet("/user/{userName}/{link}/{description}", (string userName, string link, string description) =>
{
  var userFolder = userName;
  var userDirectory = Path.Combine(storageRoot, userFolder, "user.json");

  if (!File.Exists(userDirectory))
    throw new Exception("User not found.");

  var user = JsonSerializer.Deserialize<User>(File.ReadAllText(userDirectory));
  if (user == null)
    throw new Exception("user not found");

  user.Links.Add(link);
  user.Descriptions.Add(description);
  
});

app.MapDelete("/user/{userName}/delete", (string userName) =>
{
  if (!ExistingUsers.Contains(userName))
  {
    throw new Exception("User not found");
  }
  var userFolder = userName;
  var userFile = Path.Combine(storageRoot, userName);
  ExistingUsers.Remove(userName);
  File.WriteAllText(hashSetRoot + "/hashObj.json", JsonSerializer.Serialize(ExistingUsers));
  Directory.Delete(userFile, true);
});

// app.MapGet("/users/{userName}/items", (string userName) =>
// {
//   var userPath = Path.Combine(storageRoot, userName.ToLower());
//   if (!Directory.Exists(commentsPath))
//     return [];
//   return [];
//   // var comments = Directory.GetFiles(commentsPath)
//       // .Select((file) => File.ReadAllText(file))
//       // .Select((rawComment) => JsonSerializer.Deserialize<Comment>(rawComment));

//   // return comments;
// });

app.Run();

public record User(string UserName, List<string> Links, List<string> Descriptions, List<bool> Purchased, ulong Id);
