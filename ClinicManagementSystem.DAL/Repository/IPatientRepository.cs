using ClinicManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ClinicManagementSystem.DAL.Repository
{
    public interface IPatientRepository
    {
        public IQueryable<Patient> GetAll();
        public IQueryable<DoctorPatient> GetSpecificDoctorPatients(int doctorId);
        public void AddPatient (Patient patient);
        public Patient GetPatientById (int id);
        public void PendingPlus(int patientId);


    }
}
