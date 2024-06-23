using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SzczurApp.Data;
using SzczurApp.Services;

namespace SzczurTests.Services;

public class StreamingServiceTests : StreamingServiceTestBase
{
    private UserManager<ApplicationUser> CreateUserManager(ApplicationDbContext context)
    {
        var store = new UserStore<ApplicationUser, IdentityRole, ApplicationDbContext>(context);

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton<IUserStore<ApplicationUser>>(store);
        services.AddSingleton<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();
        services.AddSingleton<ILookupNormalizer, UpperInvariantLookupNormalizer>();
        services.AddSingleton<IdentityErrorDescriber>();
        services.AddSingleton<IUserValidator<ApplicationUser>, UserValidator<ApplicationUser>>();
        services.AddSingleton<
            IPasswordValidator<ApplicationUser>,
            PasswordValidator<ApplicationUser>
        >();
        services.AddSingleton<
            ILogger<UserManager<ApplicationUser>>,
            Logger<UserManager<ApplicationUser>>
        >();

        var serviceProvider = services.BuildServiceProvider();

        var options = serviceProvider.GetRequiredService<IOptions<IdentityOptions>>();
        var pwdHasher = serviceProvider.GetRequiredService<IPasswordHasher<ApplicationUser>>();
        var userValidators = serviceProvider.GetServices<IUserValidator<ApplicationUser>>();
        var pwdValidators = serviceProvider.GetServices<IPasswordValidator<ApplicationUser>>();
        var lookupNormalizer = serviceProvider.GetRequiredService<ILookupNormalizer>();
        var errors = serviceProvider.GetRequiredService<IdentityErrorDescriber>();
        var logger = serviceProvider.GetRequiredService<ILogger<UserManager<ApplicationUser>>>();

        return new UserManager<ApplicationUser>(
            store,
            options,
            pwdHasher,
            userValidators,
            pwdValidators,
            lookupNormalizer,
            errors,
            serviceProvider,
            logger
        );
    }

    [Fact]
    public async Task GenerateStreamKeyAsync_UserExists_GeneratesKey()
    {
        using (var context = CreateContext())
        {
            var userManager = CreateUserManager(context);
            var streamingService = new StreamingService(userManager, Configuration);

            var user = new ApplicationUser { UserName = "testuser", Email = "test@example.com" };
            await userManager.CreateAsync(user, "Password123!");

            var streamKey = await streamingService.GenerateStreamKeyAsync(user.UserName);

            var updatedUser = await userManager.FindByNameAsync(user.UserName);
            Assert.Equal(streamKey, updatedUser.StreamKey);
        }
    }

    [Fact]
    public async Task GetStreamKeyAsync_UserExists_ReturnsKey()
    {
        using (var context = CreateContext())
        {
            var userManager = CreateUserManager(context);
            var streamingService = new StreamingService(userManager, Configuration);

            var user = new ApplicationUser
            {
                UserName = "testuser",
                Email = "test@example.com",
                StreamKey = "existingkey"
            };
            await userManager.CreateAsync(user, "Password123!");

            var streamKey = await streamingService.GetStreamKeyAsync(user.UserName);

            Assert.Equal("existingkey", streamKey);
        }
    }

    [Fact]
    public async Task GetStreamUrlAsync_UserExists_ReturnsUrl()
    {
        using (var context = CreateContext())
        {
            var userManager = CreateUserManager(context);
            var streamingService = new StreamingService(userManager, Configuration);

            var user = new ApplicationUser
            {
                UserName = "testuser",
                Email = "test@example.com",
                StreamKey = "existingkey"
            };
            await userManager.CreateAsync(user, "Password123!");

            var streamUrl = await streamingService.GetStreamUrlAsync(user.UserName);

            Assert.Equal("http://localhost:8083/hls/existingkey.m3u8", streamUrl);
        }
    }

    [Fact]
    public async Task GetWatchStreamUrlAsync_UserExists_ReturnsWatchUrl()
    {
        using (var context = CreateContext())
        {
            var userManager = CreateUserManager(context);
            var streamingService = new StreamingService(userManager, Configuration);

            var user = new ApplicationUser { UserName = "testuser", Email = "test@example.com" };
            await userManager.CreateAsync(user, "Password123!");

            var watchStreamUrl = await streamingService.GetWatchStreamUrlAsync(user.UserName);

            Assert.Equal("http://localhost:8082/user/testuser", watchStreamUrl);
        }
    }

    [Fact]
    public async Task GenerateStreamKeyAsync_UserDoesNotExist_ThrowsException()
    {
        using (var context = CreateContext())
        {
            var userManager = CreateUserManager(context);
            var streamingService = new StreamingService(userManager, Configuration);

            await Assert.ThrowsAsync<Exception>(
                () => streamingService.GenerateStreamKeyAsync("nonexistentuser")
            );
        }
    }

    [Fact]
    public async Task GetStreamKeyAsync_UserDoesNotExist_ThrowsException()
    {
        using (var context = CreateContext())
        {
            var userManager = CreateUserManager(context);
            var streamingService = new StreamingService(userManager, Configuration);

            await Assert.ThrowsAsync<Exception>(
                () => streamingService.GetStreamKeyAsync("nonexistentuser")
            );
        }
    }

    [Fact]
    public async Task GetStreamUrlAsync_UserDoesNotExist_ThrowsException()
    {
        using (var context = CreateContext())
        {
            var userManager = CreateUserManager(context);
            var streamingService = new StreamingService(userManager, Configuration);

            await Assert.ThrowsAsync<Exception>(
                () => streamingService.GetStreamUrlAsync("nonexistentuser")
            );
        }
    }
}
