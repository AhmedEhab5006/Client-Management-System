using ClinicManagementSystem.BLL.Dto_s.AdminDTO;
using ClinicManagementSystem.DAL.Database;
using ClinicManagementSystem.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class AdminController : Controller
    {

        private readonly ProgramContext _context;
        public AdminController(IConfiguration config)
        {
            _context = new(config);
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
            user.password = AddDoc.password;
            user.phoneNumber = AddDoc.phoneNumber;
            user.role = "Doctor";
            var check = _context.ApplicationUsers.Where(e => e.email == AddDoc.email).ToList();
            if(check.Count != 0)
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
            var docUsers = _context.ApplicationUsers.ToList();
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
                editDoc.firstName = DocUser.firstName;
                editDoc.lastName = DocUser.lastName;
                editDoc.userName = DocUser.userName;
                editDoc.email = DocUser.email;
                editDoc.password = DocUser.password;
                editDoc.phoneNumber = DocUser.phoneNumber;
                editDoc.major = docProp.major;
                editDoc.location = docProp.location;
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
            if(_context.SaveChanges() > 0)
                return Ok();
            throw new Exception("Could not remove Doctor");
        }
    }
}
