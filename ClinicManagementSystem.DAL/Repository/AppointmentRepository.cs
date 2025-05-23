﻿using ClinicManagementSystem.DAL.Database;
using ClinicManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.DAL.Repository
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private ProgramContext _context;

        public AppointmentRepository(ProgramContext context) {
            _context = context;
        }

        public IQueryable<DoctorAppointment> Get(int doctorId)
        {
            var found = _context.DoctorAppointments.Where(a=>a.doctorId == doctorId);
            return found;
        }


        public DoctorAppointment GetById(int id)
        {
            var found = _context.DoctorAppointments.FirstOrDefault(a => a.Id == id);
            return found;
        }
    }
}
