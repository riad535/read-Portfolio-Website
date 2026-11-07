using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using PortfolioWebsite.Infrastructure.Persistence.Seed;

namespace PortfolioWebsite.Application.Tests.Persistence;

public sealed class DbSeederTests
{
    [Fact]
    public async Task SeedAsync_WhenAllTablesEmpty_SeedsContent()
    {
        await using var database = await TestDatabase.CreateAsync();
        var seeder = CreateSeeder(database);

        await seeder.SeedAsync();

        Assert.Equal(6, await database.Context.SkillCategories.CountAsync());
        Assert.Equal(2, await database.Context.Projects.CountAsync());
        Assert.Equal(2, await database.Context.Experiences.CountAsync());
        Assert.True(await database.Context.Skills.AnyAsync());
        Assert.True(await database.Context.ExperiencePoints.AnyAsync());
    }

    [Fact]
    public async Task SeedAsync_WhenRunTwice_DoesNotDuplicateContent()
    {
        await using var database = await TestDatabase.CreateAsync();
        var seeder = CreateSeeder(database);

        await seeder.SeedAsync();
        await seeder.SeedAsync();

        Assert.Equal(6, await database.Context.SkillCategories.CountAsync());
        Assert.Equal(2, await database.Context.Projects.CountAsync());
        Assert.Equal(2, await database.Context.Experiences.CountAsync());
    }

    private static DbSeeder CreateSeeder(TestDatabase database)
    {
        var userManager = new UserManager<IdentityUser>(
            new UserStore<IdentityUser>(database.Context),
            Options.Create(new IdentityOptions()),
            new PasswordHasher<IdentityUser>(),
            new[] { new UserValidator<IdentityUser>() },
            new[] { new PasswordValidator<IdentityUser>() },
            new UpperInvariantLookupNormalizer(),
            new IdentityErrorDescriber(),
            new ServiceCollection().BuildServiceProvider(),
            NullLogger<UserManager<IdentityUser>>.Instance);

        return new DbSeeder(
            database.Context,
            userManager,
            new ConfigurationBuilder().Build(),
            NullLogger<DbSeeder>.Instance);
    }
}
