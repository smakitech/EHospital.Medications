using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHospital.Medications.Model
{
    /// <summary>
    /// Represents model of the
    /// Prescriptions table in the database.
    /// </summary>
    /// <seealso cref="BaseEntity"/>
    public class Prescription : BaseEntity, ISoftDeletion
    {
        /// <summary>Gets or sets the patient identifier.</summary>
        [Required]
        public int PatientId { get; set; }

        /// <summary>Gets or sets the user identifier.</summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>Gets or sets the drug identifier.</summary>
        [Required]
        public int DrugId { get; set; }

        /// <summary>Gets or sets the assignment date.</summary>
        [Column(TypeName = "date")]
        [Required]
        public DateTime AssignmentDate { get; set; }

        /// <summary>Gets or sets the duration.</summary>
        [Required]
        public short Duration { get; set; }

        /// <summary>Gets or sets the notes.</summary>
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating
        /// whether this prescription is finished.
        /// </summary>
        [Required]
        public bool IsFinished { get; set; }

        /// <summary>
        /// Gets or sets a value indicating
        /// whether prescription is deleted.
        /// </summary>
        [Required]
        public bool IsDeleted { get; set; }

        /// <summary>Gets or sets the Drug navigation property.</summary>
        [ForeignKey("DrugId")]
        [InverseProperty("Prescriptions")]
        public Drug Drug { get; set; }

        /// <summary>
        /// Gets or sets the Patient navigation property.
        /// </summary>
        [ForeignKey("PatientId")]
        [InverseProperty("Prescriptions")]
        public PatientInfo Patient { get; set; }
    }
}