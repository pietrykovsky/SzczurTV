using System.ComponentModel.DataAnnotations;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SzczurApp.Data;

namespace SzczurTests.Users;

public abstract class UserTestBase : IDisposable
{
    private readonly SqliteConnection _connection;
    protected DbContextOptions<ApplicationDbContext> Options { get; }

    protected UserTestBase()
    {
        // Establish an in-memory SQLite connection
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        Options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection) // Configure the context to use SQLite in-memory
            .Options;

        // Ensure the database is created
        using (var context = new ApplicationDbContext(Options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }

    protected ApplicationDbContext CreateContext() => new ApplicationDbContext(Options);

    public bool ValidateModel(object model, out List<ValidationResult> results)
    {
        var context = new ValidationContext(model, serviceProvider: null, items: null);
        results = new List<ValidationResult>();
        return Validator.TryValidateObject(model, context, results, true);
    }

    public void Dispose()
    {
        _connection.Close();
    }
}
