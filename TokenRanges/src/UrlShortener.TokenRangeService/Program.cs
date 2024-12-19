using Azure.Identity;
using UrlShortener.TokenRangeService;

var builder = WebApplication.CreateBuilder(args);

var keyVaultName = builder.Configuration["KeyVaultName"];
if (!string.IsNullOrEmpty(keyVaultName))
{
    builder.Configuration.AddAzureKeyVault(
        new Uri($"https://{keyVaultName}.vault.azure.net/"),
        new DefaultAzureCredential());
}

builder.Services.AddSingleton(
    new TokenRangeManger(builder.Configuration["Postgres:ConnectionString"]!));

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/", () => "TokenRanges service");
app.MapPost("/assign",
    async (AssignTokenRangeRequest request, TokenRangeManger manager) =>
    {
        var range = await manager.AssignRangeAsync(request.Key);

        return range;
    });

app.Run();
