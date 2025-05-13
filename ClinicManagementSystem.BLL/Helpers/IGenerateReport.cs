using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.BLL.Helpers
{
    public interface IGenerateReport
    {
        public Task<byte[]> ExportApiResponseToPdfAsync(string apiUrl, string token);
    }
}
