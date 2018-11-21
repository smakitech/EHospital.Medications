using Microsoft.EntityFrameworkCore;
using EHospital.Medications.Model;

namespace EHospital.Medications.Data
{
    public class MedicationDbContext : DbContext
    {
        public MedicationDbContext()
        {
        }

        public MedicationDbContext(DbContextOptions<MedicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Drug> Drugs { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<PatientInfo> PatientInfo { get; set; }
        public virtual DbSet<Prescription> Prescriptions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(this.Database.GetDbConnection().ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Drug>(entity =>
            {
                entity.HasIndex(e => new { e.Name, e.Type, e.Dose, e.DoseUnit })
                    .HasName("UC_Medications")
                    .IsUnique();

                entity.Property(e => e.DoseUnit).HasDefaultValueSql("('mg')");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.Property(e => e.ImageName).IsUnicode(false);
                entity.Property(e => e.PatientImage).HasColumnName("Image");
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

            modelBuilder.Entity<Prescription>(entity =>
            {
                entity.Property(e => e.AssignmentDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Drug)
                    .WithMany(p => p.Prescriptions)
                    .HasForeignKey(d => d.DrugId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PatientMedications_Medications");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.Prescriptions)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PatientMedications_PatientsInfo");
            });
        }
    }
}
