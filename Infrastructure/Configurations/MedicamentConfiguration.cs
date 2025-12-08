using KingdomHospital.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KingdomHospital.Infrastructure.Configurations;

public class MedicamentConfiguration : IEntityTypeConfiguration<Medicament>
{
    public void Configure(EntityTypeBuilder<Medicament> builder)
    {
        builder.ToTable("MEDICAMENT");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasColumnName("Id");

        builder.Property(m => m.Name)
            .HasColumnName("Name")
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(m => m.Name)
            .IsUnique();

        builder.Property(m => m.DosageForm)
            .HasColumnName("DosageForm")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(m => m.Strength)
            .HasColumnName("Strength")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(m => m.AtcCode)
            .HasColumnName("AtcCode")
            .HasMaxLength(20);

        builder.HasMany(m => m.OrdonnanceLignes)
            .WithOne(ol => ol.Medicament)
            .HasForeignKey(ol => ol.MedicamentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

