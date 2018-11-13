using System;
using System.Collections.Generic;
using DP148.eHealth.API.Medications.Domain.DataAccess;

namespace DP148.eHealth.API.Medications.Domain.Managers
{
    /// <summary>
    /// Represents medications service
    /// providing requeired operations.
    /// </summary>
    /// <seealso cref="IMedicationsManager"/>
    public class MedicationsManager : IMedicationsManager
    {
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

        public bool Add(Models.Medications item)
        {
            throw new NotImplementedException();
        }

        public bool Delete(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Models.Medications> GetAll()
        {
            throw new NotImplementedException();
        }

        public Models.Medications GetById(long id)
        {
            throw new NotImplementedException();
        }

        public Models.Medications GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public bool Update(long id, Models.Medications item)
        {
            throw new NotImplementedException();
        }
    }
}
