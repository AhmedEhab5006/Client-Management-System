using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.BLL.Dto_s.AdminDTO
{
    public class EditDocDTO
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public byte[] password { get; set; }
        public string phoneNumber { get; set; }
        public string major { get; set; }
        public string location { get; set; }
    }
}
