namespace KingdomHospital.Application.DTOs;

public record ConsultationDto(
    int Id,
    int DoctorId,
    string DoctorName,
    int PatientId,
    string PatientName,
    DateOnly Date,
    TimeOnly Hour,
    string? Reason
);

