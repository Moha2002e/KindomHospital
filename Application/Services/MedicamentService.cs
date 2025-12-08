using KingdomHospital.Application.DTOs;
using KingdomHospital.Application.Mappers;
using KingdomHospital.Application.Repositories;

namespace KingdomHospital.Application.Services;

public class MedicamentService
{
    private readonly IMedicamentRepository _repository;
    private readonly MedicamentMapper _mapper;
    private readonly OrdonnanceMapper _ordonnanceMapper;
    private readonly ILogger<MedicamentService> _logger;

    public MedicamentService(
        IMedicamentRepository repository,
        MedicamentMapper mapper,
        OrdonnanceMapper ordonnanceMapper,
        ILogger<MedicamentService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _ordonnanceMapper = ordonnanceMapper;
        _logger = logger;
    }

    public async Task<IEnumerable<MedicamentDto>> GetAllAsync()
    {
        _logger.LogInformation("Récupération de tous les médicaments");
        var medicaments = await _repository.GetAllAsync();
        return medicaments.Select(_mapper.ToDto);
    }

    public async Task<MedicamentDto?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Récupération du médicament {Id}", id);
        var medicament = await _repository.GetByIdAsync(id);
        return medicament == null ? null : _mapper.ToDto(medicament);
    }

    public async Task<MedicamentDto> CreateAsync(CreateMedicamentDto dto)
    {
        _logger.LogInformation("Création d'un nouveau médicament");
        
        var name = dto.Name?.Trim();
        var dosageForm = dto.DosageForm?.Trim();
        var strength = dto.Strength?.Trim();
        var atcCode = string.IsNullOrWhiteSpace(dto.AtcCode) ? null : dto.AtcCode.Trim();
        
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Le nom du médicament ne peut pas être vide.");
        }
        
        if (string.IsNullOrWhiteSpace(dosageForm))
        {
            throw new ArgumentException("La forme galénique ne peut pas être vide.");
        }
        
        if (string.IsNullOrWhiteSpace(strength))
        {
            throw new ArgumentException("La force ne peut pas être vide.");
        }
        
        var medicamentDto = new CreateMedicamentDto(name, dosageForm, strength, atcCode);
        var medicament = _mapper.ToEntity(medicamentDto);
        var created = await _repository.CreateAsync(medicament);
        return _mapper.ToDto(created);
    }

    public async Task<IEnumerable<OrdonnanceDto>> GetOrdonnancesByMedicamentIdAsync(int medicamentId)
    {
        _logger.LogInformation("Récupération des ordonnances contenant le médicament {MedicamentId}", medicamentId);
        var ordonnances = await _repository.GetOrdonnancesByMedicamentIdAsync(medicamentId);
        return ordonnances.Select(_ordonnanceMapper.ToDtoWithDetails);
    }
}

