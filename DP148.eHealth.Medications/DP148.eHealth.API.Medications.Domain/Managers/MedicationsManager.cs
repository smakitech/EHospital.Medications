using System;
using System.Collections.Generic;
using DP148.eHealth.API.Medications.Domain.DataAccess;
using DP148.eHealth.API.Medications.Domain.Models;

namespace DP148.eHealth.API.Medications.Domain.Managers
{
    /// <summary>
    /// Represents medications service
    /// providing requeired operations.
    /// </summary>
    /// <seealso cref="IMedicationsManager"/>
    public class MedicationsManager : IMedicationsManager
    {
        private const string ID_EXCEPTION = "Item with such id doesn't exist";
        private IMedicationsProvider provider;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MedicationsManager"/> class.
        /// </summary>
        /// <param name="provider">
        /// The data provider.
        /// </param>
        public MedicationsManager(IMedicationsProvider provider)
        {
            this.provider = provider;
        }

        public long Add(Models.Medications item)
        {
            return this.provider.AddMedication(item);
        }

        public long Delete(long id)
        {
            if (this.provider.IsIdentifierExists(id))
            {
                return this.provider.DeleteMedication(id);
            }
            else
            {
                throw new ArgumentException(ID_EXCEPTION);
            }
        }

        public IEnumerable<Models.Medications> GetAll()
        {
            return this.provider.GetMedications();
        }

        public Models.Medications GetById(long id)
        {
            if (this.provider.IsIdentifierExists(id))
            {
                return this.provider.GetMedicationById(id);
            }
            else
            {
                throw new ArgumentException(ID_EXCEPTION);
            }
        }

        public IEnumerable<Models.Medications> GetByName(string name)
        {
            return this.provider.GetMedicationsByName(name);
        }

        public long Update(long id, Models.Medications item)
        {
            if (this.provider.IsIdentifierExists(id))
            {
                return this.provider.UpdateMedication(id, item);
            }
            else
            {
                throw new ArgumentException(ID_EXCEPTION);
            }
        }
    }
}
