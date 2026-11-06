using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioWebsite.Domain.Entities;

namespace PortfolioWebsite.Infrastructure.Persistence.Configurations;

public class SkillConfiguration : IEntityTypeConfiguration<Skill>
{
    public void Configure(EntityTypeBuilder<Skill> builder)
    {
        builder.ToTable("Skills");

        builder.HasKey(skill => skill.Id);

        builder.Property(skill => skill.Id)
            .UseIdentityColumn();

        builder.Property(skill => skill.Name)
            .IsRequired()
            .HasMaxLength(60);

        builder.Property(skill => skill.DisplayOrder)
            .IsRequired()
            .HasDefaultValue(0);

        builder.HasOne(skill => skill.SkillCategory)
            .WithMany(category => category.Skills)
            .HasForeignKey(skill => skill.SkillCategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(skill => new { skill.SkillCategoryId, skill.Name })
            .IsUnique();
    }
}
