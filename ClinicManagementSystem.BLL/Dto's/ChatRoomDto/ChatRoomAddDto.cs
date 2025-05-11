using ClinicManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.BLL.Dto_s.ChatRoomDto
{
    public class ChatRoomAddDto
    {
        public int doctorId { get; set; }
        public string Name { get; set; }
    }
}
