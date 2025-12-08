namespace KingdomHospital.Domain.Entities;

/// <summary>
/// Représente un médicament
/// </summary>
public class Medicament
{
    /// <summary>
    /// Identifiant unique du médicament
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nom du médicament
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Forme galénique (ex: Comprimé, Gélule, Sirop)
    /// </summary>
    public string DosageForm { get; set; } = string.Empty;

    /// <summary>
    /// Dosage (ex: 500mg, 250mg)
    /// </summary>
    public string Strength { get; set; } = string.Empty;

    /// <summary>
    /// Code ATC du médicament (optionnel)
    /// </summary>
    public string? AtcCode { get; set; }

    /// <summary>
    /// Liste des lignes d'ordonnance contenant ce médicament
    /// </summary>
    public ICollection<OrdonnanceLigne> OrdonnanceLignes { get; set; } = new List<OrdonnanceLigne>();
}

