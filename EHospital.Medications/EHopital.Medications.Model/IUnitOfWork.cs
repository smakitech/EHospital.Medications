using System;
using System.Threading.Tasks;
using System.Linq;

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

        // TODO: [Connect Change] UpdatePrescriptionStatus removed

        // TODO: [DoctorView] Define only what need
        IQueryable<DoctorView> GetDoctors();

        /// <summary>
        /// Save changes to database in asynchronous mode.
        /// </summary>
        /// <returns>Task object.</returns>
        Task Save();
    }
}