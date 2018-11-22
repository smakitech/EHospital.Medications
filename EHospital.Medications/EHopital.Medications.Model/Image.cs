using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EHospital.Medications.Model
{
    /// <summary>
    /// Represents model of the
    /// Images table in the database.
    /// </summary>
    public class Image
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        public Image()
        {
            this.PatientInfo = new HashSet<PatientInfo>();
        }

        /// <summary>Gets or sets the identifier.</summary>
        [Key]
        public int Id { get; set; }

        /// <summary>Gets or sets the name of the image.</summary>
        [StringLength(100)]
        public string ImageName { get; set; }

        /// <summary>Gets or sets the patient image.</summary>
        public byte[] PatientImage { get; set; }

        /// <summary>
        /// Gets or sets Patient navigation property.
        /// </summary>
        [InverseProperty("Image")]
        public ICollection<PatientInfo> PatientInfo { get; set; }
    }
}
