using KingdomHospital.Application.DTOs;
using KingdomHospital.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace KingdomHospital.Application.Mappers;

[Mapper]
public partial class SpecialtyMapper
{
    public partial SpecialtyDto ToDto(Specialty specialty);
    public partial Specialty ToEntity(SpecialtyDto dto);
}

