using KingdomHospital.Application.Repositories;
using KingdomHospital.Domain.Entities;
using KingdomHospital.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace KingdomHospital.Infrastructure.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly KingdomHospitalDbContext _context;

    public PatientRepository(KingdomHospitalDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Patient>> GetAllAsync()
    {
        return await _context.Patients.ToListAsync();
    }

    public async Task<Patient?> GetByIdAsync(int id)
    {
        return await _context.Patients.FindAsync(id);
    }

    public async Task<Patient> CreateAsync(Patient patient)
    {
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();
        return patient;
    }

    public async Task<Patient> UpdateAsync(Patient patient)
    {
        _context.Patients.Update(patient);
        await _context.SaveChangesAsync();
        return patient;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient == null)
            return false;

        _context.Patients.Remove(patient);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Patients.AnyAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Consultation>> GetConsultationsByPatientIdAsync(int patientId)
    {
        return await _context.Consultations
            .Where(c => c.PatientId == patientId)
            .Include(c => c.Doctor)
            .Include(c => c.Patient)
            .OrderBy(c => c.Date)
            .ThenBy(c => c.Hour)
            .ToListAsync();
    }

    public async Task<IEnumerable<Ordonnance>> GetOrdonnancesByPatientIdAsync(int patientId)
    {
        return await _context.Ordonnances
            .Where(o => o.PatientId == patientId)
            .Include(o => o.Doctor)
            .Include(o => o.Patient)
            .Include(o => o.Consultation)
            .OrderBy(o => o.Date)
            .ToListAsync();
    }
}

