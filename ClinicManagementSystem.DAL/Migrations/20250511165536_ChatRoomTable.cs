using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicManagementSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChatRoomTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatRooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    doctorId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatRooms_Doctors_doctorId",
                        column: x => x.doctorId,
                        principalTable: "Doctors",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "ApplicationUsers",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: new byte[] { 36, 50, 97, 36, 49, 49, 36, 120, 116, 114, 82, 102, 83, 102, 107, 112, 100, 77, 74, 114, 86, 89, 109, 73, 72, 115, 78, 78, 79, 109, 102, 118, 85, 112, 78, 120, 105, 73, 50, 48, 90, 65, 78, 100, 104, 52, 102, 122, 90, 57, 105, 90, 86, 48, 82, 102, 47, 119, 106, 79 });

            migrationBuilder.CreateIndex(
                name: "IX_ChatRooms_doctorId",
                table: "ChatRooms",
                column: "doctorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatRooms");

            migrationBuilder.UpdateData(
                table: "ApplicationUsers",
                keyColumn: "id",
                keyValue: 1,
                column: "password",
                value: new byte[] { 36, 50, 97, 36, 49, 49, 36, 100, 119, 69, 66, 115, 48, 117, 49, 98, 46, 99, 86, 112, 90, 102, 76, 51, 50, 116, 88, 74, 101, 80, 72, 104, 86, 88, 98, 77, 120, 85, 76, 49, 85, 116, 110, 90, 122, 80, 111, 121, 56, 47, 55, 119, 119, 80, 104, 119, 116, 120, 50, 79 });
        }
    }
}
