using KingdomHospital.Domain.Entities;

namespace KingdomHospital.Application.Repositories;

public interface IConsultationRepository
{
    Task<IEnumerable<Consultation>> GetAllAsync();
    Task<Consultation?> GetByIdAsync(int id);
    Task<Consultation> CreateAsync(Consultation consultation);
    Task<Consultation> UpdateAsync(Consultation consultation);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<IEnumerable<Consultation>> GetByDoctorIdAsync(int doctorId, DateOnly? from, DateOnly? to);
    Task<IEnumerable<Consultation>> GetByPatientIdAsync(int patientId);
    Task<IEnumerable<Consultation>> GetFilteredAsync(int? doctorId, int? patientId, DateOnly? from, DateOnly? to);
    Task<IEnumerable<Ordonnance>> GetOrdonnancesByConsultationIdAsync(int consultationId);
    Task<bool> HasConflictAsync(int doctorId, int patientId, DateOnly date, TimeOnly hour, int? excludeId = null);
}

