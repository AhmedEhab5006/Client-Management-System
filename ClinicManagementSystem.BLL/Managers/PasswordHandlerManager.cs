using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.BLL.Managers
{
    public class PasswordHandlerManager : IPasswordHandlerManager
    {
        public byte[] HashPasswordToByteArray(string password)
        {

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(hashedPassword);

            return passwordBytes;
        }


        public string DecodePasswordFromByteArray(byte[] passwordBytes)
        {
            string decodedPassword = Encoding.UTF8.GetString(passwordBytes);
            return decodedPassword;
        }


        public bool VerifyPassword(string inputPassword, byte[] storedHashByteArray)
        {
            string storedHash = DecodePasswordFromByteArray(storedHashByteArray);
            return BCrypt.Net.BCrypt.Verify(inputPassword, storedHash);
        }
    }
}
