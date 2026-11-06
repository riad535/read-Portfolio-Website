using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioWebsite.Domain.Entities;

namespace PortfolioWebsite.Infrastructure.Persistence.Configurations;

public class SkillCategoryConfiguration : IEntityTypeConfiguration<SkillCategory>
{
    public void Configure(EntityTypeBuilder<SkillCategory> builder)
    {
        builder.ToTable("SkillCategories");

        builder.HasKey(category => category.Id);

        builder.Property(category => category.Id)
            .UseIdentityColumn();

        builder.Property(category => category.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(category => category.IconClass)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(category => category.DisplayOrder)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(category => category.CreatedAtUtc)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.Property(category => category.UpdatedAtUtc)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.HasIndex(category => category.Name)
            .IsUnique();
    }
}
