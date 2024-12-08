using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Octokit;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
var ghClient = new GitHubClient(new ProductHeaderValue("GithubCommit"));
string? githubToken = Environment.GetEnvironmentVariable("TOKEN");
Console.WriteLine(githubToken);
ghClient.Credentials = new Credentials(githubToken);

// github variables
var owner = "dharryc";
var repo = "WebDevFinal-Fall2024";
var branch = "main";
var targetFile = "./api/storage/users.json";

var app = builder.Build();
app.UseCors(c =>
{
    c.AllowAnyHeader();
    c.AllowAnyMethod();
    c.AllowAnyOrigin();
});

app.MapGet("/", () => "MY API IS RUNNING!!!!");

var storageRoot = "./storage/users.json";
var hashSetRoot = "./hash";

HashSet<string> usersHash = [];
if (!Directory.Exists(hashSetRoot))
{
    Directory.CreateDirectory(hashSetRoot);
}
else
{
    var myHashSetThing = File.ReadAllText(hashSetRoot + "/hashObj.json");
    usersHash = JsonSerializer.Deserialize<HashSet<string>>(myHashSetThing);
}

List<User> allUsers = [];
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
            allUsers.Add(user);
            await File.WriteAllTextAsync(storageRoot, JsonSerializer.Serialize(allUsers));
            try
            {
                // try to get the file (and with the file the last commit sha)
                var existingFile = await ghClient.Repository.Content.GetAllContentsByRef(
                    owner,
                    repo,
                    targetFile,
                    branch
                );

                // update the file
                var updateChangeSet = await ghClient.Repository.Content.UpdateFile(
                    owner,
                    repo,
                    targetFile,
                    new UpdateFileRequest(
                        "API updated users",
                        File.ReadAllText(storageRoot),
                        existingFile.First().Sha,
                        branch
                    )
                );
            }
            catch (Octokit.NotFoundException)
            {
                // if file is not found, create it
                var createChangeSet = await ghClient.Repository.Content.CreateFile(
                    owner,
                    repo,
                    targetFile,
                    new CreateFileRequest(
                        "API File creation",
                        "Hello Universe! " + DateTime.UtcNow,
                        branch
                    )
                );
            }
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
        int index = allUsers.FindIndex(u => u.UserName == userName);
        allUsers.ElementAt(index).Items ??= [];
        newItem.Purchased = false;
        allUsers?.ElementAt(index).Items?.Add(newItemId, newItem);
        await File.WriteAllTextAsync(storageRoot, JsonSerializer.Serialize(allUsers));
    }
);

app.MapPost(
    "/{userName}/{newItemId}/addDetails",
    async (ulong newItemId, string userName, [FromBody] string details) =>
    {
        int index = allUsers.FindIndex(u => u.UserName == userName);
        allUsers.ElementAt(index).Items[newItemId].MoreDetails = details;
        await File.WriteAllTextAsync(storageRoot, JsonSerializer.Serialize(allUsers));
    }
);

app.MapDelete(
    "/user/{userName}/delete",
    async (string userName) =>
    {
        if (!usersHash.Contains(userName))
        {
            throw new Exception("User not found");
        }
        allUsers?.Remove(allUsers.Find(u => u.UserName == userName));
        await File.WriteAllTextAsync(storageRoot, JsonSerializer.Serialize(allUsers));
    }
);
app.MapDelete(
    "/{userName}/{itemId}/deleteItem",
    async (string userName, ulong itemId) =>
    {
        int index = allUsers.FindIndex(u => u.UserName == userName);
        allUsers.ElementAt(index).Items.Remove(itemId);
        await File.WriteAllTextAsync(storageRoot, JsonSerializer.Serialize(allUsers));
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
    public string? MoreDetails { get; set; }
};

public record User
{
    public string? UserName { get; init; }
    public Dictionary<ulong, Item>? Items { get; set; }
    public ulong Id { get; init; }
}
