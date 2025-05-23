﻿using ClinicManagementSystem.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.DAL.Database
{
    public class ProgramContext : DbContext  {
        private readonly IConfiguration _config;
        public ProgramContext(IConfiguration config) 
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultConnection"),
                    optionsBuilder => optionsBuilder.EnableRetryOnFailure());
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword("admin");
            byte[] passwordBytes = Encoding.UTF8.GetBytes(hashedPassword);
            var adminUser = new ApplicationUser
            {
                
                id = 1,
                firstName = "Admin",
                lastName = "1",
                userName = "admin",
                email = "admin@gmail.com",
                role = "Admin",
                phoneNumber = "1234567890",
                password = passwordBytes
            };

            builder.Entity<ApplicationUser>().HasData(adminUser);

            builder.Entity<ApplicationUser>()
                .Property(u => u.role)
                 .HasDefaultValue("Patient");


            builder.Entity<Doctor>()
                .HasKey(p => p.userId);

            builder.Entity<Doctor>()
                .HasOne(p => p.user)
                .WithOne(u => u.doctor)
                .HasForeignKey<Doctor>(p => p.userId);

            //builder.Entity<Doctor>()
            //    .HasMany(d => d.patients)
            //    .WithOne(p => p.doctor)
            //    .HasForeignKey(p => p.doctorId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<Patient>()
            //    .HasOne(a => a.doctor)
            //    .WithMany(d => d.patients)
            //    .HasForeignKey(a => a.doctorId)
            //    .OnDelete(DeleteBehavior.Restrict);




            // Patient ↔ ApplicationUser (One-to-One)
            builder.Entity<Patient>()
                .HasKey(p => p.userId);

            builder.Entity<Patient>()
                .HasOne(p => p.user)
                .WithOne(u => u.patient)
                .HasForeignKey<Patient>(p => p.userId);

            // Patient ↔ MedicalHistory (One-to-Many with Cascade Delete)
            builder.Entity<Patient>()
                .HasMany(p => p.medicalHistory)
                .WithOne(m => m.patient)
                .HasForeignKey(m => m.patientId)
                .OnDelete(DeleteBehavior.Restrict); // Keep cascade here

            // Patient ↔ Reservation (One-to-Many with Restrict to avoid cascade conflict)
            //builder.Entity<Patient>()
            //    .HasOne(r => r.appointment)
            //    .WithOne(p => p.patient)
            //    .HasForeignKey<Reservation>(r => r.patientId)
            //    .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<DoctorAppointment>()
                .Property(p => p.duration)
                .HasDefaultValue(new TimeSpan(2, 0, 0));

            builder.Entity<DoctorPatient>()
                .HasKey(dp => dp.id); // Primary key

            builder.Entity<DoctorPatient>()
                .Property(dp => dp.id)
                .ValueGeneratedOnAdd(); // Makes it an identity column

            builder.Entity<DoctorPatient>()
                .HasOne(dp => dp.Patient)
                .WithMany(p => p.DoctorPatients)
                .HasForeignKey(dp => dp.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<DoctorPatient>()
                .HasOne(dp => dp.Doctor)
                .WithMany(d => d.DoctorPatients)
                .HasForeignKey(dp => dp.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Reservation>()
                .HasOne(r => r.patient)
                .WithMany(p => p.reservations)
                .HasForeignKey(r => r.patientId)
                .OnDelete(DeleteBehavior.Restrict);


        }

        public DbSet<DoctorAppointment> DoctorAppointments { get; set; }
        public DbSet<MedicalHistory> MedicalHistories { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<DoctorPatient> DoctorPatients { get; set; }
    }

}

