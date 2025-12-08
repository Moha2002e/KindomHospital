namespace KingdomHospital.Domain.Entities;

/// <summary>
/// Représente une spécialité médicale (ex: Cardiologie, Neurologie)
/// </summary>
public class Specialty
{
    /// <summary>
    /// Identifiant unique de la spécialité
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nom de la spécialité
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Liste des médecins ayant cette spécialité
    /// </summary>
    public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
}

