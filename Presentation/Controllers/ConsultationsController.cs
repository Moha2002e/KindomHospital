using KingdomHospital.Application.DTOs;
using KingdomHospital.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace KingdomHospital.Presentation.Controllers;

/// <summary>
/// Contrôleur pour gérer les consultations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ConsultationsController : ControllerBase
{
    private readonly ConsultationService _service;
    private readonly ILogger<ConsultationsController> _logger;

    /// <summary>
    /// Initialise une nouvelle instance du contrôleur
    /// </summary>
    public ConsultationsController(ConsultationService service, ILogger<ConsultationsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Liste toutes les consultations (avec filtres optionnels)
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ConsultationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ConsultationDto>>> GetAll([FromQuery] int? doctorId, [FromQuery] int? patientId, [FromQuery] DateOnly? from, [FromQuery] DateOnly? to)
    {
        if (doctorId.HasValue || patientId.HasValue || from.HasValue || to.HasValue)
        {
            var consultations = await _service.GetFilteredAsync(doctorId, patientId, from, to);
            return Ok(consultations);
        }

        var allConsultations = await _service.GetAllAsync();
        return Ok(allConsultations);
    }

    /// <summary>
    /// Détail d'une consultation
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ConsultationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ConsultationDto>> GetById(int id)
    {
        var consultation = await _service.GetByIdAsync(id);
        if (consultation == null)
            return NotFound();

        return Ok(consultation);
    }

    /// <summary>
    /// Crée une consultation
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ConsultationDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ConsultationDto>> Create([FromBody] CreateConsultationDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var consultation = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = consultation.Id }, consultation);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Met à jour une consultation
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ConsultationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ConsultationDto>> Update(int id, [FromBody] UpdateConsultationDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var consultation = await _service.UpdateAsync(id, dto);
            if (consultation == null)
                return NotFound();

            return Ok(consultation);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Supprime une consultation
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }

    /// <summary>
    /// Liste les ordonnances liées à une consultation
    /// </summary>
    [HttpGet("{id}/ordonnances")]
    [ProducesResponseType(typeof(IEnumerable<OrdonnanceDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<OrdonnanceDto>>> GetOrdonnances(int id)
    {
        var consultation = await _service.GetByIdAsync(id);
        if (consultation == null)
            return NotFound();

        var ordonnances = await _service.GetOrdonnancesByConsultationIdAsync(id);
        return Ok(ordonnances);
    }

    /// <summary>
    /// Crée une ordonnance rattachée à une consultation
    /// </summary>
    [HttpPost("{id}/ordonnances")]
    [ProducesResponseType(typeof(OrdonnanceDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrdonnanceDto>> CreateOrdonnance(int id, [FromBody] CreateOrdonnanceDto dto)
    {
        var consultation = await _service.GetByIdAsync(id);
        if (consultation == null)
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var ordonnanceDto = dto with { ConsultationId = id };
        
        var ordonnanceService = HttpContext.RequestServices.GetRequiredService<OrdonnanceService>();
        try
        {
            var ordonnance = await ordonnanceService.CreateAsync(ordonnanceDto);
            return CreatedAtAction("GetById", "Ordonnances", new { id = ordonnance.Id }, ordonnance);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

