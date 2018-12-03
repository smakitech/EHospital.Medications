using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EHospital.Medications.BusinessLogic.Contracts;
using EHospital.Medications.Model;

namespace EHospital.Medications.BusinessLogic.Services
{
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
        /// Exception message in case drug
        /// with such name, type, dose, dose unit,
        /// which represent unique index,
        /// is already exist in the database.
        /// </summary>
        private const string DRUG_IS_ALREADY_EXISTS = "Drug with such name, type, dose and unit is already exist.";

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

        /// <summary>
        /// Creates entity in asynchronous mode.
        /// Checks whether drug with name, type, dose, dose unit
        /// specified in the item is already exist.
        /// If existed drug has deleted status, method changes this status to opposite.
        /// </summary>
        /// <param name="item">Entity to create.</param>
        /// <returns>
        /// Created entity.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Drug with such name, type, dose and unit is already exist.
        /// </exception>
        public async Task<Drug> AddAsync(Drug item)
        {
            Drug result;

            // Search for existed drug
            Drug existedDrug = this.unitOfWork.Drugs.GetAllAsync(d => d.Name == item.Name
                && d.Type == item.Type
                && d.Dose == item.Dose
                && d.DoseUnit == item.DoseUnit).Result.FirstOrDefault();

            // If drug is already existed and has deleted status - status changes.
            // If drug is already existed and has non-deleted status - exception is thrown.
            if (existedDrug != null)
            {
                if (existedDrug.IsDeleted == true)
                {
                    existedDrug.IsDeleted = false;
                    result = await this.unitOfWork.Drugs.UpdateAsync(existedDrug.Id, existedDrug);
                    await this.unitOfWork.Save();
                    return result;
                }
                else
                {
                    throw new ArgumentException(DRUG_IS_ALREADY_EXISTS);
                }
            }

            // Usual addition of drug in case it isn't existed in the database.
            result = this.unitOfWork.Drugs.Insert(item);
            await this.unitOfWork.Save();
            return result;
        }

        /// <summary>
        /// Updates entity in asynchronous mode.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="item">The entity with updated properties.</param>
        /// <returns>
        /// Updated entity.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// Drug with such name, type, dose and unit is already exist.
        /// No drug with such id.
        /// </exception>
        public async Task<Drug> UpdateAsync(int id, Drug item)
        {
            // Search for existed drug with the same name, type, dose, dose unit that item has.
            // Exception is throws in case existeDrug id doesn't equals specified in the method parameters.
            Drug existedDrug = this.unitOfWork.Drugs.GetAllAsync(d => d.Name == item.Name
                && d.Type == item.Type
                && d.Dose == item.Dose
                && d.DoseUnit == item.DoseUnit).Result.FirstOrDefault();
            if (existedDrug.Id != id)
            {
                throw new ArgumentException(DRUG_IS_ALREADY_EXISTS);
            }

            Drug result = await this.unitOfWork.Drugs.GetAsync(id);
            if (result == null || result.IsDeleted == true)
            {
                throw new ArgumentException(DRUG_IS_NOT_FOUND);
            }

            result = await this.unitOfWork.Drugs.UpdateAsync(id, item);
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
    }
}