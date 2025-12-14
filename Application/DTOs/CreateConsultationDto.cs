using System.ComponentModel.DataAnnotations;

namespace KingdomHospital.Application.DTOs;

public record CreateConsultationDto(
    [Required(ErrorMessage = "L'ID du médecin est requis.")] int DoctorId,
    [Required(ErrorMessage = "L'ID du patient est requis.")] int PatientId,
    [Required(ErrorMessage = "La date est requise.")] DateOnly Date,
    [Required(ErrorMessage = "L'heure est requise.")] TimeOnly Hour,
    [MaxLength(100, ErrorMessage = "Le motif ne peut pas dépasser 100 caractères.")] string? Reason
);

