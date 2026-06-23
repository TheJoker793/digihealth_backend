using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Patient_microservice.Migrations
{
    /// <inheritdoc />
    public partial class FixMedecinTraitant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MedecinTraitant_Adresse",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "MedecinTraitant_Nom",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "MedecinTraitant_NumOrdre",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "MedecinTraitant_Prenom",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "MedecinTraitant_Specialite",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "MedecinTraitant_Telephone",
                table: "Patients");

            migrationBuilder.AddColumn<Guid>(
                name: "MedecinTraitantId",
                table: "Patients",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MedecinsTraitants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Specialite = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumOrdre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adresse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedecinsTraitants", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Patients_MedecinTraitantId",
                table: "Patients",
                column: "MedecinTraitantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_MedecinsTraitants_MedecinTraitantId",
                table: "Patients",
                column: "MedecinTraitantId",
                principalTable: "MedecinsTraitants",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_MedecinsTraitants_MedecinTraitantId",
                table: "Patients");

            migrationBuilder.DropTable(
                name: "MedecinsTraitants");

            migrationBuilder.DropIndex(
                name: "IX_Patients_MedecinTraitantId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "MedecinTraitantId",
                table: "Patients");

            migrationBuilder.AddColumn<string>(
                name: "MedecinTraitant_Adresse",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedecinTraitant_Nom",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
                name: "MedecinTraitant_Specialite",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MedecinTraitant_Telephone",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
