using KingdomHospital.Application.DTOs;
using KingdomHospital.Application.Mappers;
using KingdomHospital.Application.Repositories;

namespace KingdomHospital.Application.Services;

public class DoctorService
{
    private readonly IDoctorRepository _repository;
    private readonly ISpecialtyRepository _specialtyRepository;
    private readonly DoctorMapper _mapper;
    private readonly ConsultationMapper _consultationMapper;
    private readonly PatientMapper _patientMapper;
    private readonly OrdonnanceMapper _ordonnanceMapper;
    private readonly ILogger<DoctorService> _logger;

    public DoctorService(
        IDoctorRepository repository,
        ISpecialtyRepository specialtyRepository,
        DoctorMapper mapper,
        ConsultationMapper consultationMapper,
        PatientMapper patientMapper,
        OrdonnanceMapper ordonnanceMapper,
        ILogger<DoctorService> logger)
    {
        _repository = repository;
        _specialtyRepository = specialtyRepository;
        _mapper = mapper;
        _consultationMapper = consultationMapper;
        _patientMapper = patientMapper;
        _ordonnanceMapper = ordonnanceMapper;
        _logger = logger;
    }

    public async Task<IEnumerable<DoctorDto>> GetAllAsync()
    {
        _logger.LogInformation("Récupération de tous les médecins");
        var doctors = await _repository.GetAllAsync();
        return doctors.Select(_mapper.ToDtoWithSpecialty);
    }

    public async Task<DoctorDto?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Récupération du médecin {Id}", id);
        var doctor = await _repository.GetByIdAsync(id);
        return doctor == null ? null : _mapper.ToDtoWithSpecialty(doctor);
    }

    public async Task<DoctorDto> CreateAsync(CreateDoctorDto dto)
    {
        _logger.LogInformation("Création d'un nouveau médecin");
        
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
        var specialty = await _specialtyRepository.GetByIdAsync(dto.SpecialtyId);
        if (specialty == null)
        {
            throw new ArgumentException($"La spécialité avec l'ID {dto.SpecialtyId} n'existe pas.");
        }

        var doctorDto = new CreateDoctorDto(dto.SpecialtyId, lastName, firstName);
        var doctor = _mapper.ToEntity(doctorDto);
        var created = await _repository.CreateAsync(doctor);
        return _mapper.ToDtoWithSpecialty(created);
    }

    public async Task<DoctorDto?> UpdateAsync(int id, UpdateDoctorDto dto)
    {
        _logger.LogInformation("Mise à jour du médecin {Id}", id);
        
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
        var specialty = await _specialtyRepository.GetByIdAsync(dto.SpecialtyId);
        if (specialty == null)
        {
            throw new ArgumentException($"La spécialité avec l'ID {dto.SpecialtyId} n'existe pas.");
        }

        var updateDto = new UpdateDoctorDto(dto.SpecialtyId, lastName, firstName);
        var updated = _mapper.ToEntity(updateDto, existing);
        var result = await _repository.UpdateAsync(updated);
        return _mapper.ToDtoWithSpecialty(result);
    }

    public async Task<IEnumerable<DoctorDto>> GetBySpecialtyIdAsync(int specialtyId)
    {
        _logger.LogInformation("Récupération des médecins de la spécialité {SpecialtyId}", specialtyId);
        var doctors = await _repository.GetBySpecialtyIdAsync(specialtyId);
        return doctors.Select(_mapper.ToDtoWithSpecialty);
    }

    public async Task<IEnumerable<ConsultationDto>> GetConsultationsByDoctorIdAsync(int doctorId, DateOnly? from, DateOnly? to)
    {
        _logger.LogInformation("Récupération des consultations du médecin {DoctorId}", doctorId);
        var consultations = await _repository.GetConsultationsByDoctorIdAsync(doctorId, from, to);
        return consultations.Select(_consultationMapper.ToDtoWithNames);
    }

    public async Task<IEnumerable<PatientDto>> GetPatientsByDoctorIdAsync(int doctorId)
    {
        _logger.LogInformation("Récupération des patients du médecin {DoctorId}", doctorId);
        var patients = await _repository.GetPatientsByDoctorIdAsync(doctorId);
        return patients.Select(_patientMapper.ToDto);
    }

    public async Task<IEnumerable<OrdonnanceDto>> GetOrdonnancesByDoctorIdAsync(int doctorId, DateOnly? from, DateOnly? to)
    {
        _logger.LogInformation("Récupération des ordonnances du médecin {DoctorId}", doctorId);
        var ordonnances = await _repository.GetOrdonnancesByDoctorIdAsync(doctorId, from, to);
        return ordonnances.Select(_ordonnanceMapper.ToDtoWithDetails);
    }
}

