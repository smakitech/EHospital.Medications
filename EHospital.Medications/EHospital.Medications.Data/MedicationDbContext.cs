using Microsoft.EntityFrameworkCore;
using EHospital.Medications.Model;

namespace EHospital.Medications.Data
{
    /// <summary>
    /// Represents database context for medication.
    /// Provide access to medications data.
    /// </summary>
    /// <seealso cref="DbContext"/>
    public class MedicationDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MedicationDbContext"/> class.
        /// </summary>
        public MedicationDbContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MedicationDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public MedicationDbContext(DbContextOptions<MedicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the drugs.
        /// Represents set of drugs which store in the database.
        /// Helps to interact with Drugs table placed in the database.
        /// </summary>
        public virtual DbSet<Drug> Drugs { get; set; }

        /// <summary>
        /// Gets or sets the prescriptions.
        /// Represents set of prescriptions which store in the database.
        /// Helps to interact with Prescriptions table placed in the database.
        /// </summary>
        public virtual DbSet<Prescription> Prescriptions { get; set; }

        /// <summary>
        /// Gets or sets the doctors.
        /// Represents set of doctors which store in the database.
        /// Helps to interact with DoctorsView view placed in the database.
        /// </summary>
        public virtual DbSet<DoctorView> DoctorsView { get; set; }

        /// <summary>
        /// Gets or sets the images.
        /// Represents set of drugs which store in the database.
        /// Helps to interact with Images table placed in the database.
        /// </summary>
        /// <remarks>
        /// Using only for mapping models and tables correctly.
        /// </remarks>
        public virtual DbSet<Image> Images { get; set; }

        /// <summary>
        /// Gets or sets the patient info.
        /// Represents set of patient information records which store in the database.
        /// Helps to interact with PatientInfo table placed in the database.
        /// </summary>
        /// <remarks>Using only for mapping models and tables correctly.</remarks>
        public virtual DbSet<PatientInfo> PatientInfo { get; set; }

        /// <summary>
        /// Defines configuration of the connection to database.
        /// </summary>
        /// <param name="optionsBuilder">
        /// A builder used to create or modify options for this context.
        /// </param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(this.Database.GetDbConnection().ConnectionString);
            }
        }

        /// <summary>
        /// Maps classes of models on database tables.
        /// </summary>
        /// <param name="modelBuilder">
        /// The builder being used to construct the model for this context
        /// </param>
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

            modelBuilder.Entity<DoctorView>(entity =>
            {
                // TODO: [DoctorView] Birthday Column to configure index?
                entity.ToTable("DoctorsView");
            });
        }
    }
}
