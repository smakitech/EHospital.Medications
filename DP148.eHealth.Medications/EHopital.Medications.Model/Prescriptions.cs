using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHospital.Medications.Model
{
    public class Prescription
    {
        public long Id { get; set; }
        public int PatientId { get; set; }
        public int UserId { get; set; }
        public long DrugId { get; set; }
        [Column(TypeName = "date")]
        public DateTime AssignmentDate { get; set; }
        public short Duration { get; set; }
        public string Notes { get; set; }
        public bool IsFinished { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("DrugId")]
        [InverseProperty("Prescriptions")]
        public Drug Drug { get; set; }
        [ForeignKey("PatientId")]
        [InverseProperty("Prescriptions")]
        public PatientInfo Patient { get; set; }
    }
}
