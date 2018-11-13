using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DP148.eHealth.API.Medications.Domain.Models
{
    public partial class PatientInfo
    {
        public PatientInfo()
        {
            this.PatientMedications = new HashSet<PatientMedications>();
        }

        [Key]
        public int PatientId { get; set; }

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
        public Images Image { get; set; }

        [InverseProperty("Patient")]
        public ICollection<PatientMedications> PatientMedications { get; set; }
    }
}
