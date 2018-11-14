using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;
using DP148.eHealth.API.Medications.Domain.Models;
using System.Collections.Generic;

namespace DP148.eHealth.API.Medications.Domain.DataAccess
{
    public partial class MedicationsContext : DbContext, IMedicationsProvider, IPatientMedicationsProvider
    {
        public MedicationsContext()
        {
        }

        public MedicationsContext(DbContextOptions<MedicationsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Images> Images { get; set; }

        public virtual DbSet<Models.Medications> Medications { get; set; }

        public virtual DbSet<PatientInfo> PatientInfo { get; set; }

        public virtual DbSet<PatientMedications> PatientMedications { get; set; }

        public long AddMedication(Models.Medications item)
        {
            this.Medications.Add(item);
            this.SaveChanges();
            return this.Medications.Last<Models.Medications>().MedicationId;
        }

        public long AddPatientMedication(PatientMedications item)
        {
            this.PatientMedications.Add(item);
            this.SaveChanges();
            return this.PatientMedications.Last<PatientMedications>().PatientMedicationsId;
        }

        public bool ChangePatientMedicationStatus(long id)
        {
            PatientMedications target = this.PatientMedications.FirstOrDefault(pm => pm.PatientMedicationsId == id);
            if (target.IsFinished)
            {
                target.IsFinished = false;
            }
            else
            {
                target.IsFinished = true;
            }
            this.SaveChanges();
            return target.IsFinished;
        }

        public long DeleteMedication(long id)
        {
            this.Medications.FirstOrDefault(m => m.MedicationId == id).IsDeleted = true;
            this.SaveChanges();
            return id;
        }

        public long DeletePatientMedication(long id)
        {
            this.PatientMedications.FirstOrDefault(m => m.PatientMedicationsId == id).IsDeleted = true;
            this.SaveChanges();
            return id;
        }

        public Models.Medications GetMedicationById(long id)
        {
            return this.Medications.FirstOrDefault(m => m.MedicationId == id);
        }

        public IEnumerable<Models.Medications> GetMedicationsByName(string name)
        {
            return this.Medications.Where(m => m.InternationalName == name);
        }

        public IEnumerable<Models.Medications> GetMedications()
        {
            return this.Medications.ToList();
        }

        public PatientMedications GetPatientMedicationById(long id)
        {
            return this.PatientMedications.FirstOrDefault(m => m.PatientMedicationsId == id);
        }

        public IEnumerable<PatientMedications> GetPatientMedications()
        {
            return this.PatientMedications.ToList();
        }

        public long UpdateMedication(long id, Models.Medications item)
        {
            Models.Medications target = this.Medications.FirstOrDefault(m => m.MedicationId == id);
            target.Clone(item);
            this.SaveChanges();
            return target.MedicationId;
        }

        public long UpdatePatientMedication(long id, PatientMedications item)
        {
            Models.PatientMedications target = this.PatientMedications.FirstOrDefault(m => m.PatientMedicationsId == id);
            target.Clone(item);
            this.SaveChanges();
            return target.PatientMedicationsId;
        }

        bool IMedicationsProvider.IsIdentifierExists(long id)
        {
            return this.Medications.Any(m => m.MedicationId == id);
        }

        bool IPatientMedicationsProvider.IsIdentifierExists(long id)
        {
            return this.PatientMedications.Any(pm => pm.PatientMedicationsId == id);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // TODO: Set up config in api and pass constring via parameter
                // http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                // "Server=DESKTOP-VNR4KA5\\MSQLEXPRESS2K16;Database=eHealthDB;Trusted_Connection=True;"
                optionsBuilder.UseSqlServer(this.Database.GetDbConnection().ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Images>(entity =>
            {
                entity.Property(e => e.ImageName).IsUnicode(false);
            });

            modelBuilder.Entity<Models.Medications>(entity =>
            {
                entity.HasIndex(e => new { e.InternationalName, e.Type, e.Dose, e.DoseUnit })
                    .HasName("UC_Medications")
                    .IsUnique();

                entity.Property(e => e.DoseUnit).HasDefaultValueSql("('mg')");
            });

            modelBuilder.Entity<PatientInfo>(entity =>
            {
                entity.HasIndex(e => e.Email)
                    .HasName("UC_Email")
                    .IsUnique();

                entity.HasIndex(e => new { e.FirstName, e.LastName, e.BirthDate })
                    .HasName("UC_Person")
                    .IsUnique();

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.Phone).IsUnicode(false);

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.PatientInfo)
                    .HasForeignKey(d => d.ImageId)
                    .HasConstraintName("FK_Images_Id");
            });

            modelBuilder.Entity<PatientMedications>(entity =>
            {
                entity.Property(e => e.AssignmentDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Medication)
                    .WithMany(p => p.PatientMedications)
                    .HasForeignKey(d => d.MedicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PatientMedications_Medications");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.PatientMedications)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PatientMedications_PatientsInfo");
            });
        }
    }
}