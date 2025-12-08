using KingdomHospital.Application.DTOs;
using KingdomHospital.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace KingdomHospital.Application.Mappers;

[Mapper]
public partial class PatientMapper
{
    public partial PatientDto ToDto(Patient patient);
    public partial Patient ToEntity(CreatePatientDto dto);
    
    public Patient ToEntity(UpdatePatientDto dto, Patient existing)
    {
        existing.LastName = dto.LastName;
        existing.FirstName = dto.FirstName;
        existing.BirthDate = dto.BirthDate;
        return existing;
    }
}

