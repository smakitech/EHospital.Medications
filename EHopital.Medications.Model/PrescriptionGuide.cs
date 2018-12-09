namespace EHospital.Medications.Model
{
    /// <summary>
    /// Represents view of prescription,
    /// which contains drug instruction and doctor's notes.
    /// </summary>
    public class PrescriptionGuide
    {
        /// <summary>Gets or sets the instruction.</summary>
        public string Instruction { get; set; }

        /// <summary>Gets or sets the notes.</summary>
        public string Notes { get; set; }
    }
}