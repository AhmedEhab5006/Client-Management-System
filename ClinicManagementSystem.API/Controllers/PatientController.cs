using Microsoft.AspNetCore.Mvc;
using ClinicManagementSystem.BLL.Managers;
using ClinicManagementSystem.DAL.Models;
using ClinicManagementSystem.BLL.Dto_s.PatientDto_s;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ClinicManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Patient")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientManager _patientManager;
        public PatientController(IPatientManager patientManager)
        {
            _patientManager = patientManager;
        }
        private int GetPatientIdFromToken()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var claim = identity?.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null ? int.Parse(claim.Value) : 0;
        }

        [HttpPost("BookAppointment")]
        public IActionResult BookAppointment([FromBody] AppointmentBookingDto dto)
        {
            int patientId = GetPatientIdFromToken();
            var success = _patientManager.BookAppointment(patientId, dto);
            if(!success)
                return BadRequest("Appointment not available or already booked.");
            return Ok("Appointment booked successfully.");
        }

        [HttpPost("CancelReservation")]
        public IActionResult CancelAppointment([FromBody] AppointmentCancelDto dto)
        {
            int patientId = GetPatientIdFromToken();
            var success = _patientManager.CancelAppointment(dto, patientId);
            if (!success)
                return BadRequest("Reservation not found or unauthorized");
            return Ok("Reservation canceled successfully.");
        }

        [HttpPost("RescheduleReservation")]
        public IActionResult RescheduleReservation([FromBody] AppointmentRescheduleDto dto)
        {
            int patientId = GetPatientIdFromToken();
            var success = _patientManager.RescheduleAppointment(dto, patientId);
            if (!success)
                return BadRequest("Reservation not found or unauthorized");
            return Ok("Reservation rescheduled successfully.");
        }

        [HttpGet("GetMyAppointments")]
        public IActionResult GetMyAppointments()
        {
            int patientId = GetPatientIdFromToken();
            var appointments = _patientManager.GetMyAppointments(patientId);
            if (appointments == null || !appointments.Any())
                return NotFound("No appointments found.");
            return Ok(appointments);
        }
    }
}
