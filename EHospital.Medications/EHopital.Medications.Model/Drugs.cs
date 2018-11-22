using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHospital.Medications.Model
{
    public class Drug : BaseEntity
    {
        public Drug()
        {
            Prescriptions = new HashSet<Prescription>();
        }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
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

        [InverseProperty("Drug")]
        public ICollection<Prescription> Prescriptions { get; set; }
    }
}
