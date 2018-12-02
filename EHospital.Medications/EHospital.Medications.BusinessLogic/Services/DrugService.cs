using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EHospital.Medications.BusinessLogic.Contracts;
using EHospital.Medications.Model;

namespace EHospital.Medications.BusinessLogic.Services
{
    /// TODO: DrugService documentation
    /// TODO: Remove previous service version
    /// <summary>Represents drug service.</summary>
    /// <seealso cref="IDrugService"/>
    public class DrugService : IDrugService
    {
        /// <summary>
        /// Exception message in case drug with specified identifier isn't found.
        /// </summary>
        private const string DRUG_IS_NOT_FOUND = "No drug with such id.";

        /// <summary>
        /// Exception message in case any drug was found in the database.
        /// </summary>
        private const string DRUGS_ARE_NOT_FOUND = "No drugs found.";

        /// <summary>
        /// Combine usage of unit of work and repository pattern.
        /// It contain repositories for each entity
        /// and one common database context in order
        /// to access data and perform CRUD operation.
        /// Also contains required supplement operations
        /// which are not related to repositories.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrugService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public DrugService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Drug> AddAsync(Drug item)
        {
            // TODO: AddDrug Tricky
            Drug result = this.unitOfWork.Drugs.Insert(item);
            await this.unitOfWork.Save();
            return result;
        }

        /// <summary>
        /// Deletes entity in asynchronous mode.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Deleted entity.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// No drug with such id.
        /// </exception>
        public async Task<Drug> DeleteAsync(int id)
        {
            Drug result = await this.unitOfWork.Drugs.DeleteAsync(id);
            if (result == null)
            {
                throw new ArgumentException(DRUG_IS_NOT_FOUND);
            }

            await this.unitOfWork.Save();
            return result;
        }

        /// <summary>
        /// Gets all drugs in asynchronous mode.
        /// </summary>
        /// <returns>
        /// Drugs.
        /// </returns>
        /// <exception cref="NoContentException">
        /// No drugs found.
        /// </exception>
        public async Task<IEnumerable<Drug>> GetAllAsync()
        {
            IEnumerable<Drug> result = await this.unitOfWork.Drugs.GetAllAsync(d => d.IsDeleted == false);
            if (result.Count() == 0)
            {
                throw new NoContentException(DRUGS_ARE_NOT_FOUND);
            }

            return result.OrderBy(d => d.Name);
        }

        /// <summary>
        /// Gets all drugs by specified name
        /// in asynchronous mode.
        /// </summary>
        /// <param name="name">Specified name.</param>
        /// <returns>
        /// Drugs.
        /// </returns>
        /// <exception cref="NoContentException">
        /// No drugs found.
        /// </exception>
        public async Task<IEnumerable<Drug>> GetAllByNameAsync(string name)
        {
            IEnumerable<Drug> result = await this.unitOfWork.Drugs.GetAllAsync(d => d.Name == name && d.IsDeleted == false);
            if (result.Count() == 0)
            {
                throw new NoContentException(DRUGS_ARE_NOT_FOUND);
            }

            return result.OrderBy(d => d.Name);
        }

        /// <summary>
        /// Gets entity by the identifier in asynchronous mode.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Concrete entity.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// No drug with such id.
        /// </exception>
        public async Task<Drug> GetByIdAsync(int id)
        {
            Drug result = await this.unitOfWork.Drugs.GetAsync(id);
            if (result == null)
            {
                throw new ArgumentException(DRUG_IS_NOT_FOUND);
            }

            return result;
        }

        public Task<Drug> UpdateAsync(int id, Drug item)
        {
            // TODO: UpdateDrug Tricky
            throw new NotImplementedException();
        }

        private async Task<bool> IsDrugExist(Drug item)
        {
            var result = await this.unitOfWork.Drugs.GetAllAsync();
            return result.Any(d => d.Name == item.Name
                && d.Type == item.Type
                && d.Dose == item.Dose
                && d.DoseUnit == item.DoseUnit);
        }
    }
}