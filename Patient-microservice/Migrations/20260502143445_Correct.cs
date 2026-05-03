using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Patient_microservice.Migrations
{
    /// <inheritdoc />
    public partial class Correct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactsUrgence_Patients_PatientId",
                table: "ContactsUrgence");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContactsUrgence",
                table: "ContactsUrgence");

            migrationBuilder.RenameTable(
                name: "ContactsUrgence",
                newName: "ContactsUrgences");

            migrationBuilder.RenameIndex(
                name: "IX_ContactsUrgence_PatientId",
                table: "ContactsUrgences",
                newName: "IX_ContactsUrgences_PatientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContactsUrgences",
                table: "ContactsUrgences",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactsUrgences_Patients_PatientId",
                table: "ContactsUrgences",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactsUrgences_Patients_PatientId",
                table: "ContactsUrgences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContactsUrgences",
                table: "ContactsUrgences");

            migrationBuilder.RenameTable(
                name: "ContactsUrgences",
                newName: "ContactsUrgence");

            migrationBuilder.RenameIndex(
                name: "IX_ContactsUrgences_PatientId",
                table: "ContactsUrgence",
                newName: "IX_ContactsUrgence_PatientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContactsUrgence",
                table: "ContactsUrgence",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactsUrgence_Patients_PatientId",
                table: "ContactsUrgence",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
