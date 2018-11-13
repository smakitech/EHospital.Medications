using System.Collections.Generic;

namespace DP148.eHealth.API.Medications.Domain.Managers
{
    /// <summary>
    /// Provides prototypes of method
    /// to perform CRUD operations
    /// with Medications entiry.
    /// </summary>
    /// <seealso cref="IEntityManager{Models.Medications, long}"/>
    public interface IMedicationsManager : IEntityManager<Models.Medications, long>
    {
        /// <summary>
        /// Gets the medication record by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Medication record.</returns>
        IEnumerable<Models.Medications> GetByName(string name);
    }
}
