namespace KingdomHospital.Application.DTOs;

public record OrdonnanceDto(
    int Id,
    int DoctorId,
    string DoctorName,
    int PatientId,
    string PatientName,
    int? ConsultationId,
    DateOnly Date,
    string? Notes,
    List<OrdonnanceLigneDto> Lignes
);

