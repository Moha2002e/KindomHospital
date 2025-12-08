using KingdomHospital.Application.DTOs;
using KingdomHospital.Application.Mappers;
using KingdomHospital.Application.Repositories;
using KingdomHospital.Domain.Entities;

namespace KingdomHospital.Application.Services;

/// <summary>
/// Service pour gérer les spécialités médicales
/// </summary>
public class SpecialtyService
{
    private readonly ISpecialtyRepository _repository;
    private readonly SpecialtyMapper _mapper;
    private readonly DoctorMapper _doctorMapper;
    private readonly ILogger<SpecialtyService> _logger;

    /// <summary>
    /// Initialise une nouvelle instance du service
    /// </summary>
    public SpecialtyService(
        ISpecialtyRepository repository,
        SpecialtyMapper mapper,
        DoctorMapper doctorMapper,
        ILogger<SpecialtyService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _doctorMapper = doctorMapper;
        _logger = logger;
    }

    /// <summary>
    /// Récupère toutes les spécialités
    /// </summary>
    public async Task<IEnumerable<SpecialtyDto>> GetAllAsync()
    {
        _logger.LogInformation("Récupération de toutes les spécialités");
        var specialties = await _repository.GetAllAsync();
        return specialties.Select(_mapper.ToDto);
    }

    /// <summary>
    /// Récupère une spécialité par son identifiant
    /// </summary>
    /// <param name="id">Identifiant de la spécialité</param>
    /// <returns>La spécialité ou null si non trouvée</returns>
    public async Task<SpecialtyDto?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Récupération de la spécialité {Id}", id);
        var specialty = await _repository.GetByIdAsync(id);
        return specialty == null ? null : _mapper.ToDto(specialty);
    }

    /// <summary>
    /// Récupère tous les médecins d'une spécialité
    /// </summary>
    /// <param name="specialtyId">Identifiant de la spécialité</param>
    /// <returns>Liste des médecins de la spécialité</returns>
    public async Task<IEnumerable<DoctorDto>> GetDoctorsBySpecialtyIdAsync(int specialtyId)
    {
        _logger.LogInformation("Récupération des médecins de la spécialité {SpecialtyId}", specialtyId);
        var doctors = await _repository.GetDoctorsBySpecialtyIdAsync(specialtyId);
        return doctors.Select(_doctorMapper.ToDtoWithSpecialty);
    }
}

