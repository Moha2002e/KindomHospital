using KingdomHospital.Application.Repositories;
using KingdomHospital.Domain.Entities;
using KingdomHospital.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace KingdomHospital.Infrastructure.Repositories;

public class SpecialtyRepository : ISpecialtyRepository
{
    private readonly KingdomHospitalDbContext _context;

    public SpecialtyRepository(KingdomHospitalDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Specialty>> GetAllAsync()
    {
        return await _context.Specialties.ToListAsync();
    }

    public async Task<Specialty?> GetByIdAsync(int id)
    {
        return await _context.Specialties.FindAsync(id);
    }

    public async Task<IEnumerable<Doctor>> GetDoctorsBySpecialtyIdAsync(int specialtyId)
    {
        return await _context.Doctors
            .Where(d => d.SpecialtyId == specialtyId)
            .Include(d => d.Specialty)
            .ToListAsync();
    }
}

