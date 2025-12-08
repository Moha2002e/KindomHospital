using KingdomHospital.Domain.Entities;

namespace KingdomHospital.Application.Repositories;

public interface IPatientRepository
{
    Task<IEnumerable<Patient>> GetAllAsync();
    Task<Patient?> GetByIdAsync(int id);
    Task<Patient> CreateAsync(Patient patient);
    Task<Patient> UpdateAsync(Patient patient);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<IEnumerable<Consultation>> GetConsultationsByPatientIdAsync(int patientId);
    Task<IEnumerable<Ordonnance>> GetOrdonnancesByPatientIdAsync(int patientId);
}

