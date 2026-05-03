using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rendez_vous_microservice.Migrations
{
    /// <inheritdoc />
    public partial class Ini : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Creneaux",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedecinId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CabinetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Debut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Fin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstDisponible = table.Column<bool>(type: "bit", nullable: false),
                    TypeCreneau = table.Column<int>(type: "int", nullable: false),
                    RecurrenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Creneaux", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReglesRecurrence",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedecinId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JoursSemaineBits = table.Column<int>(type: "int", nullable: false),
                    HeureDebut = table.Column<TimeOnly>(type: "time", nullable: false),
                    HeureFin = table.Column<TimeOnly>(type: "time", nullable: false),
                    DateDebut = table.Column<DateOnly>(type: "date", nullable: false),
                    DateFin = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReglesRecurrence", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RendezVous",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedecinId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CabinetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateHeure = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DureeMinutes = table.Column<int>(type: "int", nullable: false),
                    Motif = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Statut = table.Column<int>(type: "int", nullable: false),
                    TypeConsultation = table.Column<int>(type: "int", nullable: false),
                    NoteSecretaire = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RappelEnvoye = table.Column<bool>(type: "bit", nullable: false),
                    RappelAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RendezVous", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Creneaux");

            migrationBuilder.DropTable(
                name: "ReglesRecurrence");

            migrationBuilder.DropTable(
                name: "RendezVous");
        }
    }
}
