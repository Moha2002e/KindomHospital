using KingdomHospital.Application.Mappers;
using KingdomHospital.Application.Repositories;
using KingdomHospital.Application.Services;
using KingdomHospital.Infrastructure;
using KingdomHospital.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

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

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        var context = services.GetRequiredService<KingdomHospitalDbContext>();

        logger.LogInformation("Application des migrations...");
        context.Database.Migrate();

        KingdomHospitalDbContext.Seed(context);
        logger.LogInformation("Base de données initialisée avec succès.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Erreur lors de l'initialisation de la base de données.");
    }
}

app.Run();
