using Microsoft.AspNetCore.Mvc;

namespace KingdomHospital.Presentation.Controllers;

/// <summary>
/// Contrôleur pour la page d'accueil de l'API
/// </summary>
[ApiController]
[Route("/")]
public class HomeController : ControllerBase
{
    /// <summary>
    /// Point d'entrée de l'API Kingdom Hospital
    /// </summary>
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

