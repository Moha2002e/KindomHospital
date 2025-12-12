namespace KingdomHospital.Domain.Entities;




public class Doctor
{
    
    
    
    public int Id { get; set; }

    
    
    
    public int SpecialtyId { get; set; }

    
    
    
    public string LastName { get; set; } = string.Empty;

    
    
    
    public string FirstName { get; set; } = string.Empty;

    
    
    
    public Specialty Specialty { get; set; } = null!;

    
    
    
    public ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();

    
    
    
    public ICollection<Ordonnance> Ordonnances { get; set; } = new List<Ordonnance>();
}

