using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dossier_Medical_microservice.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dossiers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedecinId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CabinetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumeroDossier = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateOuverture = table.Column<DateOnly>(type: "date", nullable: false),
                    Statut = table.Column<int>(type: "int", nullable: false),
                    Motif = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Anamnese = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCloture = table.Column<DateOnly>(type: "date", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dossiers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Consultations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DossierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RendezVousId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    TypeConsultation = table.Column<int>(type: "int", nullable: false),
                    Motif = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Statut = table.Column<int>(type: "int", nullable: false),
                    ExamenClinique_Poids = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    ExamenClinique_Taille = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    ExamenClinique_TA = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ExamenClinique_Pouls = table.Column<int>(type: "int", nullable: true),
                    ExamenClinique_Temperature = table.Column<decimal>(type: "decimal(4,1)", precision: 4, scale: 1, nullable: true),
                    ExamenClinique_Observations = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Conclusion = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    DateCloture = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Consultations_Dossiers_DossierId",
                        column: x => x.DossierId,
                        principalTable: "Dossiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DossierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeDocument = table.Column<int>(type: "int", nullable: false),
                    Titre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CheminFichier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateDocument = table.Column<DateOnly>(type: "date", nullable: false),
                    MimeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TailleOctets = table.Column<long>(type: "bigint", nullable: false),
                    EstSupprime = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Dossiers_DossierId",
                        column: x => x.DossierId,
                        principalTable: "Dossiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Diagnostics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConsultationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodeCIM11 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LibelleCIM11 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Commentaire = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstConfirme = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diagnostics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Diagnostics_Consultations_ConsultationId",
                        column: x => x.ConsultationId,
                        principalTable: "Consultations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ordonnances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConsultationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    ValiditeJours = table.Column<int>(type: "int", nullable: false),
                    Instructions = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ordonnances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ordonnances_Consultations_ConsultationId",
                        column: x => x.ConsultationId,
                        principalTable: "Consultations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LignesOrdonnances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrdonnanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedicamentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NomMedicament = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Posologie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DureeJours = table.Column<int>(type: "int", nullable: false),
                    Quantite = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LignesOrdonnances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LignesOrdonnances_Ordonnances_OrdonnanceId",
                        column: x => x.OrdonnanceId,
                        principalTable: "Ordonnances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Consultations_Date",
                table: "Consultations",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_Consultations_DossierId",
                table: "Consultations",
                column: "DossierId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnostics_CodeCIM11",
                table: "Diagnostics",
                column: "CodeCIM11");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnostics_ConsultationId",
                table: "Diagnostics",
                column: "ConsultationId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_DossierId",
                table: "Documents",
                column: "DossierId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_TypeDocument",
                table: "Documents",
                column: "TypeDocument");

            migrationBuilder.CreateIndex(
                name: "IX_Dossiers_NumeroDossier",
                table: "Dossiers",
                column: "NumeroDossier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dossiers_PatientId",
                table: "Dossiers",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_LignesOrdonnances_OrdonnanceId",
                table: "LignesOrdonnances",
                column: "OrdonnanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Ordonnances_ConsultationId",
                table: "Ordonnances",
                column: "ConsultationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Diagnostics");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "LignesOrdonnances");

            migrationBuilder.DropTable(
                name: "Ordonnances");

            migrationBuilder.DropTable(
                name: "Consultations");

            migrationBuilder.DropTable(
                name: "Dossiers");
        }
    }
}
