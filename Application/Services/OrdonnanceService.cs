using KingdomHospital.Application.DTOs;
using KingdomHospital.Application.Mappers;
using KingdomHospital.Application.Repositories;
using KingdomHospital.Domain.Entities;

namespace KingdomHospital.Application.Services;

public class OrdonnanceService
{
    private readonly IOrdonnanceRepository _repository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IConsultationRepository _consultationRepository;
    private readonly IMedicamentRepository _medicamentRepository;
    private readonly OrdonnanceMapper _mapper;
    private readonly OrdonnanceLigneMapper _ligneMapper;
    private readonly ILogger<OrdonnanceService> _logger;

    public OrdonnanceService(
        IOrdonnanceRepository repository,
        IDoctorRepository doctorRepository,
        IPatientRepository patientRepository,
        IConsultationRepository consultationRepository,
        IMedicamentRepository medicamentRepository,
        OrdonnanceMapper mapper,
        OrdonnanceLigneMapper ligneMapper,
        ILogger<OrdonnanceService> logger)
    {
        _repository = repository;
        _doctorRepository = doctorRepository;
        _patientRepository = patientRepository;
        _consultationRepository = consultationRepository;
        _medicamentRepository = medicamentRepository;
        _mapper = mapper;
        _ligneMapper = ligneMapper;
        _logger = logger;
    }

    public async Task<IEnumerable<OrdonnanceDto>> GetAllAsync()
    {
        _logger.LogInformation("Récupération de toutes les ordonnances");
        var ordonnances = await _repository.GetAllAsync();
        return ordonnances.Select(_mapper.ToDtoWithDetails);
    }

