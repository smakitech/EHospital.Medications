using System.Threading.Tasks;
using System.Linq;
using EHospital.Medications.Model;

namespace EHospital.Medications.BusinessLogic.Contracts
{
    /// <summary>
    /// Provides CRUD operations methods for prescription service.
    /// </summary>
    /// <seealso cref="IService{Prescription}"/>
    public interface IPrescriptionService : IService<Prescription>
    {
        /// <summary>
        /// Gets all prescription details in asynchronous.
        /// Includes doctor and drug extended details.
        /// </summary>
        /// <returns>All patient prescriptions in details.</returns>
        Task<IQueryable<PrescriptionDetails>> GetAllDetailsAsync();

        /// <summary>
        /// Gets the guide by identifier in asynchronous mode.
        /// Includes drug instruction and doctor's notes.
        /// </summary>
        /// <param name="id">The prescription identifier.</param>
        /// <returns>Drug instruction and doctor's notes.</returns>
        Task<PrescriptionGuide> GetGuideByIdAsync(int id);

        /// <summary>
        /// Allows to update prescription status manually to historic.
        /// </summary>
        /// <param name="id">The prescription identifier.</param>
        /// <returns>Historic prescription.</returns>
        Task<Prescription> UpdateStatus(int id);
    }
}