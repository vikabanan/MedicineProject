using Microsoft.EntityFrameworkCore;
using MedicineProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicineProject.Models
{
    public class MedDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Patient> Patients { get; set; } = null!;
        public DbSet<MedicalCard> MedicalCards { get; set; } = null!;
        public DbSet<Visit> Visits { get; set; } = null!;
        public DbSet<Referral> Referrals { get; set; } = null!;
        public DbSet<LabResult> LabResults { get; set; } = null!;

        private string _dbPath;

        public MedDbContext()
        {
            // БД в папке приложения:
            var folder = AppDomain.CurrentDomain.BaseDirectory;
            _dbPath = System.IO.Path.Combine(folder, "med.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // если нужно — дополнительные настройки
            modelBuilder.Entity<Patient>()
                .HasOne(p => p.MedicalCard)
                .WithOne(m => m.Patient)
                .HasForeignKey<MedicalCard>(m => m.PatientId);

            modelBuilder.Entity<Visit>()
                .HasMany(v => v.Referrals)
                .WithOne(r => r.Visit)
                .HasForeignKey(r => r.VisitId);

            modelBuilder.Entity<Referral>()
                .HasMany(r => r.LabResults)
                .WithOne(l => l.Referral)
                .HasForeignKey(l => l.ReferralId);
        }

        public void Seed()
        {
            // Добавляем начальных пользователей, если их нет
            if (!Users.Any())
            {
                var doctor = new User
                {
                    Login = "doctor",
                    PasswordHash = PasswordHasher.HashPassword("doctor123"),
                    FullName = "Иванов Иван Иванович",
                    Role = "Doctor"
                };
                Users.Add(doctor);

                var laborant = new User
                {
                    Login = "laborant",
                    PasswordHash = PasswordHasher.HashPassword("laborant123"),
                    FullName = "Петрова Мария Сергеевна",
                    Role = "Laborant"
                };
                Users.Add(laborant);

                SaveChanges();
            }

            if (!Patients.Any())
            {
                var patient = new Patient
                {
                    FullName = "Иванов Иван Иванович",
                    BirthDate = new DateTime(1985, 3, 15),
                    Gender = "М",
                    Address = "г. Оренбург, ул. Примерная, 5",
                    Phone = "+7 900 123-45-67",
                    Insurance = "1234 5678 9012 3456",
                    SNILS = "123-456-789 00",
                    Passport = "4500 123456"
                };
                Patients.Add(patient);
                SaveChanges();

                MedicalCards.Add(new MedicalCard
                {
                    PatientId = patient.Id,
                    BloodGroup = "A(II) Rh+",
                    ChronicDiseases = "Нет"
                });
                SaveChanges();

                var visit = new Visit
                {
                    PatientId = patient.Id,
                    VisitDate = DateTime.Now.AddDays(-10),
                    Diagnosis = "ОРВИ",
                    Prescriptions = "Пить горячее, парацетамол"
                };
                Visits.Add(visit);
                SaveChanges();

                var referral = new Referral
                {
                    VisitId = visit.Id,
                    ReferralType = "Анализ крови",
                    ReferralDate = DateTime.Now.AddDays(-9),
                    Status = ReferralStatus.New

                };
                Referrals.Add(referral);
                SaveChanges();
            }
        }


    }
}
