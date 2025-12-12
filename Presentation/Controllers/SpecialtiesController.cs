using KingdomHospital.Application.DTOs;
using KingdomHospital.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace KingdomHospital.Presentation.Controllers;




[ApiController]
[Route("api/[controller]")]
public class SpecialtiesController : ControllerBase
{
    private readonly SpecialtyService _service;
    private readonly ILogger<SpecialtiesController> _logger;

    
    
    
    public SpecialtiesController(SpecialtyService service, ILogger<SpecialtiesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    
    
    
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SpecialtyDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SpecialtyDto>>> GetAll()
    {
        var specialties = await _service.GetAllAsync();
        return Ok(specialties);
    }

    
    
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(SpecialtyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SpecialtyDto>> GetById(int id)
    {
        var specialty = await _service.GetByIdAsync(id);
        if (specialty == null)
            return NotFound();

        return Ok(specialty);
    }

    
    
    
    [HttpGet("{id}/doctors")]
    [ProducesResponseType(typeof(IEnumerable<DoctorDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<DoctorDto>>> GetDoctorsBySpecialty(int id)
    {
        var specialty = await _service.GetByIdAsync(id);
        if (specialty == null)
            return NotFound();

        var doctors = await _service.GetDoctorsBySpecialtyIdAsync(id);
        return Ok(doctors);
    }
}

