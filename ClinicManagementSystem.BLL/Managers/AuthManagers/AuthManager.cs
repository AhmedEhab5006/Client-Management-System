using ClinicManagementSystem.BLL.Dto_s;
using ClinicManagementSystem.BLL.Dto_s.AuthDto;
using ClinicManagementSystem.DAL.Models;
using ClinicManagementSystem.DAL.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.BLL.Managers.AuthManagers
{
    public class AuthManager : IAuthManager
    {
        private IUserRepository _userRepository;
        private IApplicationUserRoleRepository _applicationUserRoleRepository;
        private IConfiguration _configuration;

        public AuthManager(IUserRepository userRepository , IApplicationUserRoleRepository applicationUserRoleRepository , IConfiguration configuration) {
            _userRepository = userRepository;
            _applicationUserRoleRepository = applicationUserRoleRepository;
            _configuration = configuration;
        }
        
        public string GenerateToken(IList<Claim> claims)
        {
            var secretkey = _configuration.GetSection("SecretKey").Value;
            var byteSecretKey = Encoding.UTF8.GetBytes(secretkey);
            SecurityKey securityKey = new SymmetricSecurityKey(byteSecretKey);
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expiryDate = DateTime.Now.AddDays(5);
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(claims: claims, expires: expiryDate, signingCredentials: signingCredentials);
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var token = handler.WriteToken(jwtSecurityToken);
            
            return token;
        }

        public async Task<string> Login(LoginDto loginDto)
        {
            var logged = await _userRepository.GetByEmail(loginDto.email);

            if (logged == null)
            {
                return null;
            }

            var check = await _userRepository.CheckPassword(loginDto.password, logged);

            if (check == null)
            {
                return null;
            }

            var claims = await _userRepository.GetClaims(logged);
            claims.Add(new Claim(ClaimTypes.NameIdentifier, logged.Id));
            claims.Add(new Claim(ClaimTypes.Name, logged.UserName));
            claims.Add(new Claim(ClaimTypes.Email, logged.Email));

            var token = GenerateToken(claims);

            return token;

        }

        public async Task<string> Register(RegisterDto registerDto)
        {

            var addedUser = new ApplicationUser
            {
                Email = registerDto.email,
                UserName = registerDto.username
            };

            var emailExists = await _userRepository.GetByEmail(addedUser.Email);
            var userNameExists = await _userRepository.GetByUserName(addedUser.UserName);

            if(emailExists != null || userNameExists != null)
            {
                return "exsist";
            }

            var addingResult = await _userRepository.AddAsync(registerDto.password, addedUser);
            var addingRoleResult = await _applicationUserRoleRepository.Add(registerDto.role, addedUser);

            if (addingResult == "done" && addingRoleResult == "done")
            {
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim("UserName", registerDto.username));
                claims.Add(new Claim("Email", registerDto.email));
                claims.Add(new Claim("Password", registerDto.password));
                claims.Add(new Claim("Role", registerDto.role));

                var token = GenerateToken(claims);
                return token;

            }

            return null;
        }
    }
}
