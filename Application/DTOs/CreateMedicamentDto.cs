using System.ComponentModel.DataAnnotations;

namespace KingdomHospital.Application.DTOs;

public record CreateMedicamentDto(
    [Required] [MaxLength(100)] string Name,
    [Required] [MaxLength(30)] string DosageForm,
    [Required] [MaxLength(30)] string Strength,
    [MaxLength(20)] string? AtcCode
);