    public async Task<OrdonnanceDto?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Récupération de l'ordonnance {Id}", id);
        var ordonnance = await _repository.GetByIdAsync(id);
        return ordonnance == null ? null : _mapper.ToDtoWithDetails(ordonnance);
    }

    public async Task<OrdonnanceDto> CreateAsync(CreateOrdonnanceDto dto)
    {
        _logger.LogInformation("Création d'une nouvelle ordonnance");
        
        var notes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes.Trim();
        
        var today = DateOnly.FromDateTime(DateTime.Today);
        var maxPastDays = 365;
        var maxFutureDays = 365;
        
        if (dto.Date < today.AddDays(-maxPastDays))
        {
            throw new ArgumentException($"La date de l'ordonnance ne peut pas être antérieure à {today.AddDays(-maxPastDays):yyyy-MM-dd}.");
        }
        
        if (dto.Date > today.AddDays(maxFutureDays))
        {
            throw new ArgumentException($"La date de l'ordonnance ne peut pas être postérieure à {today.AddDays(maxFutureDays):yyyy-MM-dd}.");
        }
        
        // Vérifier que le médecin existe
        if (!await _doctorRepository.ExistsAsync(dto.DoctorId))
        {
            throw new ArgumentException($"Le médecin avec l'ID {dto.DoctorId} n'existe pas.");
        }

        // Vérifier que le patient existe
        if (!await _patientRepository.ExistsAsync(dto.PatientId))
        {
            throw new ArgumentException($"Le patient avec l'ID {dto.PatientId} n'existe pas.");
        }

        // Vérifier la consultation si fournie
        if (dto.ConsultationId.HasValue && !await _consultationRepository.ExistsAsync(dto.ConsultationId.Value))
        {
            throw new ArgumentException($"La consultation avec l'ID {dto.ConsultationId} n'existe pas.");
        }

        // Règle métier : Une ordonnance doit avoir au moins 1 ligne
        if (dto.Lignes == null || !dto.Lignes.Any())
        {
            throw new ArgumentException("Une ordonnance doit contenir au moins une ligne de médicament.");
        }

        // Vérifier que tous les médicaments existent et valider les lignes
        var validatedLignes = new List<CreateOrdonnanceLigneDto>();
        foreach (var ligne in dto.Lignes)
        {
            if (!await _medicamentRepository.ExistsAsync(ligne.MedicamentId))
            {
                throw new ArgumentException($"Le médicament avec l'ID {ligne.MedicamentId} n'existe pas.");
            }
            
            var dosage = ligne.Dosage?.Trim();
            var frequency = ligne.Frequency?.Trim();
            var duration = ligne.Duration?.Trim();
            var instructions = string.IsNullOrWhiteSpace(ligne.Instructions) ? null : ligne.Instructions.Trim();
            
            if (string.IsNullOrWhiteSpace(dosage))
            {
                throw new ArgumentException("Le dosage ne peut pas être vide.");
            }
            
            if (string.IsNullOrWhiteSpace(frequency))
            {
                throw new ArgumentException("La fréquence ne peut pas être vide.");
            }
            
            if (string.IsNullOrWhiteSpace(duration))
            {
                throw new ArgumentException("La durée ne peut pas être vide.");
            }
            
            validatedLignes.Add(new CreateOrdonnanceLigneDto(
                ligne.MedicamentId,
                dosage,
                frequency,
                duration,
                ligne.Quantity,
                instructions
            ));
        }

        if (dto.ConsultationId.HasValue)
        {
            var consultation = await _consultationRepository.GetByIdAsync(dto.ConsultationId.Value);
            if (consultation == null)
            {
                throw new ArgumentException($"La consultation avec l'ID {dto.ConsultationId} n'existe pas.");
            }

            if (consultation.DoctorId != dto.DoctorId)
            {
                throw new InvalidOperationException("Le médecin de l'ordonnance doit correspondre au médecin de la consultation.");
            }

            if (consultation.PatientId != dto.PatientId)
            {
                throw new InvalidOperationException("Le patient de l'ordonnance doit correspondre au patient de la consultation.");
            }

            if (dto.Date < consultation.Date)
            {
                throw new InvalidOperationException($"La date de l'ordonnance ({dto.Date}) doit être supérieure ou égale à la date de la consultation ({consultation.Date}).");
            }
        }

        var ordonnance = new Ordonnance
        {
            DoctorId = dto.DoctorId,
            PatientId = dto.PatientId,
            ConsultationId = dto.ConsultationId,
            Date = dto.Date,
            Notes = notes,
            OrdonnanceLignes = validatedLignes.Select(l => _ligneMapper.ToEntity(l)).ToList()
        };

        var created = await _repository.CreateAsync(ordonnance);
        return _mapper.ToDtoWithDetails(created);
    }

    public async Task<OrdonnanceDto?> UpdateAsync(int id, UpdateOrdonnanceDto dto)
    {
        _logger.LogInformation("Mise à jour de l'ordonnance {Id}", id);
        
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
            return null;

        var notes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes.Trim();
        
        var today = DateOnly.FromDateTime(DateTime.Today);
        var maxPastDays = 365;
        var maxFutureDays = 365;
        
        if (dto.Date < today.AddDays(-maxPastDays))
        {
            throw new ArgumentException($"La date de l'ordonnance ne peut pas être antérieure à {today.AddDays(-maxPastDays):yyyy-MM-dd}.");
        }
        
        if (dto.Date > today.AddDays(maxFutureDays))
        {
            throw new ArgumentException($"La date de l'ordonnance ne peut pas être postérieure à {today.AddDays(maxFutureDays):yyyy-MM-dd}.");
        }

        // Vérifier que le médecin existe
        if (!await _doctorRepository.ExistsAsync(dto.DoctorId))
        {
            throw new ArgumentException($"Le médecin avec l'ID {dto.DoctorId} n'existe pas.");
        }

        // Vérifier que le patient existe
        if (!await _patientRepository.ExistsAsync(dto.PatientId))
        {
            throw new ArgumentException($"Le patient avec l'ID {dto.PatientId} n'existe pas.");
        }

        // Vérifier la consultation si fournie
        if (dto.ConsultationId.HasValue && !await _consultationRepository.ExistsAsync(dto.ConsultationId.Value))
        {
            throw new ArgumentException($"La consultation avec l'ID {dto.ConsultationId} n'existe pas.");
        }

        // Règle métier : Une ordonnance doit avoir au moins 1 ligne
        if (dto.Lignes == null || !dto.Lignes.Any())
        {
            throw new ArgumentException("Une ordonnance doit contenir au moins une ligne de médicament.");
        }

        // Vérifier que tous les médicaments existent et valider les lignes
        var validatedLignes = new List<CreateOrdonnanceLigneDto>();
        foreach (var ligne in dto.Lignes)
        {
            if (!await _medicamentRepository.ExistsAsync(ligne.MedicamentId))
            {
                throw new ArgumentException($"Le médicament avec l'ID {ligne.MedicamentId} n'existe pas.");
            }
            
            var dosage = ligne.Dosage?.Trim();
            var frequency = ligne.Frequency?.Trim();
            var duration = ligne.Duration?.Trim();
            var instructions = string.IsNullOrWhiteSpace(ligne.Instructions) ? null : ligne.Instructions.Trim();
            
            if (string.IsNullOrWhiteSpace(dosage))
            {
                throw new ArgumentException("Le dosage ne peut pas être vide.");
            }
            
            if (string.IsNullOrWhiteSpace(frequency))
            {
                throw new ArgumentException("La fréquence ne peut pas être vide.");
            }
            
            if (string.IsNullOrWhiteSpace(duration))
            {
                throw new ArgumentException("La durée ne peut pas être vide.");
            }
            
            validatedLignes.Add(new CreateOrdonnanceLigneDto(
                ligne.MedicamentId,
                dosage,
                frequency,
                duration,
                ligne.Quantity,
                instructions
            ));
        }

        if (dto.ConsultationId.HasValue)
        {
            var consultation = await _consultationRepository.GetByIdAsync(dto.ConsultationId.Value);
            if (consultation == null)
            {
                throw new ArgumentException($"La consultation avec l'ID {dto.ConsultationId} n'existe pas.");
            }

            if (consultation.DoctorId != dto.DoctorId)
            {
                throw new InvalidOperationException("Le médecin de l'ordonnance doit correspondre au médecin de la consultation.");
            }

            if (consultation.PatientId != dto.PatientId)
            {
                throw new InvalidOperationException("Le patient de l'ordonnance doit correspondre au patient de la consultation.");
            }

            if (dto.Date < consultation.Date)
            {
                throw new InvalidOperationException($"La date de l'ordonnance ({dto.Date}) doit être supérieure ou égale à la date de la consultation ({consultation.Date}).");
            }
        }

        existing.DoctorId = dto.DoctorId;
        existing.PatientId = dto.PatientId;
        existing.ConsultationId = dto.ConsultationId;
        existing.Date = dto.Date;
        existing.Notes = notes;
        existing.OrdonnanceLignes = validatedLignes.Select(l => _ligneMapper.ToEntity(l)).ToList();

        var updated = await _repository.UpdateAsync(existing);
        return _mapper.ToDtoWithDetails(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        _logger.LogInformation("Suppression de l'ordonnance {Id}", id);
        return await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<OrdonnanceDto>> GetByDoctorIdAsync(int doctorId, DateOnly? from, DateOnly? to)
    {
        _logger.LogInformation("Récupération des ordonnances du médecin {DoctorId}", doctorId);
        var ordonnances = await _repository.GetByDoctorIdAsync(doctorId, from, to);
        return ordonnances.Select(_mapper.ToDtoWithDetails);
    }

    public async Task<IEnumerable<OrdonnanceDto>> GetByPatientIdAsync(int patientId)
    {
        _logger.LogInformation("Récupération des ordonnances du patient {PatientId}", patientId);
        var ordonnances = await _repository.GetByPatientIdAsync(patientId);
        return ordonnances.Select(_mapper.ToDtoWithDetails);
    }

    public async Task<IEnumerable<OrdonnanceDto>> GetByConsultationIdAsync(int consultationId)
    {
        _logger.LogInformation("Récupération des ordonnances de la consultation {ConsultationId}", consultationId);
        var ordonnances = await _repository.GetByConsultationIdAsync(consultationId);
        return ordonnances.Select(_mapper.ToDtoWithDetails);
    }

    public async Task<IEnumerable<OrdonnanceDto>> GetFilteredAsync(int? doctorId, int? patientId, DateOnly? from, DateOnly? to)
    {
        _logger.LogInformation("Récupération des ordonnances filtrées");
        var ordonnances = await _repository.GetFilteredAsync(doctorId, patientId, from, to);
        return ordonnances.Select(_mapper.ToDtoWithDetails);
    }

    public async Task<IEnumerable<OrdonnanceLigneDto>> GetLignesAsync(int ordonnanceId)
    {
        _logger.LogInformation("Récupération des lignes de l'ordonnance {OrdonnanceId}", ordonnanceId);
        var lignes = await _repository.GetLignesByOrdonnanceIdAsync(ordonnanceId);
        return lignes.Select(_ligneMapper.ToDtoWithMedicament);
    }

    public async Task<OrdonnanceLigneDto?> GetLigneByIdAsync(int ordonnanceId, int ligneId)
    {
        _logger.LogInformation("Récupération de la ligne {LigneId} de l'ordonnance {OrdonnanceId}", ligneId, ordonnanceId);
        var ligne = await _repository.GetLigneByIdAsync(ordonnanceId, ligneId);
        return ligne == null ? null : _ligneMapper.ToDtoWithMedicament(ligne);
    }
}

