using System.ComponentModel.DataAnnotations;

namespace KingdomHospital.Application.DTOs;

public record UpdateDoctorDto(
    [Required(ErrorMessage = "L'ID de la spécialité est requis.")] int SpecialtyId,
    [Required(ErrorMessage = "Le nom est requis.")] [MaxLength(30, ErrorMessage = "Le nom ne peut pas dépasser 30 caractères.")] string LastName,
    [Required(ErrorMessage = "Le prénom est requis.")] [MaxLength(30, ErrorMessage = "Le prénom ne peut pas dépasser 30 caractères.")] string FirstName
);

