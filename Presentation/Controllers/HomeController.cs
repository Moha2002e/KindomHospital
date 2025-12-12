using Microsoft.AspNetCore.Mvc;

namespace KingdomHospital.Presentation.Controllers;




[ApiController]
[Route("/")]
public class HomeController : ControllerBase
{
    
    
    
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            message = "Bienvenue sur l'API Kingdom Hospital",
            version = "1.0",
            endpoints = new
            {
                doctors = "/api/doctors",
                patients = "/api/patients",
                consultations = "/api/consultations",
                medicaments = "/api/medicaments",
                ordonnances = "/api/ordonnances",
                specialties = "/api/specialties",
                openApi = "/openapi/v1.json"
            }
        });
    }
}

