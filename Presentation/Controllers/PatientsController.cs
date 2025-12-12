using KingdomHospital.Application.DTOs;
using KingdomHospital.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace KingdomHospital.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly PatientService _service;
    private readonly ILogger<PatientsController> _logger;

    public PatientsController(PatientService service, ILogger<PatientsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    
    
    
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PatientDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PatientDto>>> GetAll()
    {
        var patients = await _service.GetAllAsync();
        return Ok(patients);
    }

    
    
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PatientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PatientDto>> GetById(int id)
    {
        var patient = await _service.GetByIdAsync(id);
        if (patient == null)
            return NotFound();

        return Ok(patient);
    }

    
    
    
    [HttpPost]
    [ProducesResponseType(typeof(PatientDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PatientDto>> Create([FromBody] CreatePatientDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var patient = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = patient.Id }, patient);
    }

    
    
    
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(PatientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PatientDto>> Update(int id, [FromBody] UpdatePatientDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var patient = await _service.UpdateAsync(id, dto);
        if (patient == null)
            return NotFound();

        return Ok(patient);
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

    
    
    
    [HttpGet("{id}/consultations")]
    [ProducesResponseType(typeof(IEnumerable<ConsultationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ConsultationDto>>> GetConsultations(int id)
    {
        var patient = await _service.GetByIdAsync(id);
        if (patient == null)
            return NotFound();

        var consultations = await _service.GetConsultationsByPatientIdAsync(id);
        return Ok(consultations);
    }

    
    
    
    [HttpGet("{id}/ordonnances")]
    [ProducesResponseType(typeof(IEnumerable<OrdonnanceDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<OrdonnanceDto>>> GetOrdonnances(int id)
    {
        var patient = await _service.GetByIdAsync(id);
        if (patient == null)
            return NotFound();

        var ordonnances = await _service.GetOrdonnancesByPatientIdAsync(id);
        return Ok(ordonnances);
    }
}

