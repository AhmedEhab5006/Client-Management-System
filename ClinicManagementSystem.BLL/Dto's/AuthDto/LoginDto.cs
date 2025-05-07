using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.BLL.Dto_s
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email is required")]
        public string email {  get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string password { get; set; }
    }
}
