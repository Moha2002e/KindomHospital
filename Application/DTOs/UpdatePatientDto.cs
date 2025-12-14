using System.ComponentModel.DataAnnotations;

namespace KingdomHospital.Application.DTOs;

public record UpdatePatientDto(
    [Required(ErrorMessage = "Le nom est requis.")] [MaxLength(30, ErrorMessage = "Le nom ne peut pas dépasser 30 caractères.")] string LastName,
    [Required(ErrorMessage = "Le prénom est requis.")] [MaxLength(30, ErrorMessage = "Le prénom ne peut pas dépasser 30 caractères.")] string FirstName,
    [Required(ErrorMessage = "La date de naissance est requise.")] DateOnly BirthDate
);

