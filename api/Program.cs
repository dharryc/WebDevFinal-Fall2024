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
            File.WriteAllText(hashSetRoot + "/hashObj.json", JsonSerializer.Serialize(usersHash));
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
        return allUsers?.Find(u => u.UserName == userName);
    }
);

app.MapPost(
    "/user/{userName}/addItem/{newItemId}",
    async (ulong newItemId, string userName, Item newItem) =>
    {
        int i = 1300000;
        int? index = allUsers?.FindIndex(u => u.UserName == userName);
        if (index != null) i = (int)index;
        allUsers.ElementAt(i).Items ??= [];
        newItem.Purchased = false;
        allUsers?.ElementAt(i).Items?.Add(newItemId, newItem);
        await File.WriteAllTextAsync(storageRoot, JsonSerializer.Serialize(allUsers));
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
        allUsers?.Remove(allUsers.Find(u => u.UserName == userName));
    }
);

app.MapGet(
    "/{userName}/items",
    (string userName) =>
    {
        User? user = allUsers?.Find(u => u.UserName == userName);
        return user?.Items?.ToArray();
    }
);

app.MapGet(
    "/{userName}/{itemId}",
    async (string userName, ulong itemId) =>
    {
        int index = allUsers.FindIndex(u => u.UserName == userName);
        allUsers.ElementAt(index).Items[itemId].Purchased = !allUsers.ElementAt(index).Items[itemId].Purchased;
        await File.WriteAllTextAsync(storageRoot, JsonSerializer.Serialize(allUsers));
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
