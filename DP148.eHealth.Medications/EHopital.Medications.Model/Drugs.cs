using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EHopital.Medications.Model;
using EHospital.Medications.Model;

namespace EHospital.Medications.Model
{
    public class Drug : BaseEntity
    {
        public Drug()
        {
            Prescriptions = new HashSet<Prescription>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
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

        [InverseProperty("Drug")]
        public ICollection<Prescription> Prescriptions { get; set; }
    }
}
