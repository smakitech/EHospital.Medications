using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHospital.Medications.Model
{
    /// <summary>
    /// Represents model of the
    /// PatientInfo table in the database.
    /// </summary>
    /// <seealso cref="BaseEntity"/>
    public class PatientInfo : BaseEntity, ISoftDeletion
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PatientInfo"/> class.
        /// </summary>
        public PatientInfo()
        {
            this.Prescriptions = new HashSet<Prescription>();
        }

        /// <summary>Gets or sets the first name.</summary>
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        /// <summary>Gets or sets the last name.</summary>
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        /// <summary>Gets or sets the country.</summary>
        [StringLength(50)]
        public string Country { get; set; }

        /// <summary>Gets or sets the city.</summary>
        [StringLength(50)]
        public string City { get; set; }

        /// <summary>Gets or sets the address.</summary>
        public string Address { get; set; }

        /// <summary>Gets or sets the birth date.</summary>
        [Column(TypeName = "date")]
        public DateTime BirthDate { get; set; }

        /// <summary>Gets or sets the phone.</summary>
        [StringLength(12)]
        public string Phone { get; set; }

        /// <summary>Gets or sets the gender.</summary>
        public byte? Gender { get; set; }

        /// <summary>Gets or sets the email.</summary>
        [StringLength(50)]
        public string Email { get; set; }

        /// <summary>Gets or sets the image identifier.</summary>
        public int? ImageId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating
        /// whether patient info is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the Image navigation property.
        /// </summary>
        [ForeignKey("ImageId")]
        [InverseProperty("PatientInfo")]
        public Image Image { get; set; }

        /// <summary>
        /// Gets or sets the Prescriptions navigation property.
        /// </summary>
        [InverseProperty("Patient")]
        public ICollection<Prescription> Prescriptions { get; set; }
    }
}