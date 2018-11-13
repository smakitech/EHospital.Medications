using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DP148.eHealth.API.Medications.Domain.Models
{
    public partial class MedicationsContext : DbContext
    {
        public MedicationsContext()
        {
        }

        public MedicationsContext(DbContextOptions<MedicationsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Images> Images { get; set; }
        public virtual DbSet<Medications> Medications { get; set; }
        public virtual DbSet<PatientInfo> PatientInfo { get; set; }
        public virtual DbSet<PatientMedications> PatientMedications { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-VNR4KA5\\MSQLEXPRESS2K16;Database=eHealthDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Images>(entity =>
            {
                entity.Property(e => e.ImageName).IsUnicode(false);
            });

            modelBuilder.Entity<Medications>(entity =>
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
