using ClinicManagementSystem.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.DAL.Database
{
    public class ProgramContext : IdentityDbContext<ApplicationUser, ApplicationUserRole, string>
    {
        
    public ProgramContext(DbContextOptions<ProgramContext> options)
        : base(options) { }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure TPT
            builder.Entity<Doctor>().ToTable("Patients");
            builder.Entity<Doctor>().ToTable("Doctors");

            // Optional: map base table name
            builder.Entity<ApplicationUser>().ToTable("AspNetUsers");

            builder.Entity<ApplicationUserRole>().HasData(
            new IdentityRole { Name = "Administrator", NormalizedName = "ADMINISTRATOR" },
            new IdentityRole { Name = "Patient", NormalizedName = "PATIENT" },
            new IdentityRole { Name = "Doctor", NormalizedName = "DOCTOR" }
        );
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<MedicalHistory> MedicalHistories { get; set; }
    }

}

