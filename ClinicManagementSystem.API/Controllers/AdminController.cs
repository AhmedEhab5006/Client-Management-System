using ClinicManagementSystem.BLL.Dto_s.AdminDTO;
using ClinicManagementSystem.BLL.Managers;
using ClinicManagementSystem.DAL.Database;
using ClinicManagementSystem.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Resources;

namespace ClinicManagementSystem.API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        private readonly ProgramContext _context;
        private readonly IPasswordHandlerManager _passwordHandlerManager;
        private readonly IAdminManager _adminManager;

        public AdminController(IConfiguration config , IPasswordHandlerManager passwordHandlerManager , IAdminManager adminManager)
        {
            _context = new(config);
            _passwordHandlerManager = passwordHandlerManager;
            _adminManager = adminManager;
        }


        //1st Method : Adding Doctor

        [HttpPost("AddDoc")]
        public IActionResult AddDoc(AddDocDTO AddDoc)
        {
            ApplicationUser user = new();
            user.firstName = AddDoc.firstName;
            user.lastName = AddDoc.lastName;
            user.userName = AddDoc.userName;
            user.email = AddDoc.email;
            user.password = _passwordHandlerManager.HashPasswordToByteArray(AddDoc.password);
            user.phoneNumber = AddDoc.phoneNumber;
            user.role = "Doctor";
            var check = _context.ApplicationUsers.Where(e => e.email == AddDoc.email).ToList();
            if (check.Count != 0)
            {
                return Unauthorized("Doctor with this email address already exists");
            }
            _context.ApplicationUsers.Add(user);
            _context.SaveChanges();
            var docUser = _context.ApplicationUsers.Where(i => i.email == user.email).FirstOrDefault();
            Doctor doctor = new();
            doctor.userId = docUser.id;
            doctor.major = AddDoc.major;
            doctor.location = AddDoc.location;
            _context.Doctors.Add(doctor);
            if (_context.SaveChanges() > 0)
                return Ok();
            return BadRequest("Could not add Doctor");

        }

        //2nd Method : list doctors

        [HttpGet("ListDocs")]
        public IEnumerable<GetDocsDTO> GetDocs()
        {
            List<GetDocsDTO> docDTO = new();
            var docUsers = _context.ApplicationUsers.Where(a=>a.role == "Doctor").ToList();
            var docProp = _context.Doctors.ToList();
            for (int i = 0; i < docUsers.Count; i++)
            {
                docDTO.Add(new GetDocsDTO
                {
                    id = docUsers[i].id,
                    firstName = docUsers[i].firstName,
                    lastName = docUsers[i].lastName,
                    userName = docUsers[i].userName,
                    email = docUsers[i].email,
                    phoneNumber = docUsers[i].phoneNumber,
                    major = docProp[i].major,
                    location = docProp[i].location,
                });
            }
            return docDTO;
        }

        //3rd Method : Editing Doctors

        [HttpPut("EditDoctor/{id}")]
        public IActionResult EditDoctor(int id, [FromBody] EditDocDTO editDoc)
        {
            var DocUser = _context.ApplicationUsers.Where(i => i.id == id).FirstOrDefault();
            var docProp = _context.Doctors.Where(i => i.userId == id).FirstOrDefault();
            if (DocUser != null)
            {
                var newPassword = _passwordHandlerManager.HashPasswordToByteArray(editDoc.password);
                DocUser.firstName = editDoc.firstName;
                DocUser.lastName = editDoc.lastName;
                DocUser.userName = editDoc.userName;
                DocUser.email = editDoc.email;
                DocUser.password = newPassword;
                DocUser.phoneNumber = editDoc.phoneNumber;
                docProp.major = editDoc.major;
                docProp.location = editDoc.location;
                if (_context.SaveChanges() > 0)
                    return Ok();
                return BadRequest("No updates were made");
            }
            return NotFound("User Does not exist!!");
        }

        //4th Method : Removing a Doctor
        [HttpDelete("RemoveDoc/{id}")]
        public IActionResult DeleteDoc(int id)
        {
            var checkUser = _context.ApplicationUsers.Where(i => i.id == id).FirstOrDefault();
            if (checkUser == null)
            {
                return NotFound("Doctor does not exist");
            }
            var checkDoc = _context.Doctors.Where(i => i.userId == checkUser.id).FirstOrDefault();
            _context.Doctors.Remove(checkDoc);
            _context.ApplicationUsers.Remove(checkUser);
            if (_context.SaveChanges() > 0)
                return Ok();
            return BadRequest("Could not remove Doctor");
        }

        //5th Method : Assigning Doctor

        [HttpPost("AssignDoctor")]
        public IActionResult AssigningDoctor(AssignDocDTO assign)
        {
            var check = _context.Doctors.Find(assign.doctorId);
            if (check != null)
            {
                var checkAppointment = _context.DoctorAppointments
                    .Where(i => i.doctorId == assign.doctorId && i.date == assign.date && i.appointmentStart == assign.appointmentStart).FirstOrDefault();
                if (checkAppointment == null)
                {
                    DoctorAppointment app = new();
                    app.doctorId = assign.doctorId;
                    app.date = assign.date;
                    app.appointmentStart = assign.appointmentStart;
                    app.appointmentEnd = assign.appointmentStart.Add(new TimeSpan(2, 00, 00));
                    app.status = "Available";
                    _context.DoctorAppointments.Add(app);
                    if (_context.SaveChanges() > 0)
                    {
                        return Ok();
                    }
                    return BadRequest("Could not add Doctor Appointment");
                }
                return BadRequest("Doctor already assigned");
            }
            return NotFound("Doctor does not exist");
        }

        //Appointment Handling : 

        [HttpGet("GetPendingAppointments")]
        public IActionResult listRequestedAppointments()
        {
            var PendingReservations = _adminManager.GetAllPendingAppointments();
            
            if (PendingReservations.Count() > 0)
            {
                return Ok(PendingReservations);
            }

            return NotFound("No Pending Reservations");
        }

        [HttpGet("GetConfirmedAppointments")]
        public IActionResult listReservedAppointments()
        {
            var ConfirmedReservations = _adminManager.GetAllApprovedAppointments();
            
            if (ConfirmedReservations.Count() > 0)
            {
                return Ok(ConfirmedReservations);
            }

            return NotFound("No Approved Appointments");
        }



        // Method To confirm "Reserve" a Pending Appointment
        [HttpPut("AdminConfirmAppointment/{reservationId}")]
        public IActionResult ConfirmReservation(int reservationId)
        {
            var reservation = _context.Reservations.Where(i => i.id == reservationId)
                                                   .Where(a=>a.status != "Reserved")
                                                   .FirstOrDefault();
            
            var same = _adminManager.GetSame(reservation.appointmentId).ToList();
            if (same.Count > 0 && same.Any(a=>a.status == "Booked"))
            {
                return BadRequest("There is a conflict can't make a booking");
            }
                         
            
            if (reservation != null)
            {
                reservation.status = "Reserved";
                Patient patient = new();
                DoctorPatient doctorPatient = new DoctorPatient();
                patient = _context.Patients.Where(a => a.userId == reservation.patientId).FirstOrDefault();
                var getDoc = _context.DoctorAppointments.Where(i => i.Id == reservation.appointmentId).FirstOrDefault();
                getDoc.status = "Booked";
                doctorPatient.DoctorId = getDoc.doctorId;
                doctorPatient.PatientId = reservation.patientId;
                _context.DoctorPatients.Add(doctorPatient);
                patient.approvedAppointments += 1;

                if(patient.pendingAppointments > 0)
                {
                    patient.pendingAppointments -= 1;
                }

                _context.Patients.Update(patient);
                if (_context.SaveChanges() > 0)
                    return Ok();
                return BadRequest ("Could not confirm Reservation");

            }
            return NotFound("Reservation Not Found!!");
        }



        //Method to cancel a pending OR confirmed Reservation
        [HttpDelete("CancelReservation/{reservationId}")]
        public IActionResult CancelReservation(int reservationId)
        {
            var reservation = _context.Reservations.Where(i => i.id == reservationId).FirstOrDefault();
            if (reservation != null)
            {
                if (reservation.status == "Reserved")
                {
                    var getDoc = _context.DoctorAppointments.Where(i => i.Id == reservation.appointmentId).FirstOrDefault();
                    getDoc.status = "Available";
                    var DocPatient = _context.DoctorPatients.Where(i => i.DoctorId == getDoc.doctorId && i.Patient.userId == reservation.patientId).FirstOrDefault();
                    var patient = _context.Patients.Find(reservation.patientId);

                    if (patient.approvedAppointments > 0)
                    {
                        patient.approvedAppointments -= 1;
                    }

                    patient.rejectedAppointments += 1;
                    _context.DoctorPatients.Remove(DocPatient);

                }
                _context.Reservations.Remove(reservation);
                if (_context.SaveChanges() > 0)
                    return Ok();
                return BadRequest("Could not Cancel Reservation");
            }
            return NotFound("Reservation Not Found!!");
        }



        //Get a Doctor's Appointments with a Specific date
        [HttpGet("ListDocAppointments")]
        public IEnumerable<DoctorAppointment> doctorAppointments(int docId,DateOnly date)
        {
            var appointments = _context.DoctorAppointments.Where(i => i.doctorId == docId && i.date == date).ToList();
            return appointments;
        }

        //reschedule a Reservation (Use doctorAppointments Method to get all of the doctor's available appointments)
        [HttpPut("RescheduleReservation/{reservationId}")]
        public IActionResult ReservationReschedule(int reservationId,[FromBody]int newAppointmentId)
        {
            var reservation = _context.Reservations.Where(i => i.id == reservationId).FirstOrDefault();
            if(reservation != null)
            {
                var appointment = _context.DoctorAppointments.Where(i => i.Id == reservation.appointmentId).FirstOrDefault().status;
                appointment = "Available";
                reservation.appointmentId = newAppointmentId;
                var newAppointment = _context.DoctorAppointments.Where(i => i.Id == newAppointmentId).FirstOrDefault();
                newAppointment.status = "Booked";
                if (_context.SaveChanges() > 0)
                    return Ok();
                return BadRequest("No Changes Made");
            }return NotFound("Reservation not Found!!");
        }



        //This method returns the booked appointments of a Specific Doctor
        [HttpGet("DocReport/{docId}")]
        public IActionResult DoctorReport(int docId)
        {
            var bookedDocAppointments = _context.DoctorAppointments.Where(i => i.doctorId == docId && i.status == "Booked").ToList();
            if(bookedDocAppointments.Count > 0)
            {
                //Constant through all of the doctor's appointments
                var docName = _context.ApplicationUsers.Where(i => i.id == bookedDocAppointments[0].doctorId).FirstOrDefault().userName;
                var docMajor = _context.Doctors.Where(i => i.userId == docId).FirstOrDefault().major;
                List<DocReportDTO> docReportDTO = new();
                for(int i = 0; i < bookedDocAppointments.Count; i++)
                {
                    var appointment = _context.DoctorAppointments.Where(x => x.Id == bookedDocAppointments[i].Id).FirstOrDefault();
                    //from this get appointment id / date / start time
                    var patId = _context.Reservations.Where(x => x.appointmentId == bookedDocAppointments[i].Id).FirstOrDefault().patientId;
                    var patientName = _context.ApplicationUsers.Where(x => x.id == patId).FirstOrDefault();
                    docReportDTO.Add(new DocReportDTO
                    {
                        id = docId,
                        DocUserName = docName,
                        major = docMajor,
                        appointmentId = appointment.Id,
                        date = appointment.date,
                        appointmentStart = appointment.appointmentStart,
                        patientId = patId,
                        PatientUserName = patientName.userName
                    });
                }
                return Ok (docReportDTO);
            }return NotFound("No reserved appointments for this doctor"); 

            
        }
        [HttpGet("GetDocById/{id}")]
        public IActionResult GetDocById (int id)
        {
            var found = _adminManager.GetDocById(id);

            if (found != null)
            {
                return Ok (found);
            }

            return NotFound("No doctor with that id");
        }

        [HttpGet("GetAllPatients")]
        public IActionResult GetAllPatients()
        {
            var found = _adminManager.GetAllPatients();

            if (found.Count() > 0)
            {
                return Ok (found);
            }

            return NotFound("No patients to show");
        }
    }
}
