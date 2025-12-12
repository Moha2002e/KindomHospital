using KingdomHospital.Application.DTOs;
using KingdomHospital.Application.Mappers;
using KingdomHospital.Application.Repositories;

namespace KingdomHospital.Application.Services;




public class ConsultationService
{
    private readonly IConsultationRepository _repository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly ConsultationMapper _mapper;
    private readonly OrdonnanceMapper _ordonnanceMapper;
    private readonly ILogger<ConsultationService> _logger;

    
    
    
    public ConsultationService(
        IConsultationRepository repository,
        IDoctorRepository doctorRepository,
        IPatientRepository patientRepository,
        ConsultationMapper mapper,
        OrdonnanceMapper ordonnanceMapper,
        ILogger<ConsultationService> logger)
    {
        _repository = repository;
        _doctorRepository = doctorRepository;
        _patientRepository = patientRepository;
        _mapper = mapper;
        _ordonnanceMapper = ordonnanceMapper;
        _logger = logger;
    }

    
    
    
    public async Task<IEnumerable<ConsultationDto>> GetAllAsync()
    {
        _logger.LogInformation("Récupération de toutes les consultations");
        var consultations = await _repository.GetAllAsync();
        return consultations.Select(_mapper.ToDtoWithNames);
    }

    
    
    
    
    
    public async Task<ConsultationDto?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Récupération de la consultation {Id}", id);
        var consultation = await _repository.GetByIdAsync(id);
        return consultation == null ? null : _mapper.ToDtoWithNames(consultation);
    }

    
    
    
    
    
    
    
    public async Task<ConsultationDto> CreateAsync(CreateConsultationDto dto)
    {
        _logger.LogInformation("Création d'une nouvelle consultation");
        
        var reason = string.IsNullOrWhiteSpace(dto.Reason) ? null : dto.Reason.Trim();
        
        var today = DateOnly.FromDateTime(DateTime.Today);
        var maxPastDays = 365;
        var maxFutureDays = 365;
        
        if (dto.Date < today.AddDays(-maxPastDays))
        {
            throw new ArgumentException($"La date de consultation ne peut pas être antérieure à {today.AddDays(-maxPastDays):yyyy-MM-dd}.");
        }
        
        if (dto.Date > today.AddDays(maxFutureDays))
        {
            throw new ArgumentException($"La date de consultation ne peut pas être postérieure à {today.AddDays(maxFutureDays):yyyy-MM-dd}.");
        }
        
        if (!await _doctorRepository.ExistsAsync(dto.DoctorId))
        {
            throw new ArgumentException($"Le médecin avec l'ID {dto.DoctorId} n'existe pas.");
        }

        if (!await _patientRepository.ExistsAsync(dto.PatientId))
        {
            throw new ArgumentException($"Le patient avec l'ID {dto.PatientId} n'existe pas.");
        }
        if (await _repository.HasConflictAsync(dto.DoctorId, dto.PatientId, dto.Date, dto.Hour))
        {
            throw new InvalidOperationException($"Une consultation existe déjà pour ce médecin à la date {dto.Date} et l'heure {dto.Hour}.");
        }

        var consultationDto = new CreateConsultationDto(dto.DoctorId, dto.PatientId, dto.Date, dto.Hour, reason);
        var consultation = _mapper.ToEntity(consultationDto);
        var created = await _repository.CreateAsync(consultation);
        return _mapper.ToDtoWithNames(created);
    }

    
    
    
    
    
    
    
    
    public async Task<ConsultationDto?> UpdateAsync(int id, UpdateConsultationDto dto)
    {
        _logger.LogInformation("Mise à jour de la consultation {Id}", id);
        
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
            return null;

        var reason = string.IsNullOrWhiteSpace(dto.Reason) ? null : dto.Reason.Trim();
        
        var today = DateOnly.FromDateTime(DateTime.Today);
        var maxPastDays = 365;
        var maxFutureDays = 365;
        
        if (dto.Date < today.AddDays(-maxPastDays))
        {
            throw new ArgumentException($"La date de consultation ne peut pas être antérieure à {today.AddDays(-maxPastDays):yyyy-MM-dd}.");
        }
        
        if (dto.Date > today.AddDays(maxFutureDays))
        {
            throw new ArgumentException($"La date de consultation ne peut pas être postérieure à {today.AddDays(maxFutureDays):yyyy-MM-dd}.");
        }

        if (!await _doctorRepository.ExistsAsync(dto.DoctorId))
        {
            throw new ArgumentException($"Le médecin avec l'ID {dto.DoctorId} n'existe pas.");
        }

        if (!await _patientRepository.ExistsAsync(dto.PatientId))
        {
            throw new ArgumentException($"Le patient avec l'ID {dto.PatientId} n'existe pas.");
        }
        if (await _repository.HasConflictAsync(dto.DoctorId, dto.PatientId, dto.Date, dto.Hour, id))
        {
            throw new InvalidOperationException($"Une consultation existe déjà pour ce médecin à la date {dto.Date} et l'heure {dto.Hour}.");
        }

        var updateDto = new UpdateConsultationDto(dto.DoctorId, dto.PatientId, dto.Date, dto.Hour, reason);
        var updated = _mapper.ToEntity(updateDto, existing);
        var result = await _repository.UpdateAsync(updated);
        return _mapper.ToDtoWithNames(result);
    }

    
    
    
    
    
    public async Task<bool> DeleteAsync(int id)
    {
        _logger.LogInformation("Suppression de la consultation {Id}", id);
        return await _repository.DeleteAsync(id);
    }

    
    
    
    
    
    
    
    public async Task<IEnumerable<ConsultationDto>> GetByDoctorIdAsync(int doctorId, DateOnly? from, DateOnly? to)
    {
        _logger.LogInformation("Récupération des consultations du médecin {DoctorId}", doctorId);
        var consultations = await _repository.GetByDoctorIdAsync(doctorId, from, to);
        return consultations.Select(_mapper.ToDtoWithNames);
    }

    
    
    
    
    
    public async Task<IEnumerable<ConsultationDto>> GetByPatientIdAsync(int patientId)
    {
        _logger.LogInformation("Récupération des consultations du patient {PatientId}", patientId);
        var consultations = await _repository.GetByPatientIdAsync(patientId);
        return consultations.Select(_mapper.ToDtoWithNames);
    }

    
    
    
    
    
    
    
    
    public async Task<IEnumerable<ConsultationDto>> GetFilteredAsync(int? doctorId, int? patientId, DateOnly? from, DateOnly? to)
    {
        _logger.LogInformation("Récupération des consultations filtrées");
        var consultations = await _repository.GetFilteredAsync(doctorId, patientId, from, to);
        return consultations.Select(_mapper.ToDtoWithNames);
    }

    
    
    
    
    
    public async Task<IEnumerable<OrdonnanceDto>> GetOrdonnancesByConsultationIdAsync(int consultationId)
    {
        _logger.LogInformation("Récupération des ordonnances de la consultation {ConsultationId}", consultationId);
        var ordonnances = await _repository.GetOrdonnancesByConsultationIdAsync(consultationId);
        return ordonnances.Select(_ordonnanceMapper.ToDtoWithDetails);
    }
}

