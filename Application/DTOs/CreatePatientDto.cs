using System.ComponentModel.DataAnnotations;

namespace KingdomHospital.Application.DTOs;

public record CreatePatientDto(
    [Required] [MaxLength(30)] string LastName,
    [Required] [MaxLength(30)] string FirstName,
    [Required] DateOnly BirthDate
);

