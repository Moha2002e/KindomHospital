using KingdomHospital.Application.DTOs;
using KingdomHospital.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace KingdomHospital.Application.Mappers;

[Mapper]
public partial class ConsultationMapper
{
    public ConsultationDto ToDtoWithNames(Consultation consultation)
    {
        return new ConsultationDto(
            consultation.Id,
            consultation.DoctorId,
            $"{consultation.Doctor.FirstName} {consultation.Doctor.LastName}",
            consultation.PatientId,
            $"{consultation.Patient.FirstName} {consultation.Patient.LastName}",
            consultation.Date,
            consultation.Hour,
            consultation.Reason
        );
    }
    
    public partial Consultation ToEntity(CreateConsultationDto dto);
    
    public Consultation ToEntity(UpdateConsultationDto dto, Consultation existing)
    {
        existing.DoctorId = dto.DoctorId;
        existing.PatientId = dto.PatientId;
        existing.Date = dto.Date;
        existing.Hour = dto.Hour;
        existing.Reason = dto.Reason;
        return existing;
    }
}

