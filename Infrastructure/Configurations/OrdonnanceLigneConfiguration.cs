using KingdomHospital.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KingdomHospital.Infrastructure.Configurations;

public class OrdonnanceLigneConfiguration : IEntityTypeConfiguration<OrdonnanceLigne>
{
    public void Configure(EntityTypeBuilder<OrdonnanceLigne> builder)
    {
        builder.ToTable("ORDONNANCE_LIGNE");

        builder.HasKey(ol => ol.Id);

        builder.Property(ol => ol.Id)
            .HasColumnName("Id");

        builder.Property(ol => ol.OrdonnanceId)
            .HasColumnName("OrdonnanceId")
            .IsRequired();

        builder.Property(ol => ol.MedicamentId)
            .HasColumnName("MedicamentId")
            .IsRequired();

        builder.Property(ol => ol.Dosage)
            .HasColumnName("Dosage")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(ol => ol.Frequency)
            .HasColumnName("Frequency")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(ol => ol.Duration)
            .HasColumnName("Duration")
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(ol => ol.Quantity)
            .HasColumnName("Quantity")
            .IsRequired();

        builder.ToTable(t => t.HasCheckConstraint("CK_OrdonnanceLigne_Quantity", "Quantity > 0"));

        builder.Property(ol => ol.Instructions)
            .HasColumnName("Instructions")
            .HasMaxLength(255);

        builder.HasOne(ol => ol.Ordonnance)
            .WithMany(o => o.OrdonnanceLignes)
            .HasForeignKey(ol => ol.OrdonnanceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ol => ol.Medicament)
            .WithMany(m => m.OrdonnanceLignes)
            .HasForeignKey(ol => ol.MedicamentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

