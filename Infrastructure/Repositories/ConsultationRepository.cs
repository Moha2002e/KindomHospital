using KingdomHospital.Application.Repositories;
using KingdomHospital.Domain.Entities;
using KingdomHospital.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace KingdomHospital.Infrastructure.Repositories;

public class ConsultationRepository : IConsultationRepository
{
    private readonly KingdomHospitalDbContext _context;

    public ConsultationRepository(KingdomHospitalDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Consultation>> GetAllAsync()
    {
        return await _context.Consultations
            .Include(c => c.Doctor)
            .Include(c => c.Patient)
            .ToListAsync();
    }

    public async Task<Consultation?> GetByIdAsync(int id)
    {
        return await _context.Consultations
            .Include(c => c.Doctor)
            .Include(c => c.Patient)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Consultation> CreateAsync(Consultation consultation)
    {
        _context.Consultations.Add(consultation);
        await _context.SaveChangesAsync();
        
        return await _context.Consultations
            .Include(c => c.Doctor)
            .Include(c => c.Patient)
            .FirstOrDefaultAsync(c => c.Id == consultation.Id) ?? consultation;
    }

    public async Task<Consultation> UpdateAsync(Consultation consultation)
    {
        _context.Consultations.Update(consultation);
        await _context.SaveChangesAsync();
        
        return await _context.Consultations
            .Include(c => c.Doctor)
            .Include(c => c.Patient)
            .FirstOrDefaultAsync(c => c.Id == consultation.Id) ?? consultation;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var consultation = await _context.Consultations.FindAsync(id);
        if (consultation == null)
            return false;

        _context.Consultations.Remove(consultation);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Consultations.AnyAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Consultation>> GetByDoctorIdAsync(int doctorId, DateOnly? from, DateOnly? to)
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

    public async Task<IEnumerable<Consultation>> GetByPatientIdAsync(int patientId)
    {
        return await _context.Consultations
            .Where(c => c.PatientId == patientId)
            .Include(c => c.Doctor)
            .Include(c => c.Patient)
            .OrderBy(c => c.Date)
            .ThenBy(c => c.Hour)
            .ToListAsync();
    }

    public async Task<IEnumerable<Consultation>> GetFilteredAsync(int? doctorId, int? patientId, DateOnly? from, DateOnly? to)
    {
        var query = _context.Consultations
            .Include(c => c.Doctor)
            .Include(c => c.Patient)
            .AsQueryable();

        if (doctorId.HasValue)
        {
            query = query.Where(c => c.DoctorId == doctorId.Value);
        }

        if (patientId.HasValue)
        {
            query = query.Where(c => c.PatientId == patientId.Value);
        }

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

    public async Task<IEnumerable<Ordonnance>> GetOrdonnancesByConsultationIdAsync(int consultationId)
    {
        return await _context.Ordonnances
            .Where(o => o.ConsultationId == consultationId)
            .Include(o => o.Doctor)
            .Include(o => o.Patient)
            .Include(o => o.Consultation)
            .Include(o => o.OrdonnanceLignes)
                .ThenInclude(ol => ol.Medicament)
            .ToListAsync();
    }

    public async Task<bool> HasConflictAsync(int doctorId, int patientId, DateOnly date, TimeOnly hour, int? excludeId = null)
    {
        var query = _context.Consultations
            .Where(c => c.Date == date && c.Hour == hour && c.DoctorId == doctorId);

        if (excludeId.HasValue)
        {
            query = query.Where(c => c.Id != excludeId.Value);
        }

        return await query.AnyAsync();
    }
}

