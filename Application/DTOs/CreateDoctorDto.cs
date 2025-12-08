using System.ComponentModel.DataAnnotations;

namespace KingdomHospital.Application.DTOs;

public record CreateDoctorDto(
    [Required] int SpecialtyId,
    [Required] [MaxLength(30)] string LastName,
    [Required] [MaxLength(30)] string FirstName
);

