using System;
using System.Linq;
using System.Threading.Tasks;

namespace EHospital.Medications.Model
{
    /// <summary>
    /// Represents unit of work interface.
    /// </summary>
    /// <seealso cref="IDisposable"/>
    public interface IUnitOfWork : IDisposable
    {
        /// TODO: IUnitOfWork Help Methods
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

        /// <summary>
        /// Gets the doctors in asynchronous mode.
        /// </summary>
        /// <returns>Set of doctors.</returns>
        Task<IQueryable<DoctorView>> GetAllDoctorsAsync();

        /// <summary>
        /// Updates the prescription status to historic
        /// automatically using SQL store procedure.
        /// Method aimed to be used in get methods and as part of
        /// UpdateStatusManully method.
        /// </summary>
        /// <returns>Task object.</returns>
        Task UpdateStatusAutomatically();

        /// <summary>
        /// Updates the prescription status to historic
        /// manually using SQL store procedure.
        /// </summary>
        /// <param name="id">The prescription identifier.</param>
        /// <returns>Task object.</returns>
        Task UpdateStatusManually(int id);

        /// <summary>
        /// Save changes to database in asynchronous mode.
        /// </summary>
        /// <returns>Task object.</returns>
        Task Save();
    }
}