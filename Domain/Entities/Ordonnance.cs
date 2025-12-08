namespace KingdomHospital.Domain.Entities;

/// <summary>
/// Représente une ordonnance médicale
/// </summary>
public class Ordonnance
{
    /// <summary>
    /// Identifiant unique de l'ordonnance
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Identifiant du médecin prescripteur
    /// </summary>
    public int DoctorId { get; set; }

    /// <summary>
    /// Identifiant du patient
    /// </summary>
    public int PatientId { get; set; }

    /// <summary>
    /// Identifiant de la consultation associée (optionnel)
    /// </summary>
    public int? ConsultationId { get; set; }

    /// <summary>
    /// Date de l'ordonnance
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Notes supplémentaires (optionnel)
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Médecin prescripteur
    /// </summary>
    public Doctor Doctor { get; set; } = null!;

    /// <summary>
    /// Patient destinataire
    /// </summary>
    public Patient Patient { get; set; } = null!;

    /// <summary>
    /// Consultation associée (peut être null)
    /// </summary>
    public Consultation? Consultation { get; set; }

    /// <summary>
    /// Liste des lignes de médicaments de l'ordonnance
    /// </summary>
    public ICollection<OrdonnanceLigne> OrdonnanceLignes { get; set; } = new List<OrdonnanceLigne>();
}

