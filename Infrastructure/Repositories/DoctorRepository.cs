using KingdomHospital.Application.Repositories;
using KingdomHospital.Domain.Entities;
using KingdomHospital.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace KingdomHospital.Infrastructure.Repositories;

public class DoctorRepository : IDoctorRepository
{
    private readonly KingdomHospitalDbContext _context;

    public DoctorRepository(KingdomHospitalDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Doctor>> GetAllAsync()
    {
        return await _context.Doctors
            .Include(d => d.Specialty)
            .ToListAsync();
    }

    public async Task<Doctor?> GetByIdAsync(int id)
    {
        return await _context.Doctors
            .Include(d => d.Specialty)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<Doctor> CreateAsync(Doctor doctor)
    {
        _context.Doctors.Add(doctor);
        await _context.SaveChangesAsync();
        return doctor;
    }

    public async Task<Doctor> UpdateAsync(Doctor doctor)
    {
        _context.Doctors.Update(doctor);
        await _context.SaveChangesAsync();
        return doctor;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Doctors.AnyAsync(d => d.Id == id);
    }

    public async Task<IEnumerable<Doctor>> GetBySpecialtyIdAsync(int specialtyId)
    {
        return await _context.Doctors
            .Where(d => d.SpecialtyId == specialtyId)
            .Include(d => d.Specialty)
            .ToListAsync();
    }

    public async Task<IEnumerable<Consultation>> GetConsultationsByDoctorIdAsync(int doctorId, DateOnly? from, DateOnly? to)
    {
        var query = _context.Consultations
            .Where(c => c.DoctorId == doctorId)
            .Include(c => c.Doctor)
            .Include(c => c.Patient)
            .AsQueryable();

        if (from.HasValue)
        {
            query = query.Where(c => c.Date >= from.Value);
        }

        if (to.HasValue)
        {
            query = query.Where(c => c.Date <= to.Value);
        }

        return await query.OrderBy(c => c.Date).ThenBy(c => c.Hour).ToListAsync();
    }

    public async Task<IEnumerable<Patient>> GetPatientsByDoctorIdAsync(int doctorId)
    {
        return await _context.Consultations
            .Where(c => c.DoctorId == doctorId)
            .Select(c => c.Patient)
            .Distinct()
            .ToListAsync();
    }

    public async Task<IEnumerable<Ordonnance>> GetOrdonnancesByDoctorIdAsync(int doctorId, DateOnly? from, DateOnly? to)
    {
        var query = _context.Ordonnances
            .Where(o => o.DoctorId == doctorId)
            .Include(o => o.Doctor)
            .Include(o => o.Patient)
            .Include(o => o.Consultation)
            .Include(o => o.OrdonnanceLignes)
                .ThenInclude(ol => ol.Medicament)
            .AsQueryable();

        if (from.HasValue)
        {
            query = query.Where(o => o.Date >= from.Value);
        }

        if (to.HasValue)
        {
            query = query.Where(o => o.Date <= to.Value);
        }

        return await query.OrderBy(o => o.Date).ToListAsync();
    }
}

