using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioWebsite.Domain.Entities;

namespace PortfolioWebsite.Infrastructure.Persistence.Configurations;

public class ExperiencePointConfiguration : IEntityTypeConfiguration<ExperiencePoint>
{
    public void Configure(EntityTypeBuilder<ExperiencePoint> builder)
    {
        builder.ToTable("ExperiencePoints");

        builder.HasKey(point => point.Id);

        builder.Property(point => point.Id)
            .UseIdentityColumn();

        builder.Property(point => point.Text)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(point => point.DisplayOrder)
            .IsRequired()
            .HasDefaultValue(0);

        builder.HasOne(point => point.Experience)
            .WithMany(experience => experience.Points)
            .HasForeignKey(point => point.ExperienceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
