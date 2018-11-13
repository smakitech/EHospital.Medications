using System;
using System.Collections.Generic;
using System.Text;
using DP148.eHealth.API.Medications.Domain.Models;
using DP148.eHealth.API.Medications.Domain.DataAccess;

namespace DP148.eHealth.API.Medications.Domain.Managers
{
    public class PatientMedicationsManager : IPatientMedicationsManager
    {
        private const string ID_EXCEPTION = "Item with such id doesn't exist";
        private IPatientMedicationsProvider provider;

        public PatientMedicationsManager(IPatientMedicationsProvider provider)
        {
            this.provider = provider;
        }

        public long Add(PatientMedications item)
        {
            return this.provider.AddPatientMedication(item);
        }

        /// <summary>
        /// Changes the status of prescription.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// Current status.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Item with such id doesn't exist.
        /// </exception>
        /// TODO: Clerify logic.
        public bool ChangeStatus(long id)
        {
            if (this.provider.IsIdentifierExists(id))
            {
                return this.provider.ChangePatientMedicationStatus(id);
            }
            else
            {
                throw new ArgumentException(ID_EXCEPTION);
            }
        }

        /// <summary>
        /// Deletes the specified item by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Operation success.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Item with such id doesn't exist.
        /// </exception>
        public long Delete(long id)
        {
            if (this.provider.IsIdentifierExists(id))
            {
                return this.provider.DeletePatientMedication(id);
            }
            else
            {
                throw new ArgumentException(ID_EXCEPTION);
            }
        }

        public IEnumerable<PatientMedications> GetAll()
        {
            return this.provider.GetPatientMedications();
        }

        /// <summary>
        /// Gets the item by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Item.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Item with such id doesn't exist.
        /// </exception>
        public PatientMedications GetById(long id)
        {
            if (this.provider.IsIdentifierExists(id))
            {
                return this.provider.GetPatientMedicationById(id);
            }
            else
            {
                throw new ArgumentException(ID_EXCEPTION);
            }
        }

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="item">The item.</param>
        /// <returns>
        /// Operation success.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Item with such id doesn't exist.
        /// </exception>
        public long Update(long id, PatientMedications item)
        {
            if (this.provider.IsIdentifierExists(id))
            {
                return this.provider.UpdatePatientMedication(id, item);
            }
            else
            {
                throw new ArgumentException(ID_EXCEPTION);
            }
        }
    }
}
