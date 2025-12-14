using System.ComponentModel.DataAnnotations;

namespace KingdomHospital.Application.DTOs;

public record CreateMedicamentDto(
    [Required(ErrorMessage = "Le nom est requis.")] [MaxLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères.")] string Name,
    [Required(ErrorMessage = "La forme galénique est requise.")] [MaxLength(30, ErrorMessage = "La forme galénique ne peut pas dépasser 30 caractères.")] string DosageForm,
    [Required(ErrorMessage = "Le dosage est requis.")] [MaxLength(30, ErrorMessage = "Le dosage ne peut pas dépasser 30 caractères.")] string Strength,
    [MaxLength(20, ErrorMessage = "Le code ATC ne peut pas dépasser 20 caractères.")] string? AtcCode
);

