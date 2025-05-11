using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.DAL.Models
{
    public class ChatRoom
    {
        public int Id { get; set; }
        public int doctorId { get; set; }
        public Doctor? doctor { get; set; }
        public string Name { get; set; }
    }
}
