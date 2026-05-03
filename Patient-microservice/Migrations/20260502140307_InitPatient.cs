using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Patient_microservice.Migrations
{
    /// <inheritdoc />
    public partial class InitPatient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contact_EstMedecinTraitant",
                table: "Patients");

            migrationBuilder.RenameColumn(
                name: "Contact_Telephone",
                table: "Patients",
                newName: "Telephone");

            migrationBuilder.RenameColumn(
                name: "Contact_Nom",
                table: "Patients",
                newName: "MedecinTraitant_Nom");

            migrationBuilder.RenameColumn(
                name: "Contact_Email",
                table: "Patients",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "LieuNaissance",
                table: "Patients",
                newName: "Nationalite");

            migrationBuilder.RenameColumn(
                name: "Contact_LienParente",
                table: "Patients",
                newName: "MedecinTraitant_Specialite");

            migrationBuilder.AlterColumn<string>(
                name: "Nom",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "GroupeSanguin",
                table: "Patients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MedecinTraitant_Adresse",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedecinTraitant_NumOrdre",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedecinTraitant_Prenom",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MedecinTraitant_Telephone",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Profession",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Severite",
                table: "Antecedents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Substance",
                table: "Antecedents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeReaction",
                table: "Antecedents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ContactsUrgence",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LienParente = table.Column<int>(type: "int", nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactsUrgence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactsUrgence_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CouvertureSociales",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeCaisse = table.Column<int>(type: "int", nullable: false),
                    NumeroAffilie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroImmatriculation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateDebut = table.Column<DateOnly>(type: "date", nullable: false),
                    DateFin = table.Column<DateOnly>(type: "date", nullable: false),
                    TauxPriseEnCharge = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EstALD = table.Column<bool>(type: "bit", nullable: false),
                    CodeAld = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CouvertureSociales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CouvertureSociales_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PieceIdentites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypePiece = table.Column<int>(type: "int", nullable: false),
                    Numero = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateExpiration = table.Column<DateOnly>(type: "date", nullable: true),
                    PaysEmetteur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstPrincipal = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PieceIdentites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PieceIdentites_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactsUrgence_PatientId",
                table: "ContactsUrgence",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_CouvertureSociales_PatientId",
                table: "CouvertureSociales",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PieceIdentites_PatientId",
                table: "PieceIdentites",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactsUrgence");

            migrationBuilder.DropTable(
                name: "CouvertureSociales");

            migrationBuilder.DropTable(
                name: "PieceIdentites");

            migrationBuilder.DropColumn(
                name: "GroupeSanguin",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "MedecinTraitant_Adresse",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "MedecinTraitant_NumOrdre",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "MedecinTraitant_Prenom",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "MedecinTraitant_Telephone",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Profession",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Severite",
                table: "Antecedents");

            migrationBuilder.DropColumn(
                name: "Substance",
                table: "Antecedents");

            migrationBuilder.DropColumn(
                name: "TypeReaction",
                table: "Antecedents");

            migrationBuilder.RenameColumn(
                name: "Telephone",
                table: "Patients",
                newName: "Contact_Telephone");

            migrationBuilder.RenameColumn(
                name: "MedecinTraitant_Nom",
                table: "Patients",
                newName: "Contact_Nom");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Patients",
                newName: "Contact_Email");

            migrationBuilder.RenameColumn(
                name: "Nationalite",
                table: "Patients",
                newName: "LieuNaissance");

            migrationBuilder.RenameColumn(
                name: "MedecinTraitant_Specialite",
                table: "Patients",
                newName: "Contact_LienParente");

            migrationBuilder.AlterColumn<string>(
                name: "Nom",
                table: "Patients",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "Contact_EstMedecinTraitant",
                table: "Patients",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
