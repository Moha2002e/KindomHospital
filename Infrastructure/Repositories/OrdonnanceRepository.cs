using KingdomHospital.Application.Repositories;
using KingdomHospital.Domain.Entities;
using KingdomHospital.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace KingdomHospital.Infrastructure.Repositories;

public class OrdonnanceRepository : IOrdonnanceRepository
{
    private readonly KingdomHospitalDbContext _context;

    public OrdonnanceRepository(KingdomHospitalDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Ordonnance>> GetAllAsync()
    {
        return await _context.Ordonnances
            .Include(o => o.Doctor)
            .Include(o => o.Patient)
            .Include(o => o.Consultation)
            .Include(o => o.OrdonnanceLignes)
                .ThenInclude(ol => ol.Medicament)
            .ToListAsync();
    }

    public async Task<Ordonnance?> GetByIdAsync(int id)
    {
        return await _context.Ordonnances
            .Include(o => o.Doctor)
            .Include(o => o.Patient)
            .Include(o => o.Consultation)
            .Include(o => o.OrdonnanceLignes)
                .ThenInclude(ol => ol.Medicament)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<Ordonnance> CreateAsync(Ordonnance ordonnance)
    {
        _context.Ordonnances.Add(ordonnance);
        await _context.SaveChangesAsync();
        
        return await _context.Ordonnances
            .Include(o => o.Doctor)
            .Include(o => o.Patient)
            .Include(o => o.Consultation)
            .Include(o => o.OrdonnanceLignes)
                .ThenInclude(ol => ol.Medicament)
            .FirstOrDefaultAsync(o => o.Id == ordonnance.Id) ?? ordonnance;
    }

    public async Task<Ordonnance> UpdateAsync(Ordonnance ordonnance)
    {
        // Supprimer les anciennes lignes
        var existingLignes = await _context.OrdonnanceLignes
            .Where(ol => ol.OrdonnanceId == ordonnance.Id)
            .ToListAsync();
        _context.OrdonnanceLignes.RemoveRange(existingLignes);

        // Mettre à jour l'ordonnance et ajouter les nouvelles lignes
        _context.Ordonnances.Update(ordonnance);
        await _context.SaveChangesAsync();
        
        return await _context.Ordonnances
            .Include(o => o.Doctor)
            .Include(o => o.Patient)
            .Include(o => o.Consultation)
            .Include(o => o.OrdonnanceLignes)
                .ThenInclude(ol => ol.Medicament)
            .FirstOrDefaultAsync(o => o.Id == ordonnance.Id) ?? ordonnance;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var ordonnance = await _context.Ordonnances.FindAsync(id);
        if (ordonnance == null)
            return false;

        _context.Ordonnances.Remove(ordonnance);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Ordonnances.AnyAsync(o => o.Id == id);
    }

    public async Task<IEnumerable<Ordonnance>> GetByDoctorIdAsync(int doctorId, DateOnly? from, DateOnly? to)
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

    public async Task<IEnumerable<Ordonnance>> GetByPatientIdAsync(int patientId)
    {
        return await _context.Ordonnances
            .Where(o => o.PatientId == patientId)
            .Include(o => o.Doctor)
            .Include(o => o.Patient)
            .Include(o => o.Consultation)
            .Include(o => o.OrdonnanceLignes)
                .ThenInclude(ol => ol.Medicament)
            .OrderBy(o => o.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Ordonnance>> GetByConsultationIdAsync(int consultationId)
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

    public async Task<IEnumerable<Ordonnance>> GetFilteredAsync(int? doctorId, int? patientId, DateOnly? from, DateOnly? to)
    {
        var query = _context.Ordonnances
            .Include(o => o.Doctor)
            .Include(o => o.Patient)
            .Include(o => o.Consultation)
            .Include(o => o.OrdonnanceLignes)
                .ThenInclude(ol => ol.Medicament)
            .AsQueryable();

        if (doctorId.HasValue)
        {
            query = query.Where(o => o.DoctorId == doctorId.Value);
        }

        if (patientId.HasValue)
        {
            query = query.Where(o => o.PatientId == patientId.Value);
        }

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

    public async Task<IEnumerable<OrdonnanceLigne>> GetLignesByOrdonnanceIdAsync(int ordonnanceId)
    {
        return await _context.OrdonnanceLignes
            .Where(ol => ol.OrdonnanceId == ordonnanceId)
            .Include(ol => ol.Medicament)
            .Include(ol => ol.Ordonnance)
            .ToListAsync();
    }

    public async Task<OrdonnanceLigne?> GetLigneByIdAsync(int ordonnanceId, int ligneId)
    {
        return await _context.OrdonnanceLignes
            .Where(ol => ol.OrdonnanceId == ordonnanceId && ol.Id == ligneId)
            .Include(ol => ol.Medicament)
            .Include(ol => ol.Ordonnance)
            .FirstOrDefaultAsync();
    }
}

