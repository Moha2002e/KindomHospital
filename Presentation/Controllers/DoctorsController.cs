using KingdomHospital.Application.DTOs;
using KingdomHospital.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace KingdomHospital.Presentation.Controllers;

/// <summary>
/// Contrôleur pour gérer les médecins
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly DoctorService _service;
    private readonly ILogger<DoctorsController> _logger;

    /// <summary>
    /// Initialise une nouvelle instance du contrôleur
    /// </summary>
    public DoctorsController(DoctorService service, ILogger<DoctorsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Liste tous les médecins
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DoctorDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DoctorDto>>> GetAll()
    {
        var doctors = await _service.GetAllAsync();
        return Ok(doctors);
    }

    /// <summary>
    /// Détail d'un médecin
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(DoctorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DoctorDto>> GetById(int id)
    {
        var doctor = await _service.GetByIdAsync(id);
        if (doctor == null)
            return NotFound();

        return Ok(doctor);
    }

    /// <summary>
    /// Crée un médecin
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(DoctorDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DoctorDto>> Create([FromBody] CreateDoctorDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var doctor = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = doctor.Id }, doctor);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Met à jour un médecin
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(DoctorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DoctorDto>> Update(int id, [FromBody] UpdateDoctorDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var doctor = await _service.UpdateAsync(id, dto);
            if (doctor == null)
                return NotFound();

            return Ok(doctor);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Renvoie la spécialité d'un médecin
    /// </summary>
    [HttpGet("{id}/specialty")]
    [ProducesResponseType(typeof(SpecialtyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SpecialtyDto>> GetSpecialty(int id)
    {
        var doctor = await _service.GetByIdAsync(id);
        if (doctor == null)
            return NotFound();

        var specialtyService = HttpContext.RequestServices.GetRequiredService<SpecialtyService>();
        var specialty = await specialtyService.GetByIdAsync(doctor.SpecialtyId);
        if (specialty == null)
            return NotFound();

        return Ok(specialty);
    }

    /// <summary>
    /// Change la spécialité d'un médecin
    /// </summary>
    [HttpPut("{id}/specialty/{specialtyId}")]
    [ProducesResponseType(typeof(DoctorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DoctorDto>> ChangeSpecialty(int id, int specialtyId)
    {
        var existing = await _service.GetByIdAsync(id);
        if (existing == null)
            return NotFound();

        var specialtyService = HttpContext.RequestServices.GetRequiredService<SpecialtyService>();
        var specialty = await specialtyService.GetByIdAsync(specialtyId);
        if (specialty == null)
            return BadRequest($"La spécialité avec l'ID {specialtyId} n'existe pas.");

        var updateDto = new UpdateDoctorDto(specialtyId, existing.LastName, existing.FirstName);
        try
        {
            var doctor = await _service.UpdateAsync(id, updateDto);
            return Ok(doctor);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Liste les consultations d'un médecin
    /// </summary>
    [HttpGet("{id}/consultations")]
    [ProducesResponseType(typeof(IEnumerable<ConsultationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ConsultationDto>>> GetConsultations(int id, [FromQuery] DateOnly? from, [FromQuery] DateOnly? to)
    {
        var doctor = await _service.GetByIdAsync(id);
        if (doctor == null)
            return NotFound();

        var consultations = await _service.GetConsultationsByDoctorIdAsync(id, from, to);
        return Ok(consultations);
    }

    /// <summary>
    /// Liste les patients consultés par un médecin
    /// </summary>
    [HttpGet("{id}/patients")]
    [ProducesResponseType(typeof(IEnumerable<PatientDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<PatientDto>>> GetPatients(int id)
    {
        var doctor = await _service.GetByIdAsync(id);
        if (doctor == null)
            return NotFound();

        var patients = await _service.GetPatientsByDoctorIdAsync(id);
        return Ok(patients);
    }

    /// <summary>
    /// Liste les ordonnances émises par un médecin
    /// </summary>
    [HttpGet("{id}/ordonnances")]
    [ProducesResponseType(typeof(IEnumerable<OrdonnanceDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<OrdonnanceDto>>> GetOrdonnances(int id, [FromQuery] DateOnly? from, [FromQuery] DateOnly? to)
    {
        var doctor = await _service.GetByIdAsync(id);
        if (doctor == null)
            return NotFound();

        var ordonnances = await _service.GetOrdonnancesByDoctorIdAsync(id, from, to);
        return Ok(ordonnances);
    }
}

