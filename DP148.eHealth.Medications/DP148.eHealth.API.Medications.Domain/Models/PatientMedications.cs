using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DP148.eHealth.API.Medications.Domain.Models
{
    public partial class PatientMedications : IClone<PatientMedications>
    {
        public long PatientMedicationsId { get; set; }

        public int PatientId { get; set; }

        public int UserId { get; set; }

        public long MedicationId { get; set; }

        [Column(TypeName = "date")]
        public DateTime AssignmentDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Duration { get; set; }

        public string Notes { get; set; }

        public bool IsFinished { get; set; }

        public bool IsDeleted { get; set; }

        [ForeignKey("MedicationId")]
        [InverseProperty("PatientMedications")]
        public Medications Medication { get; set; }

        [ForeignKey("PatientId")]
        [InverseProperty("PatientMedications")]
        public PatientInfo Patient { get; set; }

        public void Clone(PatientMedications source)
        {
            this.PatientId = source.PatientId;
            this.UserId = source.UserId;
            this.MedicationId = source.MedicationId;
            this.AssignmentDate = source.AssignmentDate;
            this.Duration = source.Duration;
            this.Notes = source.Notes;
            this.IsDeleted = this.IsDeleted;
            this.IsFinished = this.IsFinished;
        }
    }
}