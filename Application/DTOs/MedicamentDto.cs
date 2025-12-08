namespace KingdomHospital.Application.DTOs;

public record MedicamentDto(
    int Id,
    string Name,
    string DosageForm,
    string Strength,
    string? AtcCode
);

