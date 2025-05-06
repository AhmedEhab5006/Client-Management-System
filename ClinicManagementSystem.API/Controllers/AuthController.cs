using ClinicManagementSystem.BLL.Dto_s;
using ClinicManagementSystem.BLL.Dto_s.AuthDto;
using ClinicManagementSystem.BLL.Managers.AuthManagers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthManager _authManager;
        public AuthController (IAuthManager authManager)
        {
            _authManager = authManager;
        }
        
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var done = await _authManager.Register(registerDto);

            if (done == "exsist")
            {
                return BadRequest("Email or username already taken");
            }

            if (done == null)
            {
                return BadRequest();
            }

            return Ok(done);
        }

        [HttpPost("Login")]
        public async Task <IActionResult> Login (LoginDto loginDto)
        {
            var logged = _authManager.Login(loginDto);
            if (logged == null)
            {
                return NotFound("Wrong email or password");
            }

            return Ok(logged);
        }
    }
}
