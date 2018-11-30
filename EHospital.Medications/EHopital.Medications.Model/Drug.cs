using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHospital.Medications.Model
{
    /// <summary>
    /// Represents model of the
    /// Drugs table in the database.
    /// </summary>
    /// <seealso cref="BaseEntity"/>
    public class Drug : BaseEntity, ISoftDeletion
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Drug"/> class.
        /// </summary>
        public Drug()
        {
            this.Prescriptions = new HashSet<Prescription>();
        }

        /// <summary>Gets or sets the name.</summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>Gets or sets the type.</summary>
        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        /// <summary>Gets or sets the dose.</summary>
        [Required]
        [Range(1, float.MaxValue - 1)]
        public double Dose { get; set; }

        /// <summary>Gets or sets the dose unit.</summary>
        [Required]
        [StringLength(4)]
        public string DoseUnit { get; set; }

        /// <summary>Gets or sets the direction.</summary>
        [Required]
        [StringLength(50)]
        public string Direction { get; set; }

        /// <summary>Gets or sets the instruction.</summary>
        [Required]
        public string Instruction { get; set; }

        /// <summary>
        /// Gets or sets a value indicating
        /// whether drug is deleted.
        /// </summary>
        [Required]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the prescriptions navigation property.
        /// </summary>
        [InverseProperty("Drug")]
        public ICollection<Prescription> Prescriptions { get; set; }
    }
}