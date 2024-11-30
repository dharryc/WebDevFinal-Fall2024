using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;

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

app.MapPost("/user/{userName}/addItem/{newItemId}", async (ulong newItemId, string userName, Item newItem) =>
{
  var userFolder = userName;
  var userDirectory = Path.Combine(storageRoot, userFolder, "user.json");
  // ulong itemId = 129438235;
  // Item thisthing = currentUser.Items[itemId];

  if (!File.Exists(userDirectory))
    throw new Exception("User not found.");

  User? currentUser = JsonSerializer.Deserialize<User>(File.ReadAllText(userDirectory)) ?? throw new Exception("user not found");
  currentUser.Items ??= [];
  newItem.Purchased = false;
  currentUser.Items.Add(newItemId, newItem);
  var userPath = Path.Combine(storageRoot, userName);
  var userFile = Path.Combine(userPath, "user.json");
  await File.WriteAllTextAsync(userFile, JsonSerializer.Serialize(currentUser));

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

app.MapGet("/{userName}/items", (string userName) =>
{
  var userFolder = userName;
  var userDirectory = Path.Combine(storageRoot, userFolder, "user.json");

  if (!File.Exists(userDirectory)) throw new Exception("User not found.");
  User? currentUser = JsonSerializer.Deserialize<User>(File.ReadAllText(userDirectory)) ?? throw new Exception("user not found");
  return currentUser.Items?.ToArray();
});

app.MapGet("/{userName}/{itemId}", async (string userName, ulong itemId) =>
{
  var userFolder = userName;
  var userDirectory = Path.Combine(storageRoot, userFolder, "user.json");

  if (!File.Exists(userDirectory)) throw new Exception("User not found.");
  User currentUser = JsonSerializer.Deserialize<User>(File.ReadAllText(userDirectory)) ?? throw new Exception("user not found");
  Console.WriteLine(currentUser);
  currentUser.Items[itemId].Purchased = !currentUser.Items[itemId].Purchased;
  var userPath = Path.Combine(storageRoot, userName);
  var userFile = Path.Combine(userPath, "user.json");
  await File.WriteAllTextAsync(userFile, JsonSerializer.Serialize(currentUser));
  Console.WriteLine(currentUser.Items[itemId].Purchased);
});

app.MapGet("/allUsers", () =>
{
  List<User> usersToReturn = [];
  foreach (string name in ExistingUsers)
  {
    var userFolder = name;
    var userDirectory = Path.Combine(storageRoot, userFolder, "user.json");

    if (!File.Exists(userDirectory)) throw new Exception("User not found.");
    User currentUser = JsonSerializer.Deserialize<User>(File.ReadAllText(userDirectory)) ?? throw new Exception("user not found");
    usersToReturn.Add(currentUser);
  }
  return usersToReturn;
});

app.Run();

public record Item
{
  public string? Link { get; set; }
  public string? Description { get; set; }
  public bool Purchased { get; set; }
};
public record User
{
  public string? UserName { get; init; }
  public Dictionary<ulong, Item>? Items { get; set; }
  public ulong Id { get; init; }
}
