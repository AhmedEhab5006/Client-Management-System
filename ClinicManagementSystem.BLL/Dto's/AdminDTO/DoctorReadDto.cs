using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.BLL.Dto_s.AdminDTO
{
    public class DoctorReadDto
    {
        public int Id { get; set; }
        public string firstname {  get; set; }
        public string lastname { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string Address { get; set; }
        public string phone { get; set; }
        public string major { get; set; }
    }  
}
