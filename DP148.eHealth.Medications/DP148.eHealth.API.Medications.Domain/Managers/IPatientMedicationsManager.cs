using DP148.eHealth.API.Medications.Domain.Models;

namespace DP148.eHealth.API.Medications.Domain.Managers
{
    /// <summary>
    /// Provides prototypes of method
    /// to perform CRUD operations
    /// with PatientMedications entiry.
    /// </summary>
    /// <seealso cref="IEntityManager{PatientMedications, long}"/>
    public interface IPatientMedicationsManager : IEntityManager<PatientMedications, long>
    {
        /// TODO: Clerify logic.
        /// <summary>
        /// Changes the status of prescription.
        /// </summary>
        /// <returns>Current status.</returns>
        bool ChangeStatus(long id);
    }
}
