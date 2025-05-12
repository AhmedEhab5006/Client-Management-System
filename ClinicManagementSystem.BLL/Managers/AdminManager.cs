using ClinicManagementSystem.BLL.Dto_s.AdminDTO;
using ClinicManagementSystem.BLL.Dto_s.PatientDto_s;
using ClinicManagementSystem.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.BLL.Managers
{
    public class AdminManager : IAdminManager
    {
        private IDoctorRepository _doctorRepository;
        private IPasswordHandlerManager _passwordHadnlerManager;

        public AdminManager(IDoctorRepository doctorRepository , IPasswordHandlerManager passwordHandlerManager) {
            _doctorRepository = doctorRepository;
            _passwordHadnlerManager = passwordHandlerManager;
        }

        public DoctorReadDto GetDocById(int id)
        {
            var foundModel = _doctorRepository.GetById(id);
            if (foundModel != null)
            {
                var foundPassword = _passwordHadnlerManager.DecodePasswordFromByteArray(foundModel.password);
                var found = new DoctorReadDto { 
                    Id = foundModel.id,
                    firstName = foundModel.firstName,
                    lastName = foundModel.lastName,
                    userName = foundModel.userName,
                    email = foundModel.email,
                    location = foundModel.doctor.location,
                    major = foundModel.doctor.major,
                    phoneNumber = foundModel.phoneNumber,
                    password = foundPassword,
                };

                return found;
            }

            return null;
        }
    }
}
