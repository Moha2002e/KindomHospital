using KingdomHospital.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KingdomHospital.Infrastructure.Configurations;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.ToTable("DOCTOR");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasColumnName("Id");

        builder.Property(d => d.SpecialtyId)
            .HasColumnName("SpecialtyId")
            .IsRequired();

        builder.Property(d => d.LastName)
            .HasColumnName("LastName")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(d => d.FirstName)
            .HasColumnName("FirstName")
            .HasMaxLength(30)
            .IsRequired();

        builder.HasOne(d => d.Specialty)
            .WithMany(s => s.Doctors)
            .HasForeignKey(d => d.SpecialtyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(d => d.Consultations)
            .WithOne(c => c.Doctor)
            .HasForeignKey(c => c.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(d => d.Ordonnances)
            .WithOne(o => o.Doctor)
            .HasForeignKey(o => o.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(d => new { d.LastName, d.FirstName })
            .HasDatabaseName("IX_DOCTOR_LastName_FirstName");
    }
}

