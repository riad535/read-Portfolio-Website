using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioWebsite.Domain.Entities;

namespace PortfolioWebsite.Infrastructure.Persistence.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects");

        builder.HasKey(project => project.Id);

        builder.Property(project => project.Id)
            .UseIdentityColumn();

        builder.Property(project => project.Title)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(project => project.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(project => project.TechStack)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(project => project.Architecture)
            .HasMaxLength(300);

        builder.Property(project => project.LiveDemoUrl)
            .HasMaxLength(500);

        builder.Property(project => project.GitHubUrl)
            .HasMaxLength(500);

        builder.Property(project => project.DisplayOrder)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(project => project.IsPublished)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(project => project.CreatedAtUtc)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.Property(project => project.UpdatedAtUtc)
            .IsRequired()
            .HasColumnType("datetime2");
    }
}
