using Microsoft.AspNetCore.Mvc.Testing;
using Npgsql;
using Testcontainers.PostgreSql;

namespace UrlShortener.TokenRangeService.Tests;

public class Fixture : WebApplicationFactory<ITokenRangeAssemblyMarker>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer;
    private string ConnectionString => _postgreSqlContainer.GetConnectionString();

    public Fixture()
    {
        _postgreSqlContainer = new PostgreSqlBuilder()
            .Build();
    }
    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();
        Environment.SetEnvironmentVariable("Postgres__ConnectionString", ConnectionString);
        await InitializeSqlTable();
    }
    
    private async Task InitializeSqlTable()
    {
        var tableSql = await File.ReadAllTextAsync(
            "Table.sql");
        
        await using var connection = new NpgsqlConnection(ConnectionString);
        await connection.OpenAsync();
        
        await using var command = new NpgsqlCommand(tableSql, connection);
        await command.ExecuteNonQueryAsync();
    }

    public new async Task DisposeAsync()
    {
        await _postgreSqlContainer.StopAsync();
        await base.DisposeAsync();
    }
}