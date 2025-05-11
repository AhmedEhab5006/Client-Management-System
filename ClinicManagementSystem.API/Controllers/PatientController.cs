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
            try
            {
                int patientId = GetPatientIdFromToken();
                var success = _patientManager.BookAppointment(patientId, dto);
                if (!success)
                    return BadRequest("Appointment not available or already booked.");
                return Ok("Appointment booked successfully.");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while booking the appointment: " + ex.InnerException);
            }
        }

        [HttpPost("CancelReservation")]
        public IActionResult CancelAppointment([FromBody] AppointmentCancelDto dto)
        {
            try
            {
                int patientId = GetPatientIdFromToken();
                var success = _patientManager.CancelAppointment(dto, patientId);
                if (!success)
                    return BadRequest("Reservation not found or unauthorized");
                return Ok("Reservation canceled successfully.");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while canceling the appointment: " + ex.Message);
            }
        }

        [HttpPost("RescheduleReservation")]
        public IActionResult RescheduleReservation([FromBody] AppointmentRescheduleDto dto)
        {
            try
            {
                int patientId = GetPatientIdFromToken();
                var success = _patientManager.RescheduleAppointment(dto, patientId);
                if (!success)
                    return BadRequest("Reservation not found or unauthorized");
                return Ok("Reservation rescheduled successfully.");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred while rescheduling the appointment: " + ex.Message);
            }
        }

        [HttpGet("GetMyAppointments")]
        public IActionResult GetMyAppointments()
        {
            try
            {
                int patientId = GetPatientIdFromToken();
                var appointments = _patientManager.GetMyAppointments(patientId);
                if (appointments == null || !appointments.Any())
                    return NotFound("No appointments found.");
                return Ok(appointments);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred: " + ex.Message);
            }
        }

        [HttpGet("GetMyMedicalHistory")]
        public IActionResult GetMyMedicalHistory()
        {
            try
            {
                int patientId = GetPatientIdFromToken();
                var medicalHistory = _patientManager.GetMyMedicalHistory(patientId);
                if (medicalHistory == null || !medicalHistory.Any())
                    return NotFound("No medical history found.");
                return Ok(medicalHistory);
            }
            catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (ApplicationException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred: " + ex.Message);
            }
        }

        [HttpGet("GetAvailaleDoctors")]
        public IActionResult GetAllDoctors()
        {
            var found = _patientManager.GetAllDoctors();

            if (found.Count() > 0)
            {
                return Ok(found);
            }

            return NotFound("No doctors to show");
        }
    }
}
