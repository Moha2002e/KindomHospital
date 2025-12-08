namespace KingdomHospital.Domain.Entities;

/// <summary>
/// Représente une consultation entre un médecin et un patient
/// </summary>
public class Consultation
{
    /// <summary>
    /// Identifiant unique de la consultation
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Identifiant du médecin
    /// </summary>
    public int DoctorId { get; set; }

    /// <summary>
    /// Identifiant du patient
    /// </summary>
    public int PatientId { get; set; }

    /// <summary>
    /// Date de la consultation
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Heure de la consultation
    /// </summary>
    public TimeOnly Hour { get; set; }

    /// <summary>
    /// Raison de la consultation (optionnel)
    /// </summary>
    public string? Reason { get; set; }

    /// <summary>
    /// Médecin de la consultation
    /// </summary>
    public Doctor Doctor { get; set; } = null!;

    /// <summary>
    /// Patient de la consultation
    /// </summary>
    public Patient Patient { get; set; } = null!;

    /// <summary>
    /// Liste des ordonnances liées à cette consultation
    /// </summary>
    public ICollection<Ordonnance> Ordonnances { get; set; } = new List<Ordonnance>();
}

