using KingdomHospital.Application.DTOs;
using KingdomHospital.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace KingdomHospital.Application.Mappers;

[Mapper]
public partial class OrdonnanceLigneMapper
{
    public OrdonnanceLigneDto ToDtoWithMedicament(OrdonnanceLigne ligne)
    {
        return new OrdonnanceLigneDto(
            ligne.Id,
            ligne.MedicamentId,
            ligne.Medicament.Name,
            ligne.Dosage,
            ligne.Frequency,
            ligne.Duration,
            ligne.Quantity,
            ligne.Instructions
        );
    }
    
    public partial OrdonnanceLigne ToEntity(CreateOrdonnanceLigneDto dto);
}

