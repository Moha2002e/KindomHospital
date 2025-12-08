using KingdomHospital.Application.Repositories;
using KingdomHospital.Domain.Entities;
using KingdomHospital.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace KingdomHospital.Infrastructure.Repositories;

public class MedicamentRepository : IMedicamentRepository
{
    private readonly KingdomHospitalDbContext _context;

    public MedicamentRepository(KingdomHospitalDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Medicament>> GetAllAsync()
    {
        return await _context.Medicaments.ToListAsync();
    }

    public async Task<Medicament?> GetByIdAsync(int id)
    {
        return await _context.Medicaments.FindAsync(id);
    }

    public async Task<Medicament> CreateAsync(Medicament medicament)
    {
        _context.Medicaments.Add(medicament);
        await _context.SaveChangesAsync();
        return medicament;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Medicaments.AnyAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Ordonnance>> GetOrdonnancesByMedicamentIdAsync(int medicamentId)
    {
        return await _context.OrdonnanceLignes
            .Where(ol => ol.MedicamentId == medicamentId)
            .Select(ol => ol.Ordonnance)
            .Distinct()
            .Include(o => o.Doctor)
            .Include(o => o.Patient)
            .Include(o => o.Consultation)
            .ToListAsync();
    }
}

