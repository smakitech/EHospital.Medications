using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DP148.eHealth.API.Medications.Domain.Models
{
    public partial class Medications : IClone<Models.Medications>
    {
        public Medications()
        {
            this.PatientMedications = new HashSet<PatientMedications>();
        }

        [Key]
        public long MedicationId { get; set; }

        [Required]
        [StringLength(50)]
        public string InternationalName { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        [Range(1, float.MaxValue - 1)]
        public double Dose { get; set; }

        [Required]
        [StringLength(4)]
        public string DoseUnit { get; set; }

        [Required]
        [StringLength(50)]
        public string Direction { get; set; }

        [Required]
        public string Instruction { get; set; }

        public bool IsDeleted { get; set; }

        [InverseProperty("Medication")]
        public ICollection<PatientMedications> PatientMedications { get; set; }

        public void Clone(Medications source)
        {
            this.InternationalName = source.InternationalName;
            this.Type = source.Type;
            this.Dose = source.Dose;
            this.DoseUnit = source.DoseUnit;
            this.Direction = source.Direction;
            this.Instruction = source.Instruction;
            this.IsDeleted = source.IsDeleted;
        }
    }
}
