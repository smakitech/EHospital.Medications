namespace EHospital.Medications.Model
{
    /// <summary>
    /// Provide flag property for entities
    /// which suppose to support soft deletion of the records.
    /// </summary>
    public interface ISoftDeletion
    {
        /// <summary>
        /// Gets or sets a value indicating
        /// whether concrete entity is deleted.
        /// </summary>
        bool IsDeleted { get; set; }
    }
}