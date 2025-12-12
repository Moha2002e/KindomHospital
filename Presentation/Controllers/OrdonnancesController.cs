using KingdomHospital.Application.DTOs;
using KingdomHospital.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace KingdomHospital.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdonnancesController : ControllerBase
{
    private readonly OrdonnanceService _service;
    private readonly ILogger<OrdonnancesController> _logger;

    public OrdonnancesController(OrdonnanceService service, ILogger<OrdonnancesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    
    
    
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OrdonnanceDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<OrdonnanceDto>>> GetAll([FromQuery] int? doctorId, [FromQuery] int? patientId, [FromQuery] DateOnly? from, [FromQuery] DateOnly? to)
    {
        if (doctorId.HasValue || patientId.HasValue || from.HasValue || to.HasValue)
        {
            var ordonnances = await _service.GetFilteredAsync(doctorId, patientId, from, to);
            return Ok(ordonnances);
        }

        var allOrdonnances = await _service.GetAllAsync();
        return Ok(allOrdonnances);
    }

    
    
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(OrdonnanceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrdonnanceDto>> GetById(int id)
    {
        var ordonnance = await _service.GetByIdAsync(id);
        if (ordonnance == null)
            return NotFound();

        return Ok(ordonnance);
    }

    
    
    
    [HttpPost]
    [ProducesResponseType(typeof(OrdonnanceDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrdonnanceDto>> Create([FromBody] CreateOrdonnanceDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var ordonnance = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = ordonnance.Id }, ordonnance);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    
    
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(OrdonnanceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrdonnanceDto>> Update(int id, [FromBody] UpdateOrdonnanceDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var ordonnance = await _service.UpdateAsync(id, dto);
            if (ordonnance == null)
                return NotFound();

            return Ok(ordonnance);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    
    
    
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

    
    
    
    [HttpPut("{id}/consultation/{consultationId}")]
    [ProducesResponseType(typeof(OrdonnanceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrdonnanceDto>> AttachToConsultation(int id, int consultationId)
    {
        var ordonnance = await _service.GetByIdAsync(id);
        if (ordonnance == null)
            return NotFound();

        var consultationService = HttpContext.RequestServices.GetRequiredService<ConsultationService>();
        var consultation = await consultationService.GetByIdAsync(consultationId);
        if (consultation == null)
            return NotFound();

        var updateDto = new UpdateOrdonnanceDto(
            ordonnance.DoctorId,
            ordonnance.PatientId,
            consultationId,
            ordonnance.Date,
            ordonnance.Notes,
            ordonnance.Lignes.Select(l => new CreateOrdonnanceLigneDto(
                l.MedicamentId,
                l.Dosage,
                l.Frequency,
                l.Duration,
                l.Quantity,
                l.Instructions
            )).ToList()
        );

        try
        {
            var updated = await _service.UpdateAsync(id, updateDto);
            return Ok(updated);
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

    
    
    
    [HttpDelete("{id}/consultation")]
    [ProducesResponseType(typeof(OrdonnanceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrdonnanceDto>> DetachFromConsultation(int id)
    {
        var ordonnance = await _service.GetByIdAsync(id);
        if (ordonnance == null)
            return NotFound();

        var updateDto = new UpdateOrdonnanceDto(
            ordonnance.DoctorId,
            ordonnance.PatientId,
            null,
            ordonnance.Date,
            ordonnance.Notes,
            ordonnance.Lignes.Select(l => new CreateOrdonnanceLigneDto(
                l.MedicamentId,
                l.Dosage,
                l.Frequency,
                l.Duration,
                l.Quantity,
                l.Instructions
            )).ToList()
        );

        try
        {
            var updated = await _service.UpdateAsync(id, updateDto);
            return Ok(updated);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    
    
    
    [HttpGet("{id}/lignes")]
    [ProducesResponseType(typeof(IEnumerable<OrdonnanceLigneDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<OrdonnanceLigneDto>>> GetLignes(int id)
    {
        var ordonnance = await _service.GetByIdAsync(id);
        if (ordonnance == null)
            return NotFound();

        var lignes = await _service.GetLignesAsync(id);
        return Ok(lignes);
    }

    
    
    
    [HttpGet("{id}/lignes/{ligneId}")]
    [ProducesResponseType(typeof(OrdonnanceLigneDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrdonnanceLigneDto>> GetLigne(int id, int ligneId)
    {
        var ordonnance = await _service.GetByIdAsync(id);
        if (ordonnance == null)
            return NotFound();

        var ligne = await _service.GetLigneByIdAsync(id, ligneId);
        if (ligne == null)
            return NotFound();

        return Ok(ligne);
    }

    
    
    
    [HttpPost("{id}/lignes")]
    [ProducesResponseType(typeof(OrdonnanceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrdonnanceDto>> AddLignes(int id, [FromBody] List<CreateOrdonnanceLigneDto> lignes)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var ordonnance = await _service.GetByIdAsync(id);
        if (ordonnance == null)
            return NotFound();

        var allLignes = ordonnance.Lignes.Select(l => new CreateOrdonnanceLigneDto(
            l.MedicamentId,
            l.Dosage,
            l.Frequency,
            l.Duration,
            l.Quantity,
            l.Instructions
        )).ToList();

        allLignes.AddRange(lignes);

        var updateDto = new UpdateOrdonnanceDto(
            ordonnance.DoctorId,
            ordonnance.PatientId,
            ordonnance.ConsultationId,
            ordonnance.Date,
            ordonnance.Notes,
            allLignes
        );

        try
        {
            var updated = await _service.UpdateAsync(id, updateDto);
            return Ok(updated);
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

    
    
    
    [HttpPut("{id}/lignes/{ligneId}")]
    [ProducesResponseType(typeof(OrdonnanceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrdonnanceDto>> UpdateLigne(int id, int ligneId, [FromBody] CreateOrdonnanceLigneDto ligneDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var ordonnance = await _service.GetByIdAsync(id);
        if (ordonnance == null)
            return NotFound();

        var ligne = await _service.GetLigneByIdAsync(id, ligneId);
        if (ligne == null)
            return NotFound();

        var allLignes = ordonnance.Lignes.Select((l, index) =>
        {
            if (l.Id == ligneId)
            {
                return ligneDto;
            }
            return new CreateOrdonnanceLigneDto(
                l.MedicamentId,
                l.Dosage,
                l.Frequency,
                l.Duration,
                l.Quantity,
                l.Instructions
            );
        }).ToList();

        var updateDto = new UpdateOrdonnanceDto(
            ordonnance.DoctorId,
            ordonnance.PatientId,
            ordonnance.ConsultationId,
            ordonnance.Date,
            ordonnance.Notes,
            allLignes
        );

        try
        {
            var updated = await _service.UpdateAsync(id, updateDto);
            return Ok(updated);
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

    
    
    
    [HttpDelete("{id}/lignes/{ligneId}")]
    [ProducesResponseType(typeof(OrdonnanceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrdonnanceDto>> DeleteLigne(int id, int ligneId)
    {
        var ordonnance = await _service.GetByIdAsync(id);
        if (ordonnance == null)
            return NotFound();

        var ligne = await _service.GetLigneByIdAsync(id, ligneId);
        if (ligne == null)
            return NotFound();

        if (ordonnance.Lignes.Count <= 1)
        {
            return BadRequest("Une ordonnance doit contenir au moins une ligne de médicament.");
        }

        var allLignes = ordonnance.Lignes
            .Where(l => l.Id != ligneId)
            .Select(l => new CreateOrdonnanceLigneDto(
                l.MedicamentId,
                l.Dosage,
                l.Frequency,
                l.Duration,
                l.Quantity,
                l.Instructions
            )).ToList();

        var updateDto = new UpdateOrdonnanceDto(
            ordonnance.DoctorId,
            ordonnance.PatientId,
            ordonnance.ConsultationId,
            ordonnance.Date,
            ordonnance.Notes,
            allLignes
        );

        try
        {
            var updated = await _service.UpdateAsync(id, updateDto);
            return Ok(updated);
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

