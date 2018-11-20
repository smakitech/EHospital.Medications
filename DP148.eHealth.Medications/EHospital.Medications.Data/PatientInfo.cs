using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHospital.Medications.Data
{
    public partial class PatientInfo
    {
        public PatientInfo()
        {
            Prescriptions = new HashSet<Prescription>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(50)]
        public string Country { get; set; }
        [StringLength(50)]
        public string City { get; set; }
        public string Address { get; set; }
        [Column(TypeName = "date")]
        public DateTime BirthDate { get; set; }
        [StringLength(12)]
        public string Phone { get; set; }
        public byte? Gender { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        public int? ImageId { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("ImageId")]
        [InverseProperty("PatientInfo")]
        public Image Image { get; set; }
        [InverseProperty("Patient")]
        public ICollection<Prescription> Prescriptions { get; set; }
    }
}
