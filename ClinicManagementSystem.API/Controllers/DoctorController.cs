using ClinicManagementSystem.BLL.Dto_s.ChatRoomDto;
using ClinicManagementSystem.BLL.Dto_s.DocDto;
using ClinicManagementSystem.BLL.Helpers;
using ClinicManagementSystem.BLL.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ClinicManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Doctor")]
    public class DoctorController : ControllerBase
    {
        private IDoctorManager _doctorManager;
        private IGetLoggedData _getLoggedData;

        public DoctorController(IDoctorManager doctorManager , IGetLoggedData getLoggedData)
        {
            _doctorManager = doctorManager;
            _getLoggedData = getLoggedData;
        }

        [HttpGet("ViewAppointments")]
        public IActionResult GetAppointments()
        {
            var found = _doctorManager.GetAppointments(_getLoggedData.GetId());

            if (found.Count() > 0)
            {
                return Ok(found);
            }

            return NotFound("You Haven't assigned to an appointment yet");
        }

        [HttpGet("ViewPatients")]
        public IActionResult GetPatients()
        {
            var found = _doctorManager.GetPatients(_getLoggedData.GetId());

            if (found.Count() > 0)
            {
                return Ok(found);
            }

            return NotFound("You don't have patients");
        }

        [HttpGet("ViewMedicalHistory/{patientId}")]
        public IActionResult GetFullMedicalHistory([FromQuery] int patientId)
        {
            var found = _doctorManager.GetFullMedicalHistory(patientId);

            if (found.Count() > 0)
            {
                return Ok(found);
            }

            return NotFound("There is no history for this patient");
        }
        [HttpPost("UpdateMedicalHistory/{patientId}")]
        public IActionResult UpdateMedicalHistory([FromBody] MedicalHistoryAddDto medicalHistoryAddDto , int patientId)
        {
            medicalHistoryAddDto.doctorId = _getLoggedData.GetId();
            medicalHistoryAddDto.patientId = patientId;

            var done = _doctorManager.AddMedicalHistory(medicalHistoryAddDto);
            if (done)
            {
                return Created(nameof(medicalHistoryAddDto), new
                {
                    medicalHistoryAddDto.patientId,
                    medicalHistoryAddDto.doctorId,
                    medicalHistoryAddDto.describtion,
                    medicalHistoryAddDto.note
                });
            }

            return NotFound("Desired Patient wasn't found");
        }

        [HttpPost("AddChatRoom")]
        public IActionResult AddChatRoom ()
        {
            ChatRoomAddDto chatRoomAddDto = new ChatRoomAddDto();
            chatRoomAddDto.doctorId = _getLoggedData.GetId();
            chatRoomAddDto.Name = _getLoggedData.GetName();

            _doctorManager.AddChatRoom(chatRoomAddDto);
            return Created(nameof(chatRoomAddDto), new
            {
                chatRoomAddDto.doctorId,
                chatRoomAddDto.Name,
            });
        }
    }
}
