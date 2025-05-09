using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.BLL.Helpers
{
    public class GetLoggedData : IGetLoggedData
    {
        
        private IHttpContextAccessor _httpContextAccessor;

        public GetLoggedData(IHttpContextAccessor httpContextAccessor)
        {
              _httpContextAccessor = httpContextAccessor;
        }

        public int GetId()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var id = Convert.ToInt32(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            return id;

        }
    }
}
