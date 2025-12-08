using KingdomHospital.Domain.Entities;

namespace KingdomHospital.Application.Repositories;

public interface IOrdonnanceRepository
{
    Task<IEnumerable<Ordonnance>> GetAllAsync();
    Task<Ordonnance?> GetByIdAsync(int id);
    Task<Ordonnance> CreateAsync(Ordonnance ordonnance);
    Task<Ordonnance> UpdateAsync(Ordonnance ordonnance);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<IEnumerable<Ordonnance>> GetByDoctorIdAsync(int doctorId, DateOnly? from, DateOnly? to);
    Task<IEnumerable<Ordonnance>> GetByPatientIdAsync(int patientId);
    Task<IEnumerable<Ordonnance>> GetByConsultationIdAsync(int consultationId);
    Task<IEnumerable<Ordonnance>> GetFilteredAsync(int? doctorId, int? patientId, DateOnly? from, DateOnly? to);
    Task<IEnumerable<OrdonnanceLigne>> GetLignesByOrdonnanceIdAsync(int ordonnanceId);
    Task<OrdonnanceLigne?> GetLigneByIdAsync(int ordonnanceId, int ligneId);
}

