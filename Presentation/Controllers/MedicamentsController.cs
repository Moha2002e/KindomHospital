using KingdomHospital.Application.DTOs;
using KingdomHospital.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace KingdomHospital.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicamentsController : ControllerBase
{
    private readonly MedicamentService _service;
    private readonly ILogger<MedicamentsController> _logger;

    public MedicamentsController(MedicamentService service, ILogger<MedicamentsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    
    
    
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MedicamentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MedicamentDto>>> GetAll()
    {
        var medicaments = await _service.GetAllAsync();
        return Ok(medicaments);
    }

    
    
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(MedicamentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MedicamentDto>> GetById(int id)
    {
        var medicament = await _service.GetByIdAsync(id);
        if (medicament == null)
            return NotFound();

        return Ok(medicament);
    }

    
    
    
    [HttpPost]
    [ProducesResponseType(typeof(MedicamentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MedicamentDto>> Create([FromBody] CreateMedicamentDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var medicament = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = medicament.Id }, medicament);
    }

    
    
    
    [HttpGet("{id}/ordonnances")]
    [ProducesResponseType(typeof(IEnumerable<OrdonnanceDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<OrdonnanceDto>>> GetOrdonnances(int id)
    {
        var medicament = await _service.GetByIdAsync(id);
        if (medicament == null)
            return NotFound();

        var ordonnances = await _service.GetOrdonnancesByMedicamentIdAsync(id);
        return Ok(ordonnances);
    }
}

