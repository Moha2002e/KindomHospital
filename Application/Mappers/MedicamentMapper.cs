using KingdomHospital.Application.DTOs;
using KingdomHospital.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace KingdomHospital.Application.Mappers;

[Mapper]
public partial class MedicamentMapper
{
    public partial MedicamentDto ToDto(Medicament medicament);
    public partial Medicament ToEntity(CreateMedicamentDto dto);
}

