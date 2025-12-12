
using System;
using KingdomHospital.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace KingdomHospital.Infrastructure.Migrations
{
    [DbContext(typeof(KingdomHospitalDbContext))]
    [Migration("Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("KingdomHospital.Domain.Entities.Consultation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date")
                        .HasColumnName("Date");

                    b.Property<int>("DoctorId")
                        .HasColumnType("int")
                        .HasColumnName("DoctorId");

                    b.Property<TimeOnly>("Hour")
                        .HasColumnType("time")
                        .HasColumnName("Hour");

                    b.Property<int>("PatientId")
                        .HasColumnType("int")
                        .HasColumnName("PatientId");

                    b.Property<string>("Reason")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("Reason");

                    b.HasKey("Id");

                    b.HasIndex("PatientId");

                    b.HasIndex("DoctorId", "Date", "Hour")
                        .HasDatabaseName("IX_CONSULTATION_DoctorId_Date_Hour");

                    b.ToTable("CONSULTATION", (string)null);
                });

            modelBuilder.Entity("KingdomHospital.Domain.Entities.Doctor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)")
                        .HasColumnName("FirstName");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)")
                        .HasColumnName("LastName");

                    b.Property<int>("SpecialtyId")
                        .HasColumnType("int")
                        .HasColumnName("SpecialtyId");

                    b.HasKey("Id");

                    b.HasIndex("SpecialtyId");

                    b.HasIndex("LastName", "FirstName")
                        .HasDatabaseName("IX_DOCTOR_LastName_FirstName");

                    b.ToTable("DOCTOR", (string)null);
                });

            modelBuilder.Entity("KingdomHospital.Domain.Entities.Medicament", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AtcCode")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("AtcCode");

                    b.Property<string>("DosageForm")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)")
                        .HasColumnName("DosageForm");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("Name");

                    b.Property<string>("Strength")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)")
                        .HasColumnName("Strength");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("MEDICAMENT", (string)null);
                });

            modelBuilder.Entity("KingdomHospital.Domain.Entities.Ordonnance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("ConsultationId")
                        .HasColumnType("int")
                        .HasColumnName("ConsultationId");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date")
                        .HasColumnName("Date");

                    b.Property<int>("DoctorId")
                        .HasColumnType("int")
                        .HasColumnName("DoctorId");

                    b.Property<string>("Notes")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("Notes");

                    b.Property<int>("PatientId")
                        .HasColumnType("int")
                        .HasColumnName("PatientId");

                    b.HasKey("Id");

                    b.HasIndex("ConsultationId");

                    b.HasIndex("DoctorId");

                    b.HasIndex("PatientId");

                    b.ToTable("ORDONNANCE", (string)null);
                });

            modelBuilder.Entity("KingdomHospital.Domain.Entities.OrdonnanceLigne", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Dosage")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("Dosage");

                    b.Property<string>("Duration")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)")
                        .HasColumnName("Duration");

                    b.Property<string>("Frequency")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("Frequency");

                    b.Property<string>("Instructions")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("Instructions");

                    b.Property<int>("MedicamentId")
                        .HasColumnType("int")
                        .HasColumnName("MedicamentId");

                    b.Property<int>("OrdonnanceId")
                        .HasColumnType("int")
                        .HasColumnName("OrdonnanceId");

                    b.Property<int>("Quantity")
                        .HasColumnType("int")
                        .HasColumnName("Quantity");

                    b.HasKey("Id");

                    b.HasIndex("MedicamentId");

                    b.HasIndex("OrdonnanceId");

                    b.ToTable("ORDONNANCE_LIGNE", null, t =>
                        {
                            t.HasCheckConstraint("CK_OrdonnanceLigne_Quantity", "Quantity > 0");
                        });
                });

            modelBuilder.Entity("KingdomHospital.Domain.Entities.Patient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("BirthDate")
                        .HasColumnType("date")
                        .HasColumnName("BirthDate");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)")
                        .HasColumnName("FirstName");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)")
                        .HasColumnName("LastName");

                    b.HasKey("Id");

                    b.HasIndex("LastName", "FirstName", "BirthDate")
                        .HasDatabaseName("IX_PATIENT_LastName_FirstName_BirthDate");

                    b.ToTable("PATIENT", (string)null);
                });

            modelBuilder.Entity("KingdomHospital.Domain.Entities.Specialty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)")
                        .HasColumnName("Name");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("SPECIALTY", (string)null);
                });

            modelBuilder.Entity("KingdomHospital.Domain.Entities.Consultation", b =>
                {
                    b.HasOne("KingdomHospital.Domain.Entities.Doctor", "Doctor")
                        .WithMany("Consultations")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("KingdomHospital.Domain.Entities.Patient", "Patient")
                        .WithMany("Consultations")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Doctor");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("KingdomHospital.Domain.Entities.Doctor", b =>
                {
                    b.HasOne("KingdomHospital.Domain.Entities.Specialty", "Specialty")
                        .WithMany("Doctors")
                        .HasForeignKey("SpecialtyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Specialty");
                });

            modelBuilder.Entity("KingdomHospital.Domain.Entities.Ordonnance", b =>
                {
                    b.HasOne("KingdomHospital.Domain.Entities.Consultation", "Consultation")
                        .WithMany("Ordonnances")
                        .HasForeignKey("ConsultationId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("KingdomHospital.Domain.Entities.Doctor", "Doctor")
                        .WithMany("Ordonnances")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("KingdomHospital.Domain.Entities.Patient", "Patient")
                        .WithMany("Ordonnances")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Consultation");

                    b.Navigation("Doctor");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("KingdomHospital.Domain.Entities.OrdonnanceLigne", b =>
                {
                    b.HasOne("KingdomHospital.Domain.Entities.Medicament", "Medicament")
                        .WithMany("OrdonnanceLignes")
                        .HasForeignKey("MedicamentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("KingdomHospital.Domain.Entities.Ordonnance", "Ordonnance")
                        .WithMany("OrdonnanceLignes")
                        .HasForeignKey("OrdonnanceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Medicament");

                    b.Navigation("Ordonnance");
                });

            modelBuilder.Entity("KingdomHospital.Domain.Entities.Consultation", b =>
                {
                    b.Navigation("Ordonnances");
                });

            modelBuilder.Entity("KingdomHospital.Domain.Entities.Doctor", b =>
                {
                    b.Navigation("Consultations");

                    b.Navigation("Ordonnances");
                });

            modelBuilder.Entity("KingdomHospital.Domain.Entities.Medicament", b =>
                {
                    b.Navigation("OrdonnanceLignes");
                });

            modelBuilder.Entity("KingdomHospital.Domain.Entities.Ordonnance", b =>
                {
                    b.Navigation("OrdonnanceLignes");
                });

            modelBuilder.Entity("KingdomHospital.Domain.Entities.Patient", b =>
                {
                    b.Navigation("Consultations");

                    b.Navigation("Ordonnances");
                });

            modelBuilder.Entity("KingdomHospital.Domain.Entities.Specialty", b =>
                {
                    b.Navigation("Doctors");
                });
#pragma warning restore 612, 618
        }
    }
}

