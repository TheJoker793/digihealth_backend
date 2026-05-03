using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Patient_microservice.Migrations
{
    /// <inheritdoc />
    public partial class AddAssuranceComplementaire : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssuranceComplementaires",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NomAssureur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroPolice = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateDebut = table.Column<DateOnly>(type: "date", nullable: false),
                    DateFin = table.Column<DateOnly>(type: "date", nullable: false),
                    TauxRemboursement = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssuranceComplementaires", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssuranceComplementaires_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssuranceComplementaires_PatientId",
                table: "AssuranceComplementaires",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssuranceComplementaires");
        }
    }
}
