using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicineProject.Models
{
    class MedDbContext : DbContext
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
    }
}
