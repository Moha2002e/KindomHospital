using System.ComponentModel.DataAnnotations;

namespace KingdomHospital.Application.DTOs;

public record CreateOrdonnanceDto(
    [Required] int DoctorId,
    [Required] int PatientId,
    int? ConsultationId,
    [Required] DateOnly Date,
    [MaxLength(255)] string? Notes,
    [Required] List<CreateOrdonnanceLigneDto> Lignes
);

