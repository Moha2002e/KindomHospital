using System.Reflection;
using KingdomHospital.Domain.Entities;
using KingdomHospital.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace KingdomHospital.Infrastructure;

/// <summary>
/// Contexte Entity Framework Core pour la base de données KingdomHospital
/// </summary>
public class KingdomHospitalDbContext : DbContext
{
    /// <summary>
    /// Initialise une nouvelle instance du contexte
    /// </summary>
    public KingdomHospitalDbContext(DbContextOptions<KingdomHospitalDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Table des spécialités médicales
    /// </summary>
    public DbSet<Specialty> Specialties { get; set; }

    /// <summary>
    /// Table des médecins
    /// </summary>
    public DbSet<Doctor> Doctors { get; set; }

    /// <summary>
    /// Table des patients
    /// </summary>
    public DbSet<Patient> Patients { get; set; }

    /// <summary>
    /// Table des consultations
    /// </summary>
    public DbSet<Consultation> Consultations { get; set; }

    /// <summary>
    /// Table des médicaments
    /// </summary>
    public DbSet<Medicament> Medicaments { get; set; }

    /// <summary>
    /// Table des ordonnances
    /// </summary>
    public DbSet<Ordonnance> Ordonnances { get; set; }

    /// <summary>
    /// Table des lignes d'ordonnance
    /// </summary>
    public DbSet<OrdonnanceLigne> OrdonnanceLignes { get; set; }

    /// <summary>
    /// Configure le modèle de données avec les configurations des entités
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new SpecialtyConfiguration());
        modelBuilder.ApplyConfiguration(new DoctorConfiguration());
        modelBuilder.ApplyConfiguration(new PatientConfiguration());
        modelBuilder.ApplyConfiguration(new ConsultationConfiguration());
        modelBuilder.ApplyConfiguration(new MedicamentConfiguration());
        modelBuilder.ApplyConfiguration(new OrdonnanceConfiguration());
        modelBuilder.ApplyConfiguration(new OrdonnanceLigneConfiguration());
    }

    /// <summary>
    /// Remplit la base de données avec des données initiales depuis les fichiers CSV
    /// </summary>
    /// <param name="context">Le contexte de base de données</param>
    public static void Seed(KingdomHospitalDbContext context)
    {
        if (context.Specialties.Any() && context.Consultations.Any())
        {
            return;
        }
        
        if (context.Specialties.Any() && !context.Consultations.Any())
        {
            var existingDoctors = context.Doctors.ToList();
            var existingPatients = context.Patients.ToList();
            
            if (existingDoctors.Count == 0 || existingPatients.Count == 0)
            {
                return;
            }
            
            var seedDate = DateOnly.FromDateTime(DateTime.Now);
            var newConsultations = new List<Consultation>
            {
                new Consultation { DoctorId = existingDoctors[0].Id, PatientId = existingPatients[0].Id, Date = seedDate.AddDays(-10), Hour = new TimeOnly(9, 0), Reason = "Contrôle cardiaque" },
                new Consultation { DoctorId = existingDoctors[1].Id, PatientId = existingPatients[0].Id, Date = seedDate.AddDays(-5), Hour = new TimeOnly(14, 30), Reason = "Examen de la peau" },
                new Consultation { DoctorId = existingDoctors[2].Id, PatientId = existingPatients[1].Id, Date = seedDate.AddDays(-15), Hour = new TimeOnly(10, 0), Reason = "Maux de tête" },
                new Consultation { DoctorId = existingDoctors[3].Id, PatientId = existingPatients[1].Id, Date = seedDate.AddDays(-8), Hour = new TimeOnly(11, 0), Reason = "Consultation pédiatrique" },
                new Consultation { DoctorId = existingDoctors[0].Id, PatientId = existingPatients[1].Id, Date = seedDate.AddDays(-3), Hour = new TimeOnly(15, 0), Reason = "Bilan cardiaque" },
                new Consultation { DoctorId = existingDoctors[4 % existingDoctors.Count].Id, PatientId = existingPatients[2].Id, Date = seedDate.AddDays(-12), Hour = new TimeOnly(9, 30), Reason = "Douleur au genou" },
                new Consultation { DoctorId = existingDoctors[5 % existingDoctors.Count].Id, PatientId = existingPatients[2].Id, Date = seedDate.AddDays(-7), Hour = new TimeOnly(16, 0), Reason = "Suivi orthopédique" },
                new Consultation { DoctorId = existingDoctors[1].Id, PatientId = existingPatients[3 % existingPatients.Count].Id, Date = seedDate.AddDays(-6), Hour = new TimeOnly(10, 30), Reason = "Problème cutané" },
                new Consultation { DoctorId = existingDoctors[0].Id, PatientId = existingPatients[4 % existingPatients.Count].Id, Date = seedDate.AddDays(-4), Hour = new TimeOnly(9, 0), Reason = "Première consultation" },
                new Consultation { DoctorId = existingDoctors[0].Id, PatientId = existingPatients[4 % existingPatients.Count].Id, Date = seedDate.AddDays(-4), Hour = new TimeOnly(14, 0), Reason = "Consultation de suivi" }
            };
            context.Consultations.AddRange(newConsultations);
            context.SaveChanges();
            return;
        }

        var basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Data");
        basePath = Path.GetFullPath(basePath);
        
        if (!Directory.Exists(basePath) || !File.Exists(Path.Combine(basePath, "specialties.csv")))
        {
            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var assemblyDir = Path.GetDirectoryName(assemblyLocation);
            if (assemblyDir != null)
            {
                basePath = Path.Combine(assemblyDir, "..", "..", "..", "..", "Data");
                basePath = Path.GetFullPath(basePath);
            }
        }
        
        if (!Directory.Exists(basePath) || !File.Exists(Path.Combine(basePath, "specialties.csv")))
        {
            var currentDir = Directory.GetCurrentDirectory();
            basePath = Path.Combine(currentDir, "Data");
            if (!Directory.Exists(basePath))
            {
                var projectDir = Path.Combine(currentDir, "..", "..", "..");
                basePath = Path.Combine(Path.GetFullPath(projectDir), "KindomHospital", "Data");
            }
        }

        // Charger les spécialités depuis le CSV
        var specialties = new List<Specialty>();
        var specialtiesPath = Path.Combine(basePath, "specialties.csv");
        if (File.Exists(specialtiesPath))
        {
            var lines = File.ReadAllLines(specialtiesPath);
            // Ignorer la première ligne (en-tête)
            for (int i = 1; i < lines.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(lines[i]))
                {
                    specialties.Add(new Specialty { Name = lines[i].Trim() });
                }
            }
        }
        else
        {
            // Données par défaut si le fichier n'existe pas
            specialties = new List<Specialty>
            {
                new Specialty { Name = "Cardiologie" },
                new Specialty { Name = "Dermatologie" },
                new Specialty { Name = "Neurologie" },
                new Specialty { Name = "Pédiatrie" },
                new Specialty { Name = "Orthopédie" },
                new Specialty { Name = "Ophtalmologie" }
            };
        }
        context.Specialties.AddRange(specialties);
        context.SaveChanges();

        // Charger les médicaments depuis le CSV
        var medicaments = new List<Medicament>();
        var medicamentsPath = Path.Combine(basePath, "medicaments.csv");
        if (File.Exists(medicamentsPath))
        {
            var lines = File.ReadAllLines(medicamentsPath);
            // Ignorer la première ligne (en-tête)
            for (int i = 1; i < lines.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(lines[i]))
                {
                    var parts = lines[i].Split(',');
                    if (parts.Length >= 3)
                    {
                        medicaments.Add(new Medicament
                        {
                            Name = parts[0].Trim(),
                            DosageForm = parts[1].Trim(),
                            Strength = parts[2].Trim(),
                            AtcCode = parts.Length > 3 ? parts[3].Trim() : null
                        });
                    }
                }
            }
        }
        else
        {
            // Données par défaut si le fichier n'existe pas
            medicaments = new List<Medicament>
            {
                new Medicament { Name = "Paracétamol", DosageForm = "Comprimé", Strength = "500mg", AtcCode = "N02BE01" },
                new Medicament { Name = "Ibuprofène", DosageForm = "Comprimé", Strength = "400mg", AtcCode = "M01AE01" },
                new Medicament { Name = "Amoxicilline", DosageForm = "Gélule", Strength = "500mg", AtcCode = "J01CA04" }
            };
        }
        context.Medicaments.AddRange(medicaments);
        context.SaveChanges();

        // Créer au moins 6 médecins répartis dans différentes spécialités
        var doctors = new List<Doctor>
        {
            new Doctor { SpecialtyId = specialties[0].Id, FirstName = "Jean", LastName = "Dupont" },      // Cardiologie
            new Doctor { SpecialtyId = specialties[1].Id, FirstName = "Marie", LastName = "Martin" },    // Dermatologie
            new Doctor { SpecialtyId = specialties[2].Id, FirstName = "Pierre", LastName = "Bernard" },  // Neurologie
            new Doctor { SpecialtyId = specialties[3].Id, FirstName = "Sophie", LastName = "Dubois" },  // Pédiatrie
            new Doctor { SpecialtyId = specialties[0].Id, FirstName = "Luc", LastName = "Moreau" },     // Cardiologie
            new Doctor { SpecialtyId = specialties[4].Id, FirstName = "Anne", LastName = "Lefebvre" }     // Orthopédie
        };
        context.Doctors.AddRange(doctors);
        context.SaveChanges();

        // Créer au moins 5 patients avec dates de naissance plausibles
        var patients = new List<Patient>
        {
            new Patient { FirstName = "Paul", LastName = "Durand", BirthDate = new DateOnly(1985, 5, 15) },
            new Patient { FirstName = "Julie", LastName = "Leroy", BirthDate = new DateOnly(1990, 8, 22) },
            new Patient { FirstName = "Thomas", LastName = "Petit", BirthDate = new DateOnly(1978, 3, 10) },
            new Patient { FirstName = "Emma", LastName = "Rousseau", BirthDate = new DateOnly(1995, 11, 5) },
            new Patient { FirstName = "Lucas", LastName = "Garcia", BirthDate = new DateOnly(1988, 7, 30) },
            new Patient { FirstName = "Léa", LastName = "Simon", BirthDate = new DateOnly(2000, 2, 14) } // Patient sans consultation
        };
        context.Patients.AddRange(patients);
        context.SaveChanges();

        // Créer au moins 10 consultations
        // Réparties entre patients (0 à 3 consultations par patient)
        // Au moins un patient sans consultation (Léa Simon)
        // Une journée avec 2 consultations du même médecin à heures différentes
        var today = DateOnly.FromDateTime(DateTime.Now);
        var consultations = new List<Consultation>
        {
            // Patient 1 (Paul) - 2 consultations
            new Consultation { DoctorId = doctors[0].Id, PatientId = patients[0].Id, Date = today.AddDays(-10), Hour = new TimeOnly(9, 0), Reason = "Contrôle cardiaque" },
            new Consultation { DoctorId = doctors[1].Id, PatientId = patients[0].Id, Date = today.AddDays(-5), Hour = new TimeOnly(14, 30), Reason = "Examen de la peau" },
            
            // Patient 2 (Julie) - 3 consultations
            new Consultation { DoctorId = doctors[2].Id, PatientId = patients[1].Id, Date = today.AddDays(-15), Hour = new TimeOnly(10, 0), Reason = "Maux de tête" },
            new Consultation { DoctorId = doctors[3].Id, PatientId = patients[1].Id, Date = today.AddDays(-8), Hour = new TimeOnly(11, 0), Reason = "Consultation pédiatrique" },
            new Consultation { DoctorId = doctors[0].Id, PatientId = patients[1].Id, Date = today.AddDays(-3), Hour = new TimeOnly(15, 0), Reason = "Bilan cardiaque" },
            
            // Patient 3 (Thomas) - 2 consultations
            new Consultation { DoctorId = doctors[4].Id, PatientId = patients[2].Id, Date = today.AddDays(-12), Hour = new TimeOnly(9, 30), Reason = "Douleur au genou" },
            new Consultation { DoctorId = doctors[5].Id, PatientId = patients[2].Id, Date = today.AddDays(-7), Hour = new TimeOnly(16, 0), Reason = "Suivi orthopédique" },
            
            // Patient 4 (Emma) - 1 consultation
            new Consultation { DoctorId = doctors[1].Id, PatientId = patients[3].Id, Date = today.AddDays(-6), Hour = new TimeOnly(10, 30), Reason = "Problème cutané" },
            
            // Patient 5 (Lucas) - 2 consultations
            // Même médecin (doctors[0]) le même jour à heures différentes
            new Consultation { DoctorId = doctors[0].Id, PatientId = patients[4].Id, Date = today.AddDays(-4), Hour = new TimeOnly(9, 0), Reason = "Première consultation" },
            new Consultation { DoctorId = doctors[0].Id, PatientId = patients[4].Id, Date = today.AddDays(-4), Hour = new TimeOnly(14, 0), Reason = "Consultation de suivi" },
            
            // Patient 6 (Léa) - 0 consultation (comme requis)
        };
        context.Consultations.AddRange(consultations);
        context.SaveChanges();

        // Créer au moins 5 ordonnances
        // Au moins une ordonnance avec 3 médicaments
        // Au moins un patient avec deux ordonnances
        var ordonnances = new List<Ordonnance>
        {
            // Ordonnance 1 - Patient 1 (Paul) - avec 3 médicaments
            new Ordonnance 
            { 
                DoctorId = doctors[0].Id, 
                PatientId = patients[0].Id, 
                ConsultationId = consultations[0].Id,
                Date = today.AddDays(-10),
                Notes = "Traitement cardiaque avec suivi"
            },
            
            // Ordonnance 2 - Patient 2 (Julie) - première ordonnance
            new Ordonnance 
            { 
                DoctorId = doctors[2].Id, 
                PatientId = patients[1].Id, 
                ConsultationId = consultations[2].Id,
                Date = today.AddDays(-15),
                Notes = "Traitement neurologique"
            },
            
            // Ordonnance 3 - Patient 2 (Julie) - deuxième ordonnance (patient avec 2 ordonnances)
            new Ordonnance 
            { 
                DoctorId = doctors[0].Id, 
                PatientId = patients[1].Id, 
                ConsultationId = consultations[4].Id,
                Date = today.AddDays(-3),
                Notes = "Traitement cardiaque"
            },
            
            // Ordonnance 4 - Patient 3 (Thomas)
            new Ordonnance 
            { 
                DoctorId = doctors[4].Id, 
                PatientId = patients[2].Id, 
                ConsultationId = consultations[5].Id,
                Date = today.AddDays(-12),
                Notes = "Traitement orthopédique"
            },
            
            // Ordonnance 5 - Patient 4 (Emma)
            new Ordonnance 
            { 
                DoctorId = doctors[1].Id, 
                PatientId = patients[3].Id, 
                ConsultationId = consultations[7].Id,
                Date = today.AddDays(-6),
                Notes = "Traitement dermatologique"
            }
        };
        context.Ordonnances.AddRange(ordonnances);
        context.SaveChanges();

        // Créer les lignes d'ordonnance
        // Ordonnance 1 avec 3 médicaments
        var ordonnanceLignes = new List<OrdonnanceLigne>
        {
            new OrdonnanceLigne 
            { 
                OrdonnanceId = ordonnances[0].Id, 
                MedicamentId = medicaments[0].Id, 
                Dosage = "500mg", 
                Frequency = "3 fois par jour", 
                Duration = "7 jours", 
                Quantity = 21,
                Instructions = "Prendre après les repas"
            },
            new OrdonnanceLigne 
            { 
                OrdonnanceId = ordonnances[0].Id, 
                MedicamentId = medicaments[1].Id, 
                Dosage = "400mg", 
                Frequency = "2 fois par jour", 
                Duration = "5 jours", 
                Quantity = 10,
                Instructions = "Prendre avec de la nourriture"
            },
            new OrdonnanceLigne 
            { 
                OrdonnanceId = ordonnances[0].Id, 
                MedicamentId = medicaments[2].Id, 
                Dosage = "500mg", 
                Frequency = "2 fois par jour", 
                Duration = "10 jours", 
                Quantity = 20,
                Instructions = "Prendre à jeun"
            },
            
            // Ordonnance 2
            new OrdonnanceLigne 
            { 
                OrdonnanceId = ordonnances[1].Id, 
                MedicamentId = medicaments[0].Id, 
                Dosage = "500mg", 
                Frequency = "2 fois par jour", 
                Duration = "5 jours", 
                Quantity = 10
            },
            
            // Ordonnance 3
            new OrdonnanceLigne 
            { 
                OrdonnanceId = ordonnances[2].Id, 
                MedicamentId = medicaments[1].Id, 
                Dosage = "400mg", 
                Frequency = "1 fois par jour", 
                Duration = "7 jours", 
                Quantity = 7
            },
            
            // Ordonnance 4
            new OrdonnanceLigne 
            { 
                OrdonnanceId = ordonnances[3].Id, 
                MedicamentId = medicaments[0].Id, 
                Dosage = "500mg", 
                Frequency = "3 fois par jour", 
                Duration = "3 jours", 
                Quantity = 9
            },
            
            // Ordonnance 5
            new OrdonnanceLigne 
            { 
                OrdonnanceId = ordonnances[4].Id, 
                MedicamentId = medicaments[2].Id, 
                Dosage = "500mg", 
                Frequency = "2 fois par jour", 
                Duration = "7 jours", 
                Quantity = 14
            }
        };
        context.OrdonnanceLignes.AddRange(ordonnanceLignes);
        context.SaveChanges();
    }
}

