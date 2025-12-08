using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KingdomHospital.Infrastructure.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MEDICAMENT",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DosageForm = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Strength = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    AtcCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MEDICAMENT", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PATIENT",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PATIENT", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SPECIALTY",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SPECIALTY", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DOCTOR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpecialtyId = table.Column<int>(type: "int", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DOCTOR", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DOCTOR_SPECIALTY_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "SPECIALTY",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CONSULTATION",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Hour = table.Column<TimeOnly>(type: "time", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONSULTATION", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CONSULTATION_DOCTOR_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "DOCTOR",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CONSULTATION_PATIENT_PatientId",
                        column: x => x.PatientId,
                        principalTable: "PATIENT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ORDONNANCE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    ConsultationId = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORDONNANCE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ORDONNANCE_CONSULTATION_ConsultationId",
                        column: x => x.ConsultationId,
                        principalTable: "CONSULTATION",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ORDONNANCE_DOCTOR_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "DOCTOR",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ORDONNANCE_PATIENT_PatientId",
                        column: x => x.PatientId,
                        principalTable: "PATIENT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ORDONNANCE_LIGNE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrdonnanceId = table.Column<int>(type: "int", nullable: false),
                    MedicamentId = table.Column<int>(type: "int", nullable: false),
                    Dosage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Frequency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Duration = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Instructions = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORDONNANCE_LIGNE", x => x.Id);
                    table.CheckConstraint("CK_OrdonnanceLigne_Quantity", "Quantity > 0");
                    table.ForeignKey(
                        name: "FK_ORDONNANCE_LIGNE_MEDICAMENT_MedicamentId",
                        column: x => x.MedicamentId,
                        principalTable: "MEDICAMENT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ORDONNANCE_LIGNE_ORDONNANCE_OrdonnanceId",
                        column: x => x.OrdonnanceId,
                        principalTable: "ORDONNANCE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CONSULTATION_DoctorId_Date_Hour",
                table: "CONSULTATION",
                columns: new[] { "DoctorId", "Date", "Hour" });

            migrationBuilder.CreateIndex(
                name: "IX_CONSULTATION_PatientId",
                table: "CONSULTATION",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_DOCTOR_LastName_FirstName",
                table: "DOCTOR",
                columns: new[] { "LastName", "FirstName" });

            migrationBuilder.CreateIndex(
                name: "IX_DOCTOR_SpecialtyId",
                table: "DOCTOR",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_MEDICAMENT_Name",
                table: "MEDICAMENT",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ORDONNANCE_ConsultationId",
                table: "ORDONNANCE",
                column: "ConsultationId");

            migrationBuilder.CreateIndex(
                name: "IX_ORDONNANCE_DoctorId",
                table: "ORDONNANCE",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_ORDONNANCE_PatientId",
                table: "ORDONNANCE",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_ORDONNANCE_LIGNE_MedicamentId",
                table: "ORDONNANCE_LIGNE",
                column: "MedicamentId");

            migrationBuilder.CreateIndex(
                name: "IX_ORDONNANCE_LIGNE_OrdonnanceId",
                table: "ORDONNANCE_LIGNE",
                column: "OrdonnanceId");

            migrationBuilder.CreateIndex(
                name: "IX_PATIENT_LastName_FirstName_BirthDate",
                table: "PATIENT",
                columns: new[] { "LastName", "FirstName", "BirthDate" });

            migrationBuilder.CreateIndex(
                name: "IX_SPECIALTY_Name",
                table: "SPECIALTY",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ORDONNANCE_LIGNE");

            migrationBuilder.DropTable(
                name: "MEDICAMENT");

            migrationBuilder.DropTable(
                name: "ORDONNANCE");

            migrationBuilder.DropTable(
                name: "CONSULTATION");

            migrationBuilder.DropTable(
                name: "DOCTOR");

            migrationBuilder.DropTable(
                name: "PATIENT");

            migrationBuilder.DropTable(
                name: "SPECIALTY");
        }
    }
}

