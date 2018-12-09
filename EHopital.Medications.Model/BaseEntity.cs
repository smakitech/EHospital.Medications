using System.ComponentModel.DataAnnotations;

namespace EHospital.Medications.Model
{
    /// <summary>
    /// Provides common property Id for entities,
    /// which represents unique identifier of the record in database.
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        [Key]
        public int Id { get; set; }
    }
}