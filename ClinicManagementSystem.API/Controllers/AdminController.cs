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

        public AdminController(IConfiguration config , IPasswordHandlerManager passwordHandlerManager)
        {
            _context = new(config);
            _passwordHandlerManager = passwordHandlerManager;
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
                throw new Exception("Doctor with this email address already exists");
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
            throw new Exception("Could not add Doctor");

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
                throw new Exception("No updates were made");
            }
            throw new Exception("User Does not exist!!");
        }

        //4th Method : Removing a Doctor
        [HttpDelete("RemoveDoc/{id}")]
        public IActionResult DeleteDoc(int id)
        {
            var checkUser = _context.ApplicationUsers.Where(i => i.id == id).FirstOrDefault();
            if (checkUser == null)
            {
                throw new Exception("Doctor does not exist");
            }
            var checkDoc = _context.Doctors.Where(i => i.userId == checkUser.id).FirstOrDefault();
            _context.Doctors.Remove(checkDoc);
            _context.ApplicationUsers.Remove(checkUser);
            if (_context.SaveChanges() > 0)
                return Ok();
            throw new Exception("Could not remove Doctor");
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
                    throw new Exception("Could not add Doctor Appointment");
                }
                throw new Exception("Doctor already assigned");
            }
            throw new Exception("Doctor does not exist");
        }

        //Appointment Handling : 

        [HttpGet("GetPendingAppointments")]
        public IEnumerable<Reservation> listRequestedAppointments()
        {
            var PendingReservations = _context.Reservations.Where(i => i.status == "Pending");
            return PendingReservations;
        }

        [HttpGet("GetConfirmedAppointments")]
        public IEnumerable<Reservation> listReservedAppointments()
        {
            var ConfirmedReservations = _context.Reservations.Where(i => i.status == "Reserved");
            return ConfirmedReservations;
        }



        // Method To confirm "Reserve" a Pending Appointment
        [HttpPut("AdminConfirmAppointment/{reservationId}")]
        public IActionResult ConfirmReservation(int reservationId)
        {
            var reservation = _context.Reservations.Where(i => i.id == reservationId).FirstOrDefault();
            if (reservation != null)
            {
                reservation.status = "Reserved";
                Patient patient = new();
                patient = _context.Patients.Where(a => a.userId == reservation.patientId).FirstOrDefault();
                var getDoc = _context.DoctorAppointments.Where(i => i.Id == reservation.appointmentId).FirstOrDefault();
                getDoc.status = "Booked";
                //patient.userId = reservation.patientId;
                patient.doctorId = getDoc.doctorId;
                _context.Patients.Update(patient);
                if (_context.SaveChanges() > 0)
                    return Ok();
                throw new Exception("Could not confirm Reservation");

            }
            throw new Exception("Reservation Not Found!!");
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
                    var DocPatient = _context.Patients.Where(i => i.doctorId == getDoc.doctorId && i.userId == reservation.patientId).FirstOrDefault();
                    _context.Patients.Remove(DocPatient);
                }
                _context.Reservations.Remove(reservation);
                if (_context.SaveChanges() > 0)
                    return Ok();
                throw new Exception("Could not Cancel Reservation");
            }
            throw new Exception("Reservation Not Found!!");
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
                reservation.appointmentId = newAppointmentId;
                if(_context.SaveChanges() > 0)
                    return Ok();
                throw new Exception("No Changes Made");
            }throw new Exception("Reservation not Found!!");
        }



        //This method returns the booked appointments of a Specific Doctor
        [HttpGet("DocReport/{docId}")]
        public IEnumerable<DocReportDTO> DoctorReport(int docId)
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
                return docReportDTO;
            }throw new Exception("No reserved appointments for this doctor"); 
        }
    }
}
