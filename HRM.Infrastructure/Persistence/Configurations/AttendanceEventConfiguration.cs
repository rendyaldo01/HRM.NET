using HRM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRM.Infrastructure.Persistence.Configurations;

public class AttendanceEventConfiguration
    : IEntityTypeConfiguration<AttendanceEvent>
{
    public void Configure(EntityTypeBuilder<AttendanceEvent> builder)
    {
        builder.ToTable("AttendanceEvents");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.EventId)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.CardUid)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.OccurredAtUtc)
            .IsRequired();

        builder.Property(x => x.ReceivedAtUtc)
            .IsRequired();

        builder.HasIndex(x => new { x.DeviceId, x.EventId })
            .IsUnique();

        builder.HasIndex(x => x.CardUid);

        builder.HasIndex(x => x.OccurredAtUtc);
    }
}
