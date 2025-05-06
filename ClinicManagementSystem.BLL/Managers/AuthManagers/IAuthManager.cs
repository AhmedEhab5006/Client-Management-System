using ClinicManagementSystem.BLL.Dto_s;
using ClinicManagementSystem.BLL.Dto_s.AuthDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.BLL.Managers.AuthManagers
{
    public interface IAuthManager
    {
        public Task <string> Login(LoginDto loginDto);
        public Task<string> Register(RegisterDto registerDto);
        public string GenerateToken(IList<Claim> claims);
    }
}
