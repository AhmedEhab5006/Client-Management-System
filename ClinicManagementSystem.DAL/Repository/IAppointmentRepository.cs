using ClinicManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.DAL.Repository
{
    public interface IAppointmentRepository
    {
        public IQueryable<DoctorAppointment> Get(int doctorId);
        public DoctorAppointment GetById(int id);

    }
}
