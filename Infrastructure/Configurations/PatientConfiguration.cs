using KingdomHospital.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KingdomHospital.Infrastructure.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.ToTable("PATIENT");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("Id");

        builder.Property(p => p.LastName)
            .HasColumnName("LastName")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(p => p.FirstName)
            .HasColumnName("FirstName")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(p => p.BirthDate)
            .HasColumnName("BirthDate")
            .IsRequired();

        builder.HasMany(p => p.Consultations)
            .WithOne(c => c.Patient)
            .HasForeignKey(c => c.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Ordonnances)
            .WithOne(o => o.Patient)
            .HasForeignKey(o => o.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(p => new { p.LastName, p.FirstName, p.BirthDate })
            .HasDatabaseName("IX_PATIENT_LastName_FirstName_BirthDate");
    }
}

