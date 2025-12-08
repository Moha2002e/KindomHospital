namespace KingdomHospital.Domain.Entities;

/// <summary>
/// Représente un médecin
/// </summary>
public class Doctor
{
    /// <summary>
    /// Identifiant unique du médecin
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Identifiant de la spécialité du médecin
    /// </summary>
    public int SpecialtyId { get; set; }

    /// <summary>
    /// Nom de famille du médecin
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Prénom du médecin
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Spécialité du médecin
    /// </summary>
    public Specialty Specialty { get; set; } = null!;

    /// <summary>
    /// Liste des consultations du médecin
    /// </summary>
    public ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();

    /// <summary>
    /// Liste des ordonnances émises par le médecin
    /// </summary>
    public ICollection<Ordonnance> Ordonnances { get; set; } = new List<Ordonnance>();
}

