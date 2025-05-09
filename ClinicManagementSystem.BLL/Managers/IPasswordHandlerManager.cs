using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.BLL.Managers
{
    public interface IPasswordHandlerManager
    {
        public byte[] HashPasswordToByteArray(string password);
        public string DecodePasswordFromByteArray(byte[] passwordBytes);
        public bool VerifyPassword(string inputPassword, byte[] storedHashByteArray);



    }
}
