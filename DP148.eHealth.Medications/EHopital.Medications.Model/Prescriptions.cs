using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHospital.Medications.Model
{
    public class Prescription : BaseEntity
    {
        public int PatientId { get; set; }
        public int UserId { get; set; }
        public int DrugId { get; set; }
        [Column(TypeName = "date")]
        public DateTime AssignmentDate { get; set; }
        public short Duration { get; set; }
        public string Notes { get; set; }
        public bool IsFinished { get; set; }

        [ForeignKey("DrugId")]
        [InverseProperty("Prescriptions")]
        public Drug Drug { get; set; }
        [ForeignKey("PatientId")]
        [InverseProperty("Prescriptions")]
        public PatientInfo Patient { get; set; }
    }
}