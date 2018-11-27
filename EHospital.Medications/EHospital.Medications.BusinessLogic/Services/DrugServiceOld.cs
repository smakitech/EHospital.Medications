﻿//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using EHospital.Medications.Model;
//using EHospital.Medications.BusinessLogic.Contracts;

//namespace EHospital.Medications.BusinessLogic.Services
//{
//    /// <summary>Represents drug service.</summary>
//    /// <seealso cref="IDrugService"/>
//    public class DrugServiceOld : IDrugService
//    {
//        /// <summary>
//        /// Exception message in case drug is already exist in the database
//        /// or soft deleted - has value <c>true</c> in property IsDeleted.
//        /// </summary>
//        private const string DRUG_IS_EXIST_OR_STORES_AS_SOFT_DELETED 
//            = "Such drug is already exist in database or stores with deleted status.";

//        /// <summary>
//        /// Exception message in case drug with specified identifier isn't found.
//        /// </summary>
//        private const string DRUG_IS_NOT_FOUND = "No drug with such id.";

//        /// <summary>
//        /// Exception message in case any drug was found in the database.
//        /// </summary>
//        private const string DRUGS_ARE_NOT_FOUND = "No drugs found.";

//        /// <summary>
//        /// Combine usage of unit of work and repository pattern.
//        /// It contain repositories for each entity
//        /// and one common database context in order
//        /// to access data and perform CRUD operation.
//        /// </summary>
//        private readonly IUnitOfWork unitOfWork;

//        /// <summary>
//        /// Initializes a new instance of the <see cref="DrugService"/> class.
//        /// </summary>
//        /// <param name="unitOfWork">The unit of work.</param>
//        public DrugService(IUnitOfWork unitOfWork)
//        {
//            this.unitOfWork = unitOfWork;
//        }

//        /// <summary>Adds drug in asynchronous mode.</summary>
//        /// <param name="item">Drug to create.</param>
//        /// <returns>Created drug.</returns>
//        /// <exception cref="ArgumentException">
//        /// Such drug is already exist in the database.
//        /// </exception>
//        public async Task<Drug> AddAsync(Drug item)
//        {
//            if (this.IsDrugExistInDatabase(item))
//            {
//                throw new ArgumentException(DRUG_IS_EXIST_OR_STORES_AS_SOFT_DELETED);
//            }

//            Drug result = this.unitOfWork.Drugs.Insert(item);
//            await this.unitOfWork.Save();
//            return result;
//        }

//        /// <summary>Updates drug in asynchronous mode.</summary>
//        /// <param name="id">The identifier.</param>
//        /// <param name="item">The drug with updated properties.</param>
//        /// <returns>Updated drug.</returns>
//        /// <exception cref="ArgumentException">
//        /// Such drug is already exist in database or stores with deleted status.
//        /// </exception>
//        /// <exception cref="ArgumentNullException">
//        /// No drug with such id.
//        /// </exception>
//        public async Task<Drug> UpdateAsync(int id, Drug item)
//        {
//            if (this.IsDrugExistInDatabase(item))
//            {
//                throw new ArgumentException(DRUG_IS_EXIST_OR_STORES_AS_SOFT_DELETED);
//            }

//            if (this.unitOfWork.Drugs.Get(id).IsDeleted == true)
//            {
//                throw new ArgumentNullException(DRUG_IS_NOT_FOUND);
//            }

//            Drug result = this.unitOfWork.Drugs.Update(id, item);
//            await this.unitOfWork.Save();
//            return result;
//        }

//        /// <summary>Deletes drug in asynchronous mode.</summary>
//        /// <param name="id">The identifier.</param>
//        /// <returns>Deleted drug.</returns>
//        /// <exception cref="ArgumentNullException">
//        /// No drug with such id
//        /// </exception>
//        public async Task<Drug> DeleteAsync(int id)
//        {
//            Drug result = this.unitOfWork.Drugs.Get(id);
//            if (result == null)
//            {
//                throw new ArgumentNullException(DRUG_IS_NOT_FOUND);
//            }

//            if (result.IsDeleted == true)
//            {
//                throw new ArgumentNullException(DRUG_IS_NOT_FOUND);
//            }

//            this.unitOfWork.Drugs.Delete(result);
//            await this.unitOfWork.Save();
//            return result;
//        }

//        /// <summary>Gets all drugs.</summary>
//        /// <returns>All drugs.</returns>
//        /// <exception cref="ArgumentNullException">
//        /// No drugs found.
//        /// </exception>
//        public IQueryable<Drug> GetAll()
//        {
//            IQueryable<Drug> result = this.unitOfWork.Drugs.GetAll().Where(d => d.IsDeleted != true);
//            if (result.Count() == 0)
//            {
//                throw new ArgumentNullException(DRUGS_ARE_NOT_FOUND);
//            }

//            result.OrderBy(d => d.Name);
//            return result;
//        }

//        /// <summary>Gets all drugs by specified name.</summary>
//        /// <param name="name">Specified name.</param>
//        /// <returns>Drugs.</returns>
//        /// <exception cref="ArgumentNullException">
//        /// No drugs found.
//        /// </exception>
//        public IQueryable<Drug> GetAllByName(string name)
//        {
//            IQueryable<Drug> result = this.unitOfWork.Drugs.GetAll(d => d.Name == name).Where(d => d.IsDeleted != true);
//            if (result.Count() == 0)
//            {
//                throw new ArgumentNullException(DRUGS_ARE_NOT_FOUND);
//            }

//            result.OrderBy(d => d.Name);
//            return result;
//        }

//        /// <summary>Gets drug by identifier.</summary>
//        /// <param name="id">The identifier.</param>
//        /// <returns>Concrete drug.</returns>
//        /// <exception cref="ArgumentNullException">
//        /// No drug with such id.
//        /// </exception>
//        public Drug GetById(int id)
//        {
//            Drug result = this.unitOfWork.Drugs.Get(id);
//            if (result == null || result.IsDeleted == true)
//            {
//                throw new ArgumentNullException(DRUG_IS_NOT_FOUND);
//            }

//            return result;
//        }

//        /// <summary>
//        /// Determines whether drug is already exist in database.
//        /// </summary>
//        /// <param name="item">The drug to check.</param>
//        /// <returns>
//        /// <c>true</c> if drug is already exist in database, otherwise, <c>false</c>.
//        /// </returns>
//        private bool IsDrugExistInDatabase(Drug item)
//        {
//            return this.unitOfWork.Drugs.GetAll()
//                            .Any(d => d.Name == item.Name
//                            && d.Type == item.Type
//                            && d.Dose == item.Dose
//                            && d.DoseUnit == item.DoseUnit);
//        }
//    }
//}