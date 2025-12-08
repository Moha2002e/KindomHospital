using KingdomHospital.Domain.Entities;

namespace KingdomHospital.Application.Repositories;

public interface IMedicamentRepository
{
    Task<IEnumerable<Medicament>> GetAllAsync();
    Task<Medicament?> GetByIdAsync(int id);
    Task<Medicament> CreateAsync(Medicament medicament);
    Task<bool> ExistsAsync(int id);
    Task<IEnumerable<Ordonnance>> GetOrdonnancesByMedicamentIdAsync(int medicamentId);
}

