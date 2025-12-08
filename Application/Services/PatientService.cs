using KingdomHospital.Application.DTOs;
using KingdomHospital.Application.Mappers;
using KingdomHospital.Application.Repositories;

namespace KingdomHospital.Application.Services;

public class PatientService
{
    private readonly IPatientRepository _repository;
    private readonly PatientMapper _mapper;
    private readonly ConsultationMapper _consultationMapper;
    private readonly OrdonnanceMapper _ordonnanceMapper;
    private readonly ILogger<PatientService> _logger;

    public PatientService(
        IPatientRepository repository,
        PatientMapper mapper,
        ConsultationMapper consultationMapper,
        OrdonnanceMapper ordonnanceMapper,
        ILogger<PatientService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _consultationMapper = consultationMapper;
        _ordonnanceMapper = ordonnanceMapper;
        _logger = logger;
    }

    public async Task<IEnumerable<PatientDto>> GetAllAsync()
    {
        _logger.LogInformation("Récupération de tous les patients");
        var patients = await _repository.GetAllAsync();
        return patients.Select(_mapper.ToDto);
    }

    public async Task<PatientDto?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Récupération du patient {Id}", id);
        var patient = await _repository.GetByIdAsync(id);
        return patient == null ? null : _mapper.ToDto(patient);
    }

    public async Task<PatientDto> CreateAsync(CreatePatientDto dto)
    {
        _logger.LogInformation("Création d'un nouveau patient");
        
        var lastName = dto.LastName?.Trim();
        var firstName = dto.FirstName?.Trim();
        
        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("Le nom de famille ne peut pas être vide.");
        }
        
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new ArgumentException("Le prénom ne peut pas être vide.");
        }
        var today = DateOnly.FromDateTime(DateTime.Today);
        if (dto.BirthDate > today)
        {
            throw new ArgumentException("La date de naissance ne peut pas être dans le futur.");
        }
        
        var maxAge = 150;
        var minDate = today.AddYears(-maxAge);
        if (dto.BirthDate < minDate)
        {
            throw new ArgumentException($"La date de naissance ne peut pas être antérieure à {minDate:yyyy-MM-dd}.");
        }
        
        var patientDto = new CreatePatientDto(lastName, firstName, dto.BirthDate);
        var patient = _mapper.ToEntity(patientDto);
        var created = await _repository.CreateAsync(patient);
        return _mapper.ToDto(created);
    }

    public async Task<PatientDto?> UpdateAsync(int id, UpdatePatientDto dto)
    {
        _logger.LogInformation("Mise à jour du patient {Id}", id);
        
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
            return null;

        var lastName = dto.LastName?.Trim();
        var firstName = dto.FirstName?.Trim();
        
        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("Le nom de famille ne peut pas être vide.");
        }
        
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new ArgumentException("Le prénom ne peut pas être vide.");
        }
        var today = DateOnly.FromDateTime(DateTime.Today);
        if (dto.BirthDate > today)
        {
            throw new ArgumentException("La date de naissance ne peut pas être dans le futur.");
        }
        
        var maxAge = 150;
        var minDate = today.AddYears(-maxAge);
        if (dto.BirthDate < minDate)
        {
            throw new ArgumentException($"La date de naissance ne peut pas être antérieure à {minDate:yyyy-MM-dd}.");
        }
        
        var updateDto = new UpdatePatientDto(lastName, firstName, dto.BirthDate);
        var updated = _mapper.ToEntity(updateDto, existing);
        var result = await _repository.UpdateAsync(updated);
        return _mapper.ToDto(result);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        _logger.LogInformation("Suppression du patient {Id}", id);
        return await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<ConsultationDto>> GetConsultationsByPatientIdAsync(int patientId)
    {
        _logger.LogInformation("Récupération des consultations du patient {PatientId}", patientId);
        var consultations = await _repository.GetConsultationsByPatientIdAsync(patientId);
        return consultations.Select(_consultationMapper.ToDtoWithNames);
    }

    public async Task<IEnumerable<OrdonnanceDto>> GetOrdonnancesByPatientIdAsync(int patientId)
    {
        _logger.LogInformation("Récupération des ordonnances du patient {PatientId}", patientId);
        var ordonnances = await _repository.GetOrdonnancesByPatientIdAsync(patientId);
        return ordonnances.Select(_ordonnanceMapper.ToDtoWithDetails);
    }
}

