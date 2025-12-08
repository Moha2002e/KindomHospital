using KingdomHospital.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KingdomHospital.Infrastructure.Configurations;

public class OrdonnanceConfiguration : IEntityTypeConfiguration<Ordonnance>
{
    public void Configure(EntityTypeBuilder<Ordonnance> builder)
    {
        builder.ToTable("ORDONNANCE");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasColumnName("Id");

        builder.Property(o => o.DoctorId)
            .HasColumnName("DoctorId")
            .IsRequired();

        builder.Property(o => o.PatientId)
            .HasColumnName("PatientId")
            .IsRequired();

        builder.Property(o => o.ConsultationId)
            .HasColumnName("ConsultationId");

        builder.Property(o => o.Date)
            .HasColumnName("Date")
            .IsRequired();

        builder.Property(o => o.Notes)
            .HasColumnName("Notes")
            .HasMaxLength(255);

        builder.HasOne(o => o.Doctor)
            .WithMany(d => d.Ordonnances)
            .HasForeignKey(o => o.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.Patient)
            .WithMany(p => p.Ordonnances)
            .HasForeignKey(o => o.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.Consultation)
            .WithMany(c => c.Ordonnances)
            .HasForeignKey(o => o.ConsultationId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(o => o.OrdonnanceLignes)
            .WithOne(ol => ol.Ordonnance)
            .HasForeignKey(ol => ol.OrdonnanceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

