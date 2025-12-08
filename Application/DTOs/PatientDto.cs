namespace KingdomHospital.Application.DTOs;

public record PatientDto(int Id, string LastName, string FirstName, DateOnly BirthDate);

