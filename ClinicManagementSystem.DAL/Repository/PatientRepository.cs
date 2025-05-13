using ClinicManagementSystem.DAL.Database;
using ClinicManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.DAL.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private ProgramContext _context;

        public PatientRepository (ProgramContext context)
        {
            _context = context;
        }
        
        public void AddPatient(Patient patient)
        {
            _context.Patients.Add(patient);
            _context.SaveChanges();
        }

        public IQueryable<Patient> GetAll()
        {
            var found = _context.Patients.Include(a=>a.user);

            return found;
        }

        public Patient GetPatientById(int id)
        {
            var found = _context.Patients.Find(id);
            return found;
        }

        public IQueryable<DoctorPatient> GetSpecificDoctorPatients(int doctorId)
        {
            var found = _context.DoctorPatients
                                    .Where(a => a.DoctorId == doctorId)
                                    .Include(a => a.Patient.medicalHistory)
                                    .Include(a => a.Patient.user);
            return found;
        }

        public void PendingPlus(int patientId)
        {
            var found = _context.Patients.Find(patientId);
            if (found != null)
            {
                found.pendingAppointments += 1;
            }
        }

    }
}
