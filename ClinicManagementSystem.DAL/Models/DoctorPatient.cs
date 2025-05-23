﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.DAL.Models
{
    public class DoctorPatient
    {
        public int id { get; set; }
        public int PatientId { get; set; }
        public int? DoctorId { get; set; }
        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
    }
}
