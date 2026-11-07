using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PortfolioWebsite.Infrastructure.Persistence;

namespace PortfolioWebsite.Application.Tests;

/// <summary>
/// Creates an isolated SQLite in-memory database for a single test.
/// Foreign keys are enabled so cascade deletes behave like SQL Server.
/// </summary>
public sealed class TestDatabase : IAsyncDisposable
{
    private readonly SqliteConnection _connection;

    private TestDatabase(SqliteConnection connection, ApplicationDbContext context)
    {
        _connection = connection;
        Context = context;
    }

    public ApplicationDbContext Context { get; }

    public static async Task<TestDatabase> CreateAsync()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        await using (var command = connection.CreateCommand())
        {
            command.CommandText = "PRAGMA foreign_keys = ON;";
            await command.ExecuteNonQueryAsync();
        }

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new ApplicationDbContext(options);
        await context.Database.EnsureCreatedAsync();

        return new TestDatabase(connection, context);
    }

    public async ValueTask DisposeAsync()
    {
        await Context.DisposeAsync();
        await _connection.DisposeAsync();
    }
}
