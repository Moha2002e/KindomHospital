using System.Linq;
using KingdomHospital.Application.Mappers;
using KingdomHospital.Application.Repositories;
using KingdomHospital.Application.Services;
using KingdomHospital.Infrastructure;
using KingdomHospital.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

/// <summary>
/// Point d'entrée de l'application KingdomHospital
/// Configure les services, la base de données et démarre l'API REST
/// </summary>
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSerilog((services, lc) =>
    lc.ReadFrom.Configuration(builder.Configuration));

builder.Services.AddDbContext<KingdomHospitalDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IConsultationRepository, ConsultationRepository>();
builder.Services.AddScoped<IMedicamentRepository, MedicamentRepository>();
builder.Services.AddScoped<IOrdonnanceRepository, OrdonnanceRepository>();

builder.Services.AddScoped<SpecialtyMapper>();
builder.Services.AddScoped<DoctorMapper>();
builder.Services.AddScoped<PatientMapper>();
builder.Services.AddScoped<ConsultationMapper>();
builder.Services.AddScoped<MedicamentMapper>();
builder.Services.AddScoped<OrdonnanceMapper>();
builder.Services.AddScoped<OrdonnanceLigneMapper>();

builder.Services.AddScoped<SpecialtyService>();
builder.Services.AddScoped<DoctorService>();
builder.Services.AddScoped<PatientService>();
builder.Services.AddScoped<ConsultationService>();
builder.Services.AddScoped<MedicamentService>();
builder.Services.AddScoped<OrdonnanceService>();

var app = builder.Build();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Only use HTTPS redirection if HTTPS is actually configured
if (app.Configuration["ASPNETCORE_URLS"]?.Contains("https://") == true || 
    app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    
    try
    {
        var context = services.GetRequiredService<KingdomHospitalDbContext>();
        
        if (!context.Database.CanConnect())
        {
            logger.LogInformation("Création de la base de données...");
            context.Database.EnsureCreated();
        }
        else
        {
            try
            {
                var pendingMigrations = context.Database.GetPendingMigrations().ToList();
                if (pendingMigrations.Any())
                {
                    logger.LogInformation("Application des migrations en attente...");
                    context.Database.Migrate();
                }
                else
                {
                    logger.LogInformation("Toutes les migrations sont déjà appliquées.");
                }
            }
            catch (Exception dbEx)
            {
                logger.LogWarning(dbEx, "Une erreur s'est produite lors de la vérification des migrations. Continuation...");
            }
        }
        
        KingdomHospitalDbContext.Seed(context);
        logger.LogInformation("Base de données initialisée avec succès.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Une erreur s'est produite lors de l'initialisation de la base de données.");
    }
}

app.Run();
