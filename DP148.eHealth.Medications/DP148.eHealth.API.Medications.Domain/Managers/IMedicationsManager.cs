using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DP148.eHealth.API.Medications.Domain.Models;

namespace DP148.eHealth.API.Medications.Domain.Managers
{
    /// <summary>
    /// Provides prototypes of method
    /// to perform CRUD operations
    /// with Medications entiry.
    /// </summary>
    /// <seealso cref="DP148.eHealth.API.Medications.Domain.Managers.IEntityManager{DP148.eHealth.API.Medications.Domain.Models.Medications, System.Int64}" />
    public interface IMedicationsManager : IEntityManager<Models.Medications, long>
    {
        /// <summary>
        /// Gets the medication record by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Medication record.</returns>
        Models.Medications GetByName(string name);
    }
}
