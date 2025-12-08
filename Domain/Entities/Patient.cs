namespace KingdomHospital.Domain.Entities;

/// <summary>
/// Représente un patient
/// </summary>
public class Patient
{
    /// <summary>
    /// Identifiant unique du patient
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nom de famille du patient
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Prénom du patient
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Date de naissance du patient
    /// </summary>
    public DateOnly BirthDate { get; set; }

    /// <summary>
    /// Liste des consultations du patient
    /// </summary>
    public ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();

    /// <summary>
    /// Liste des ordonnances du patient
    /// </summary>
    public ICollection<Ordonnance> Ordonnances { get; set; } = new List<Ordonnance>();
}

