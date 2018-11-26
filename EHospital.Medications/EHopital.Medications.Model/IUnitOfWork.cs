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

        // TODO: Supplement methods

        /// <summary>
        /// Updates the prescription status to historic
        /// automatically using SQL store procedure.
        /// Method aimed to be used in get methods and as part of
        /// UpdateStatusManully method.
        /// </summary>
        /// <param name="id">The prescription identifier.</param>
        void UpdateStatusAutomatically(int id);

        /// <summary>
        /// Updates the prescription status to historic
        /// manually using SQL store procedure.
        /// </summary>
        /// <param name="id">The prescription identifier.</param>
        void UpdateStatusManually(int id);

        /// <summary>
        /// Save changes to database in asynchronous mode.
        /// </summary>
        /// <returns>Task object.</returns>
        Task Save();
    }
}