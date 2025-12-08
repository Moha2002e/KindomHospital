using KingdomHospital.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KingdomHospital.Infrastructure.Configurations;

public class ConsultationConfiguration : IEntityTypeConfiguration<Consultation>
{
    public void Configure(EntityTypeBuilder<Consultation> builder)
    {
        builder.ToTable("CONSULTATION");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("Id");

        builder.Property(c => c.DoctorId)
            .HasColumnName("DoctorId")
            .IsRequired();

        builder.Property(c => c.PatientId)
            .HasColumnName("PatientId")
            .IsRequired();

        builder.Property(c => c.Date)
            .HasColumnName("Date")
            .IsRequired();

        builder.Property(c => c.Hour)
            .HasColumnName("Hour")
            .IsRequired();

        builder.Property(c => c.Reason)
            .HasColumnName("Reason")
            .HasMaxLength(100);

        builder.HasOne(c => c.Doctor)
            .WithMany(d => d.Consultations)
            .HasForeignKey(c => c.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Patient)
            .WithMany(p => p.Consultations)
            .HasForeignKey(c => c.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Ordonnances)
            .WithOne(o => o.Consultation)
            .HasForeignKey(o => o.ConsultationId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(c => new { c.DoctorId, c.Date, c.Hour })
            .HasDatabaseName("IX_CONSULTATION_DoctorId_Date_Hour");
    }
}

