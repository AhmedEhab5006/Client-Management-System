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
        private IConfiguration _configuration;
        private IPatientRepository _patientRepostiory;

        public AuthManager(IUserRepository userRepository , IConfiguration configuration , IPatientRepository patientRepository)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _patientRepostiory = patientRepository;
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
                return "Email not found";
            }

            var hashed = await _userRepository.GetPassword(logged.email);
            if (VerifyPassword(loginDto.password , hashed)){
                
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, logged.id.ToString()));
                claims.Add(new Claim(ClaimTypes.Name, logged.userName));
                claims.Add(new Claim(ClaimTypes.Email, logged.email));
                claims.Add(new Claim(ClaimTypes.Role, logged.role));

                var token = GenerateToken(claims);
               
                return token;
            }

            return "Wrong Password";

        }

        public async Task<string> Register(RegisterDto registerDto)
        {

            var addedUser = new ApplicationUser
            {
                email = registerDto.email,
                userName = registerDto.username,
                firstName = registerDto.firstname,
                lastName = registerDto.lastname,
                phoneNumber = registerDto.phoneNumber
            };

            var emailExists = _userRepository.GetByEmail(addedUser.email);
            var userNameExists = _userRepository.GetByUserName(addedUser.userName);

            if (emailExists != null || userNameExists != null)
            {
                return "exsist";
            }

            addedUser.password = HashPasswordToByteArray(registerDto.password);

            var addingResult = await _userRepository.AddAsync(addedUser);
            _patientRepostiory.AddPatient(new Patient
            {
                userId = addingResult
            });

            if (addingResult != 0)
            {
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim("UserName", registerDto.username));
                claims.Add(new Claim("Email", registerDto.email));
                claims.Add(new Claim("Password", registerDto.password));
                claims.Add(new Claim("Role", addedUser.role));

                var token = GenerateToken(claims);
                return token;

            }

            return null;
        }

        public static byte[] HashPasswordToByteArray(string password)
        {
            
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(hashedPassword);

            return passwordBytes;
        }


        public static string DecodePasswordFromByteArray(byte[] passwordBytes)
        {
            string decodedPassword = Encoding.UTF8.GetString(passwordBytes);
            return decodedPassword;
        }


        public static bool VerifyPassword(string inputPassword, byte[] storedHashByteArray)
        {
            string storedHash = DecodePasswordFromByteArray(storedHashByteArray);
            return BCrypt.Net.BCrypt.Verify(inputPassword, storedHash);
        }
    }
}
