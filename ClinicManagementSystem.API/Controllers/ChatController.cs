using ClinicManagementSystem.DAL.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ChatController : ControllerBase
    {
        private IChatRoomRepository _chatRoomRepository;

        public ChatController (IChatRoomRepository chatRoomRepository)
        {
            _chatRoomRepository = chatRoomRepository;
        }

        [HttpGet("GetChats")]
        public IActionResult GetAll()
        {
            var found = _chatRoomRepository.GetAllRooms();
            return Ok(found);
        }

    }
}
