using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicManagementSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class admin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ApplicationUsers",
                columns: new[] { "id", "email", "firstName", "lastName", "password", "phoneNumber", "role", "userName" },
                values: new object[] { 1, "admin@gmail.com", "Admin", "1", new byte[] { 36, 50, 97, 36, 49, 49, 36, 101, 65, 80, 84, 101, 87, 104, 119, 97, 115, 52, 67, 75, 116, 105, 115, 104, 52, 47, 100, 54, 101, 87, 115, 73, 67, 51, 120, 57, 101, 102, 47, 121, 105, 52, 80, 100, 46, 115, 111, 67, 84, 121, 49, 55, 85, 105, 74, 66, 113, 57, 73, 101 }, "1234567890", "Admin", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ApplicationUsers",
                keyColumn: "id",
                keyValue: 1);
        }
    }
}
