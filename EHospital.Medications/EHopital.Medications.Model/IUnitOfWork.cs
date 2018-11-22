using System;
using System.Threading.Tasks;

namespace EHospital.Medications.Model
{
    /// <summary>
    /// Represents unit of work interface.
    /// </summary>
    /// <seealso cref="IDisposable"/>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the drugs.
        /// Provides access to drug repository functionality.
        /// </summary>
        IRepository<Drug> Drugs { get; }

        /// <summary>
        /// Gets the prescriptions.
        /// Provides access to prescription repository functionality.
        /// </summary>
        IRepository<Prescription> Prescriptions { get; }

        // TODO: change logic and add documentation
        bool UpdatePrescriptionStatus(int id);

        /// <summary>
        /// Save changes to database in asynchronous mode.
        /// </summary>
        /// <returns>Task object.</returns>
        Task Save();
    }
}
