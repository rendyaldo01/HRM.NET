
using HRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRM.Infrastructure.Persistence.Configurations;

public class RfidCardConfiguration : IEntityTypeConfiguration<RfidCard>
{
    public void Configure(EntityTypeBuilder<RfidCard> builder)
    {
        builder.ToTable("RfidCards");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CardUid)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(x => x.CardUid)
            .IsUnique();

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.CreatedAtUtc)
            .IsRequired();

        builder.HasOne(x => x.Employee)
            .WithMany()
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.EmployeeId)
            .IsUnique()
            .HasFilter("[IsActive] = 1");
    }
}
