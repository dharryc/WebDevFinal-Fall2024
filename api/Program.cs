using System.Text.Json;
using Octokit;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();

var app = builder.Build();
app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.MapGet("/", () => "MY API IS RUNNING!!!!");

var storageRoot = "./storage/users.json";
var hashSetRoot = "./hash";

HashSet<string>? usersHash = [];
if (!Directory.Exists(hashSetRoot))
{
  Directory.CreateDirectory(hashSetRoot);
}
else
{
  var myHashSetThing = File.ReadAllText(hashSetRoot);
  usersHash = JsonSerializer.Deserialize<HashSet<string>>(myHashSetThing);
}

List<User>? allUsers = [];
if (!Directory.Exists("./storage"))
  Directory.CreateDirectory("./storage");
else
{
  allUsers = JsonSerializer.Deserialize<List<User>>(File.ReadAllText(storageRoot));
}
app.MapPost(
    "/newUser",
    async (User user) =>
    {
      if (!usersHash.Contains(user.UserName))
      {
        usersHash.Add(user.UserName);
        File.WriteAllText(
            hashSetRoot + "/hashObj.json",
            JsonSerializer.Serialize(usersHash)
        );
        await File.WriteAllTextAsync(storageRoot, JsonSerializer.Serialize(user));
        return "User successfuly created!";
      }
      return "That user already exists. If Harry broke something, please let him know";
    }
);

app.MapGet(
    "/userList",
    () =>
    {
      string[]? myUsers = new string[usersHash.Count()];
      usersHash.CopyTo(myUsers);
      return myUsers;
    }
);

app.MapGet(
    "/user/{userName}",
    (string userName) =>
    {
      var userFolder = userName;
      var userDirectory = Path.Combine(storageRoot, userFolder, "user.json");

      if (!File.Exists(userDirectory))
        throw new Exception("User not found.");

      var user = JsonSerializer.Deserialize<User>(File.ReadAllText(userDirectory));
      if (user == null)
        throw new Exception("user not found");

      return user;
    }
);

app.MapPost(
    "/user/{userName}/addItem/{newItemId}",
    async (ulong newItemId, string userName, Item newItem) =>
    {
      var userFolder = userName;
      var userDirectory = Path.Combine(storageRoot, userFolder, "user.json");
      // ulong itemId = 129438235;
      // Item thisthing = currentUser.Items[itemId];

      if (!File.Exists(userDirectory))
        throw new Exception("User not found.");

      User? currentUser =
          JsonSerializer.Deserialize<User>(File.ReadAllText(userDirectory))
          ?? throw new Exception("user not found");
      currentUser.Items ??= [];
      newItem.Purchased = false;
      currentUser.Items.Add(newItemId, newItem);
      var userPath = Path.Combine(storageRoot, userName);
      var userFile = Path.Combine(userPath, "user.json");
      await File.WriteAllTextAsync(userFile, JsonSerializer.Serialize(currentUser));
    }
);

app.MapDelete(
    "/user/{userName}/delete",
    (string userName) =>
    {
      if (!usersHash.Contains(userName))
      {
        throw new Exception("User not found");
      }
      var userFolder = userName;
      var userFile = Path.Combine(storageRoot, userName);
      usersHash.Remove(userName);
      File.WriteAllText(hashSetRoot + "/hashObj.json", JsonSerializer.Serialize(usersHash));
      Directory.Delete(userFile, true);
    }
);

app.MapGet(
    "/{userName}/items",
    (string userName) =>
    {
      var userFolder = userName;
      var userDirectory = Path.Combine(storageRoot, userFolder, "user.json");

      if (!File.Exists(userDirectory))
        throw new Exception("User not found.");
      User? currentUser =
          JsonSerializer.Deserialize<User>(File.ReadAllText(userDirectory))
          ?? throw new Exception("user not found");
      return currentUser.Items?.ToArray();
    }
);

app.MapGet(
    "/{userName}/{itemId}",
    async (string userName, ulong itemId) =>
    {
      var userFolder = userName;
      var userDirectory = Path.Combine(storageRoot, userFolder, "user.json");

      if (!File.Exists(userDirectory))
        throw new Exception("User not found.");
      User currentUser =
          JsonSerializer.Deserialize<User>(File.ReadAllText(userDirectory))
          ?? throw new Exception("user not found");
      Console.WriteLine(currentUser);
      currentUser.Items[itemId].Purchased = !currentUser.Items[itemId].Purchased;
      var userPath = Path.Combine(storageRoot, userName);
      var userFile = Path.Combine(userPath, "user.json");
      await File.WriteAllTextAsync(userFile, JsonSerializer.Serialize(currentUser));
      Console.WriteLine(currentUser.Items[itemId].Purchased);
    }
);

app.MapGet(
    "/allUsers",
    () =>
    {
      return allUsers;
    }
);

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
