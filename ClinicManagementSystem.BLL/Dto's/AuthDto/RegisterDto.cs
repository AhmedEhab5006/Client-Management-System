using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.BLL.Dto_s.AuthDto
{
    public class RegisterDto
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        [AllowedValues("Administrator", "Patient", "Doctor")]
        [StringLength(100 , MinimumLength = 5)]
        public string role { get; set; }

    }
}
