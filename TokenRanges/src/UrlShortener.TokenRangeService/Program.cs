using Azure.Identity;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using UrlShortener.TokenRangeService;

var builder = WebApplication.CreateBuilder(args);

var keyVaultName = builder.Configuration["KeyVaultName"];
if (!string.IsNullOrEmpty(keyVaultName))
{
    builder.Configuration.AddAzureKeyVault(
        new Uri($"https://{keyVaultName}.vault.azure.net/"),
        new DefaultAzureCredential());
}

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration["Postgres:ConnectionString"]!);

builder.Services.AddSingleton(
    new TokenRangeManger(builder.Configuration["Postgres:ConnectionString"]!));

var telemetryConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
if (telemetryConnectionString is not null)
{
    builder.Services.AddOpenTelemetry().UseAzureMonitor();
}

var app = builder.Build();

app.UseHttpsRedirection();
app.MapHealthChecks("/healthz");

app.MapGet("/", () => "TokenRanges service");
app.MapPost("/assign",
    async (AssignTokenRangeRequest request, TokenRangeManger manager) =>
    {
        var range = await manager.AssignRangeAsync(request.Key);

        return range;
    });

app.Run();
