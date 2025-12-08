using KingdomHospital.Domain.Entities;

namespace KingdomHospital.Application.Repositories;

public interface IDoctorRepository
{
    Task<IEnumerable<Doctor>> GetAllAsync();
    Task<Doctor?> GetByIdAsync(int id);
    Task<Doctor> CreateAsync(Doctor doctor);
    Task<Doctor> UpdateAsync(Doctor doctor);
    Task<bool> ExistsAsync(int id);
    Task<IEnumerable<Doctor>> GetBySpecialtyIdAsync(int specialtyId);
    Task<IEnumerable<Consultation>> GetConsultationsByDoctorIdAsync(int doctorId, DateOnly? from, DateOnly? to);
    Task<IEnumerable<Patient>> GetPatientsByDoctorIdAsync(int doctorId);
    Task<IEnumerable<Ordonnance>> GetOrdonnancesByDoctorIdAsync(int doctorId, DateOnly? from, DateOnly? to);
}

