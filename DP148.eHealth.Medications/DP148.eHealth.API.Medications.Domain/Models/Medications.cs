using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DP148.eHealth.API.Medications.Domain.Models
{
    public partial class Medications
    {
        public Medications()
        {
            PatientMedications = new HashSet<PatientMedications>();
        }

        [Key]
        public long MedicationId { get; set; }
        [Required]
        [StringLength(50)]
        public string InternationalName { get; set; }
        [Required]
        [StringLength(50)]
        public string Type { get; set; }
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
    }
}
