using KingdomHospital.Application.DTOs;
using KingdomHospital.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace KingdomHospital.Application.Mappers;

[Mapper]
public partial class OrdonnanceMapper
{
    private readonly OrdonnanceLigneMapper _ligneMapper = new();

    public OrdonnanceDto ToDtoWithDetails(Ordonnance ordonnance)
    {
        return new OrdonnanceDto(
            ordonnance.Id,
            ordonnance.DoctorId,
            $"{ordonnance.Doctor.FirstName} {ordonnance.Doctor.LastName}",
            ordonnance.PatientId,
            $"{ordonnance.Patient.FirstName} {ordonnance.Patient.LastName}",
            ordonnance.ConsultationId,
            ordonnance.Date,
            ordonnance.Notes,
            ordonnance.OrdonnanceLignes.Select(l => _ligneMapper.ToDtoWithMedicament(l)).ToList()
        );
    }
}

