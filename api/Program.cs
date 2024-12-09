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
var hashTargetFile = "./api/hash/hashObj.json";

var app = builder.Build();
app.UseCors(c =>
{
    c.AllowAnyHeader();
    c.AllowAnyMethod();
    c.AllowAnyOrigin();
});

app.MapGet("/", () => "MY API IS RUNNING!!!!");

var storageRoot = "./storage/users.json";

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
        if (allUsers.FindIndex(u => u.UserName == user.UserName) == -1)
        {
            allUsers.Add(user);
            File.WriteAllText(storageRoot, JsonSerializer.Serialize(allUsers));
            var existingFile = await ghClient.Repository.Content.GetAllContentsByRef(
                owner,
                repo,
                targetFile,
                branch
            );
            var updateChangeSet = await ghClient.Repository.Content.UpdateFile(
                owner,
                repo,
                targetFile,
                new UpdateFileRequest(
                    "API updated users",
                    JsonSerializer.Serialize(allUsers),
                    existingFile.First().Sha,
                    branch
                )
            );
            return "User successfuly created!";
        }
        return "That user already exists. If Harry broke something, please let him know";
    }
);

app.MapGet(
    "/userList",
    () =>
    {
        List<string> myUsers = [];
        foreach (User user in allUsers) myUsers.Add(user.UserName);
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
        File.WriteAllText(storageRoot, JsonSerializer.Serialize(allUsers));
        var existingFile = await ghClient.Repository.Content.GetAllContentsByRef(
            owner,
            repo,
            targetFile,
            branch
        );

        var updateChangeSet = await ghClient.Repository.Content.UpdateFile(
            owner,
            repo,
            targetFile,
            new UpdateFileRequest(
                "API updated users",
                JsonSerializer.Serialize(allUsers),
                existingFile.First().Sha,
                branch
            )
        );
    }
);

app.MapPost(
    "/{userName}/{newItemId}/addDetails",
    async (ulong newItemId, string userName, [FromBody] string details) =>
    {
        int index = allUsers.FindIndex(u => u.UserName == userName);
        allUsers.ElementAt(index).Items[newItemId].MoreDetails = details;
        File.WriteAllText(storageRoot, JsonSerializer.Serialize(allUsers));
        var existingFile = await ghClient.Repository.Content.GetAllContentsByRef(
            owner,
            repo,
            targetFile,
            branch
        );
        var updateChangeSet = await ghClient.Repository.Content.UpdateFile(
            owner,
            repo,
            targetFile,
            new UpdateFileRequest(
                "API updated users",
                JsonSerializer.Serialize(allUsers),
                existingFile.First().Sha,
                branch
            )
        );
    }
);

app.MapDelete(
    "/user/{userName}/delete",
    async (string userName) =>
    {
        if (allUsers.FindIndex(u => u.UserName == userName) == -1)
        {
            throw new Exception("User not found");
        }
        allUsers?.Remove(allUsers.Find(u => u.UserName == userName));
        File.WriteAllText(storageRoot, JsonSerializer.Serialize(allUsers));
        var existingFile = await ghClient.Repository.Content.GetAllContentsByRef(
            owner,
            repo,
            targetFile,
            branch
        );
        var updateChangeSet = await ghClient.Repository.Content.UpdateFile(
            owner,
            repo,
            targetFile,
            new UpdateFileRequest(
                "API updated users",
                JsonSerializer.Serialize(allUsers),
                existingFile.First().Sha,
                branch
            )
        );
    }
);
app.MapDelete(
    "/{userName}/{itemId}/deleteItem",
    async (string userName, ulong itemId) =>
    {
        int index = allUsers.FindIndex(u => u.UserName == userName);
        allUsers.ElementAt(index).Items.Remove(itemId);
        File.WriteAllText(storageRoot, JsonSerializer.Serialize(allUsers));
        var existingFile = await ghClient.Repository.Content.GetAllContentsByRef(
            owner,
            repo,
            targetFile,
            branch
        );
        var updateChangeSet = await ghClient.Repository.Content.UpdateFile(
            owner,
            repo,
            targetFile,
            new UpdateFileRequest(
                "API updated users",
                JsonSerializer.Serialize(allUsers),
                existingFile.First().Sha,
                branch
            )
        );
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
        File.WriteAllText(storageRoot, JsonSerializer.Serialize(allUsers));
        var existingFile = await ghClient.Repository.Content.GetAllContentsByRef(
            owner,
            repo,
            targetFile,
            branch
        );
        var updateChangeSet = await ghClient.Repository.Content.UpdateFile(
            owner,
            repo,
            targetFile,
            new UpdateFileRequest(
                "API updated users",
                JsonSerializer.Serialize(allUsers),
                existingFile.First().Sha,
                branch
            )
        );
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