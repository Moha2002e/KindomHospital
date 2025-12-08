using System.ComponentModel.DataAnnotations;

namespace KingdomHospital.Application.DTOs;

public record CreateOrdonnanceLigneDto(
    [Required] int MedicamentId,
    [Required] [MaxLength(50)] string Dosage,
    [Required] [MaxLength(50)] string Frequency,
    [Required] [MaxLength(30)] string Duration,
    [Required] [Range(1, int.MaxValue)] int Quantity,
    [MaxLength(255)] string? Instructions
);

