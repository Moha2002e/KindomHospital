using System.ComponentModel.DataAnnotations;

namespace KingdomHospital.Application.DTOs;

public record UpdateConsultationDto(
    [Required] int DoctorId,
    [Required] int PatientId,
    [Required] DateOnly Date,
    [Required] TimeOnly Hour,
    [MaxLength(100)] string? Reason
);

