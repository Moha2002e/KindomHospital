namespace KingdomHospital.Application.DTOs;

public record OrdonnanceLigneDto(
    int Id,
    int MedicamentId,
    string MedicamentName,
    string Dosage,
    string Frequency,
    string Duration,
    int Quantity,
    string? Instructions
);

