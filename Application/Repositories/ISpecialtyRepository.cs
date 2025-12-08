using KingdomHospital.Domain.Entities;

namespace KingdomHospital.Application.Repositories;

public interface ISpecialtyRepository
{
    Task<IEnumerable<Specialty>> GetAllAsync();
    Task<Specialty?> GetByIdAsync(int id);
    Task<IEnumerable<Doctor>> GetDoctorsBySpecialtyIdAsync(int specialtyId);
}

