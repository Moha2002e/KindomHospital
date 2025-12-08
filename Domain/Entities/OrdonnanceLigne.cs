namespace KingdomHospital.Domain.Entities;

/// <summary>
/// Représente une ligne d'ordonnance (un médicament dans une ordonnance)
/// </summary>
public class OrdonnanceLigne
{
    /// <summary>
    /// Identifiant unique de la ligne
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Identifiant de l'ordonnance
    /// </summary>
    public int OrdonnanceId { get; set; }

    /// <summary>
    /// Identifiant du médicament
    /// </summary>
    public int MedicamentId { get; set; }

    /// <summary>
    /// Dosage prescrit (ex: 500mg)
    /// </summary>
    public string Dosage { get; set; } = string.Empty;

    /// <summary>
    /// Fréquence de prise (ex: 2 fois par jour)
    /// </summary>
    public string Frequency { get; set; } = string.Empty;

    /// <summary>
    /// Durée du traitement (ex: 7 jours)
    /// </summary>
    public string Duration { get; set; } = string.Empty;

    /// <summary>
    /// Quantité de médicament prescrite
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Instructions supplémentaires (optionnel)
    /// </summary>
    public string? Instructions { get; set; }

    /// <summary>
    /// Ordonnance à laquelle appartient cette ligne
    /// </summary>
    public Ordonnance Ordonnance { get; set; } = null!;

    /// <summary>
    /// Médicament prescrit
    /// </summary>
    public Medicament Medicament { get; set; } = null!;
}

