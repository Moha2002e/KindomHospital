using System.ComponentModel.DataAnnotations;

namespace KingdomHospital.Application.DTOs;

public record CreateOrdonnanceDto(
    [Required(ErrorMessage = "L'ID du médecin est requis.")] int DoctorId,
    [Required(ErrorMessage = "L'ID du patient est requis.")] int PatientId,
    int? ConsultationId,
    [Required(ErrorMessage = "La date est requise.")] DateOnly Date,
    [MaxLength(255, ErrorMessage = "Les notes ne peuvent pas dépasser 255 caractères.")] string? Notes,
    [Required(ErrorMessage = "Au moins une ligne d'ordonnance est requise.")] List<CreateOrdonnanceLigneDto> Lignes
);

