using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicManagementSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class EditDocAppointmentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "timeSlot",
                table: "DoctorAppointments",
                newName: "appointmentStart");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "appointmentEnd",
                table: "DoctorAppointments",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "duration",
                table: "DoctorAppointments",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 2, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "appointmentEnd",
                table: "DoctorAppointments");

            migrationBuilder.DropColumn(
                name: "duration",
                table: "DoctorAppointments");

            migrationBuilder.RenameColumn(
                name: "appointmentStart",
                table: "DoctorAppointments",
                newName: "timeSlot");
        }
    }
}
