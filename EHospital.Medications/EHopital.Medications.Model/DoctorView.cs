using System.ComponentModel.DataAnnotations;

namespace EHospital.Medications.Model
{
    /// <summary>
    /// Represents model of the
    /// DoctorsView view in the database.
    /// </summary>
    public class DoctorView : BaseEntity
    {
        // TODO: DataAnnotations for View or PairView

        /// <summary>Gets or sets the first name.</summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>Gets or sets the last name.</summary>
        [Required]
        public string LastName { get; set; }
    }
}