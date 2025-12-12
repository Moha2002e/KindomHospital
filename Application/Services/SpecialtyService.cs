using KingdomHospital.Application.DTOs;
using KingdomHospital.Application.Mappers;
using KingdomHospital.Application.Repositories;
using KingdomHospital.Domain.Entities;

namespace KingdomHospital.Application.Services;




public class SpecialtyService
{
    private readonly ISpecialtyRepository _repository;
    private readonly SpecialtyMapper _mapper;
    private readonly DoctorMapper _doctorMapper;
    private readonly ILogger<SpecialtyService> _logger;

    
    
    
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

    
    
    
    public async Task<IEnumerable<SpecialtyDto>> GetAllAsync()
    {
        _logger.LogInformation("Récupération de toutes les spécialités");
        var specialties = await _repository.GetAllAsync();
        return specialties.Select(_mapper.ToDto);
    }

    
    
    
    
    
    public async Task<SpecialtyDto?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Récupération de la spécialité {Id}", id);
        var specialty = await _repository.GetByIdAsync(id);
        return specialty == null ? null : _mapper.ToDto(specialty);
    }

    
    
    
    
    
    public async Task<IEnumerable<DoctorDto>> GetDoctorsBySpecialtyIdAsync(int specialtyId)
    {
        _logger.LogInformation("Récupération des médecins de la spécialité {SpecialtyId}", specialtyId);
        var doctors = await _repository.GetDoctorsBySpecialtyIdAsync(specialtyId);
        return doctors.Select(_doctorMapper.ToDtoWithSpecialty);
    }
}

