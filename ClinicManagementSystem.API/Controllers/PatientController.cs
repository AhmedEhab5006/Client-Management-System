using Microsoft.AspNetCore.Mvc;
using ClinicManagementSystem.BLL.Managers;
using ClinicManagementSystem.DAL.Models;
using ClinicManagementSystem.BLL.Dto_s.PatientDto_s;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ClinicManagementSystem.BLL.Helpers;

namespace ClinicManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Patient")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientManager _patientManager;
        private readonly IGetLoggedData _getLoggedData;

        public PatientController(IPatientManager patientManager , IGetLoggedData getLoggedData)
        {
            _patientManager = patientManager;
            _getLoggedData = getLoggedData;
        }
        private int GetPatientIdFromToken()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var claim = identity?.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null ? int.Parse(claim.Value) : 0;
        }

        [HttpPost("BookAppointment/{id}")]
        public IActionResult BookAppointment(int id)
        {
            try
            {
                int patientId = GetPatientIdFromToken();
                var dto = new AppointmentBookingDto();
                dto.AppointmentId = id;
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

        [HttpPost("CancelReservation/{id}")]
        public IActionResult CancelAppointment(int id)
        {
            try
            {
                int patientId = GetPatientIdFromToken();
                var cancelDto = new AppointmentCancelDto();
                cancelDto.ReservationId = id;
                var success = _patientManager.CancelAppointment(cancelDto, patientId);
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
            catch (UnauthorizedAccessException ex)
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

        [HttpGet("GetAvailaleAppointments/{docID}")]
        public IActionResult GetAvailableAppointments(int docID)
        {
            var found = _patientManager.GetDoctorAppointment(docID);

            if (found.Count() > 0)
            {
                return Ok(found);
            }

            return NotFound("No appointments for this doctor");
        }

        [HttpGet("GetMyPendingCount")]
        public IActionResult PendingCount()
        {
            var found = _patientManager.ViewPendingCount(_getLoggedData.GetId());
            
            if (found != null)
            {
                return Ok(found);
            }

            return NotFound("No patient with this id");

        }

        [HttpGet("GetMyApprovedCount")]
        public IActionResult ApprovedCount()
        {
            var found = _patientManager.ViewApprovedCount(_getLoggedData.GetId());

            if (found != null)
            {
                return Ok(found);
            }

            return NotFound("No patient with this id");

        }

        [HttpGet("GetMyRejectedCount")]
        public IActionResult RejectedCount()
        {
            var found = _patientManager.ViewRejectedCount(_getLoggedData.GetId());

            if (found != null)
            {
                return Ok(found);
            }

            return NotFound("No patient with this id");

        }
    }
}
