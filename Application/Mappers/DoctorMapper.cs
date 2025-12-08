using KingdomHospital.Application.DTOs;
using KingdomHospital.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace KingdomHospital.Application.Mappers;

[Mapper]
public partial class DoctorMapper
{
    public DoctorDto ToDtoWithSpecialty(Doctor doctor)
    {
        return new DoctorDto(
            doctor.Id,
            doctor.SpecialtyId,
            doctor.LastName,
            doctor.FirstName,
            doctor.Specialty.Name
        );
    }
    
    public partial Doctor ToEntity(CreateDoctorDto dto);
    
    public Doctor ToEntity(UpdateDoctorDto dto, Doctor existing)
    {
        existing.SpecialtyId = dto.SpecialtyId;
        existing.LastName = dto.LastName;
        existing.FirstName = dto.FirstName;
        return existing;
    }
}

