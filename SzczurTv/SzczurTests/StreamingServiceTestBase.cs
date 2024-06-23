using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SzczurApp.Data;

namespace SzczurTests.Services;

public abstract class StreamingServiceTestBase : IDisposable
{
    private readonly SqliteConnection _connection;
    protected DbContextOptions<ApplicationDbContext> Options { get; }
    protected IConfiguration Configuration { get; }

    protected StreamingServiceTestBase()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        Options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection)
            .Options;

        Configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
                new Dictionary<string, string>
                {
                    { "StreamingUrl", "http://localhost:8083/hls" },
                    { "DomainUrl", "http://localhost:8082" }
                }
            )
            .Build();

        using (var context = new ApplicationDbContext(Options))
        {
            context.Database.EnsureCreated();
        }
    }

    protected ApplicationDbContext CreateContext() => new ApplicationDbContext(Options);

    public void Dispose()
    {
        _connection.Close();
    }
}
