using System;

namespace EHospital.Medications.Model
{
    /// <summary>
    /// Represents a extended user-friendly view of the prescription.
    /// Model contain additional information about prescribed drug,
    /// and doctor who has prescribed it.
    /// </summary>
    public class PrescriptionDetails : BaseEntity, ISoftDeletion
    {
        /// <summary>Gets or sets doctor's first name./// </summary>
        public string FirstName { get; set; }

        /// <summary>Gets or sets doctor's last name./// </summary>
        public string LastName { get; set; }

        /// <summary>Gets or sets drug name./// </summary>
        public string Name { get; set; }

        /// <summary>Gets or sets drug type./// </summary>
        public string Type { get; set; }

        /// <summary>Gets or sets drug dose./// </summary>
        public double Dose { get; set; }

        /// <summary>Gets or sets drug dose unit./// </summary>
        public string DoseUnit { get; set; }

        /// <summary>Gets or sets drug direction./// </summary>
        public string Direction { get; set; }

        /// <summary>
        /// Gets or sets a value indicating
        /// whether drug is deleted or not.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>Gets or sets the assignment date./// </summary>
        public DateTime AssignmentDate { get; set; }

        /// <summary>Gets or sets the prescription duration.</summary>
        public short Duration { get; set; }

        /// <summary>
        /// Gets or sets a value indicating
        /// whether this prescription is finished.
        /// </summary>
        public bool IsFinished { get; set; }
    }
}