using Microsoft.EntityFrameworkCore;
using PortfolioWebsite.Domain.Entities;

namespace PortfolioWebsite.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<SkillCategory> SkillCategories { get; }

    DbSet<Skill> Skills { get; }

    DbSet<Project> Projects { get; }

    DbSet<Experience> Experiences { get; }

    DbSet<ExperiencePoint> ExperiencePoints { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
