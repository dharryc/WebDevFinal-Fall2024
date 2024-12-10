using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Octokit;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
var ghClient = new GitHubClient(new ProductHeaderValue("GithubCommit"));
string? githubToken = Environment.GetEnvironmentVariable("TOKEN");
Console.WriteLine(githubToken);
ghClient.Credentials = new Credentials(githubToken);

// variables
var owner = "dharryc";
var repo = "WebDevFinal-Fall2024";
var branch = "main";
var targetFile = "./api/storage/users.json";
var storageRoot = "./storage/users.json";

var app = builder.Build();
app.UseCors(c =>
{
    c.AllowAnyHeader();
    c.AllowAnyMethod();
    c.AllowAnyOrigin();
});

List<User> allUsers = [];
if (!Directory.Exists("./storage"))
    Directory.CreateDirectory("./storage");
else
{
    allUsers = JsonSerializer.Deserialize<List<User>>(File.ReadAllText(storageRoot));
}

async Task pushToRepo()
{
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
    return;
}

app.MapPost(
    "/newUser",
    async (User user) =>
    {
        if (allUsers.FindIndex(u => u.UserName == user.UserName) == -1)
        {
            allUsers.Add(user);
            File.WriteAllText(storageRoot, JsonSerializer.Serialize(allUsers));
            await pushToRepo();
        }
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
        await pushToRepo();
    }
);

app.MapPost(
    "/{userName}/{newItemId}/addDetails",
    async (ulong newItemId, string userName, [FromBody] string details) =>
    {
        int index = allUsers.FindIndex(u => u.UserName == userName);
        allUsers.ElementAt(index).Items[newItemId].MoreDetails = details;
        File.WriteAllText(storageRoot, JsonSerializer.Serialize(allUsers));
        await pushToRepo();
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
        await pushToRepo();
    }
);
app.MapDelete(
    "/{userName}/{itemId}/deleteItem",
    async (string userName, ulong itemId) =>
    {
        int index = allUsers.FindIndex(u => u.UserName == userName);
        allUsers.ElementAt(index).Items.Remove(itemId);
        File.WriteAllText(storageRoot, JsonSerializer.Serialize(allUsers));
        await pushToRepo();
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
        await pushToRepo();
    }
);

app.MapGet(
    "/allUsers",
    () =>
    {
        return allUsers;
    }
);

app.MapPost("/{userName}/{birthDay}/setBirthday", async (string userName, ulong birthDay) =>
{
    int index = allUsers.FindIndex(u => u.UserName == userName);
    allUsers.ElementAt(index).BirthDay = birthDay;
    File.WriteAllText(storageRoot, JsonSerializer.Serialize(allUsers));
    await pushToRepo();
});

app.Run();

public record Item
{
    public string? Link { get; set; }
    public string? Description { get; set; }
    public bool Purchased { get; set; }
    public string? MoreDetails { get; set; }
    public string? Priority = "Default";
};

public record User
{
    public string? UserName { get; init; }
    public Dictionary<ulong, Item>? Items { get; set; }
    public ulong Id { get; init; }
    public ulong? BirthDay { get; set; }
}