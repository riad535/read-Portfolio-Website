using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioWebsite.Domain.Entities;

namespace PortfolioWebsite.Infrastructure.Persistence.Configurations;

public class ExperienceConfiguration : IEntityTypeConfiguration<Experience>
{
    public void Configure(EntityTypeBuilder<Experience> builder)
    {
        builder.ToTable("Experiences", table =>
        {
            table.HasCheckConstraint(
                "CK_Experiences_EndDate",
                "[EndDate] IS NULL OR [EndDate] >= [StartDate]");

            table.HasCheckConstraint(
                "CK_Experiences_IsCurrent",
                "[IsCurrent] = 0 OR [EndDate] IS NULL");
        });

        builder.HasKey(experience => experience.Id);

        builder.Property(experience => experience.Id)
            .UseIdentityColumn();

        builder.Property(experience => experience.Role)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(experience => experience.Organisation)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(experience => experience.StartDate)
            .IsRequired()
            .HasColumnType("date");

        builder.Property(experience => experience.EndDate)
            .HasColumnType("date");

        builder.Property(experience => experience.IsCurrent)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(experience => experience.IsPublished)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(experience => experience.CreatedAtUtc)
            .IsRequired()
            .HasColumnType("datetime2");

        builder.Property(experience => experience.UpdatedAtUtc)
            .IsRequired()
            .HasColumnType("datetime2");
    }
}
