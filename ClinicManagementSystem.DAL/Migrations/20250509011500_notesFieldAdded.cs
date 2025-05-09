using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicManagementSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class notesFieldAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "note",
                table: "MedicalHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "ApplicationUsers",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: new byte[] { 36, 50, 97, 36, 49, 49, 36, 100, 119, 69, 66, 115, 48, 117, 49, 98, 46, 99, 86, 112, 90, 102, 76, 51, 50, 116, 88, 74, 101, 80, 72, 104, 86, 88, 98, 77, 120, 85, 76, 49, 85, 116, 110, 90, 122, 80, 111, 121, 56, 47, 55, 119, 119, 80, 104, 119, 116, 120, 50, 79 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "note",
                table: "MedicalHistories");

            migrationBuilder.UpdateData(
                table: "ApplicationUsers",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: new byte[] { 36, 50, 97, 36, 49, 49, 36, 101, 65, 80, 84, 101, 87, 104, 119, 97, 115, 52, 67, 75, 116, 105, 115, 104, 52, 47, 100, 54, 101, 87, 115, 73, 67, 51, 120, 57, 101, 102, 47, 121, 105, 52, 80, 100, 46, 115, 111, 67, 84, 121, 49, 55, 85, 105, 74, 66, 113, 57, 73, 101 });
        }
    }
}
