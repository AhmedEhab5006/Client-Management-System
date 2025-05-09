using ClinicManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.DAL.Repository
{
    public interface IMedicalHistoryRepository
    {
        public void AddMedicalHistory(MedicalHistory medicalHistory);
        public IQueryable<MedicalHistory> GetFullHistory(int patientId);
    }
}
