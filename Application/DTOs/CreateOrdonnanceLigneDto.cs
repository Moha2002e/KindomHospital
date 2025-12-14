using System.ComponentModel.DataAnnotations;

namespace KingdomHospital.Application.DTOs;

public record CreateOrdonnanceLigneDto(
    [Required(ErrorMessage = "L'ID du médicament est requis.")] int MedicamentId,
    [Required(ErrorMessage = "Le dosage est requis.")] [MaxLength(50, ErrorMessage = "Le dosage ne peut pas dépasser 50 caractères.")] string Dosage,
    [Required(ErrorMessage = "La fréquence est requise.")] [MaxLength(50, ErrorMessage = "La fréquence ne peut pas dépasser 50 caractères.")] string Frequency,
    [Required(ErrorMessage = "La durée est requise.")] [MaxLength(30, ErrorMessage = "La durée ne peut pas dépasser 30 caractères.")] string Duration,
    [Required(ErrorMessage = "La quantité est requise.")] [Range(1, int.MaxValue, ErrorMessage = "La quantité doit être supérieure à 0.")] int Quantity,
    [MaxLength(255, ErrorMessage = "Les instructions ne peuvent pas dépasser 255 caractères.")] string? Instructions
);

