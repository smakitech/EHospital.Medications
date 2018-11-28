//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using EHospital.Medications.Model;
//using EHospital.Medications.BusinessLogic.Contracts;

//namespace EHospital.Medications.BusinessLogic.Services
//{
//    /// <summary>Represents prescription service.</summary>
//    /// <seealso cref="IPrescriptionService"/>
//    public class PrescriptionServiceOld : IPrescriptionService
//    {
//        /// <summary>
//        /// Exception message in case prescription with specified identifier isn't found.
//        /// </summary>
//        private const string PRESCRIPTION_IS_NOT_FOUND = "No prescription with such id.";

//        /// <summary>
//        /// Exception message in case any prescription was found in the database.
//        /// </summary>
//        private const string PRESCRIPTIONS_ARE_NOT_FOUND = "No prescriptions found.";

//        /// <summary>
//        /// Combine usage of unit of work and repository pattern.
//        /// It contain repositories for each entity
//        /// and one common database context in order
//        /// to access data and perform CRUD operation.
//        /// </summary>
//        private readonly IUnitOfWork unitOfWork;

//        /// <summary>
//        /// Initializes a new instance of the <see cref="PrescriptionServiceOld"/> class.
//        /// </summary>
//        /// <param name="unitOfWork">The unit of work.</param>
//        public PrescriptionServiceOld(IUnitOfWork unitOfWork)
//        {
//            this.unitOfWork = unitOfWork;
//        }

//        /// <summary>Adds prescription in asynchronous mode.</summary>
//        /// <param name="item">Prescription to create.</param>
//        /// <returns>Created prescription.</returns>
//        public async Task<Prescription> AddAsync(Prescription item)
//        {
//            Prescription result = this.unitOfWork.Prescriptions.Insert(item);
//            await this.unitOfWork.Save();
//            return result;
//        }

//        /// <summary>Updates prescription in asynchronous mode.</summary>
//        /// <param name="id">The identifier.</param>
//        /// <param name="item">The prescription with updated properties.</param>
//        /// <returns>Updated prescription.</returns>
//        /// <exception cref="ArgumentNullException">
//        /// No prescription with such id.
//        /// </exception>
//        public async Task<Prescription> UpdateAsync(int id, Prescription item)
//        {
//            Prescription result = this.unitOfWork.Prescriptions.Update(id, item);
//            await this.unitOfWork.Save();
//            return result;
//        }

//        /// <summary>Deletes prescription in asynchronous mode.</summary>
//        /// <param name="id">The identifier.</param>
//        /// <returns>Deleted prescription.</returns>
//        /// <exception cref="ArgumentNullException">
//        /// No prescription with such id.
//        /// </exception>
//        public async Task<Prescription> DeleteAsync(int id)
//        {
//            Prescription result = this.unitOfWork.Prescriptions.Get(id);
//            if (result == null)
//            {
//                throw new ArgumentNullException(PRESCRIPTION_IS_NOT_FOUND);
//            }

//            if (result.IsDeleted == true)
//            {
//                throw new ArgumentNullException(PRESCRIPTION_IS_NOT_FOUND);
//            }

//            this.unitOfWork.Prescriptions.Delete(result);
//            await this.unitOfWork.Save();
//            return result;
//        }

//        /// <summary>Gets all prescriptions.</summary>
//        /// <returns>All prescriptions.</returns>
//        /// <exception cref="ArgumentNullException">
//        /// No prescriptions found.
//        /// </exception>
//        public IQueryable<Prescription> GetAll()
//        {
//            IQueryable<Prescription> result = this.unitOfWork.Prescriptions.GetAll().Where(p => p.IsDeleted != true);
//            if (result.Count() == 0)
//            {
//                throw new ArgumentNullException(PRESCRIPTIONS_ARE_NOT_FOUND);
//            }

//            result.OrderBy(d => d.AssignmentDate);
//            return result;
//        }

//        /// <summary>Gets prescription by identifier.</summary>
//        /// <param name="id">The identifier.</param>
//        /// <returns>Concrete prescription.</returns>
//        /// <exception cref="ArgumentNullException">
//        /// No prescription with such id.
//        /// </exception>
//        public Prescription GetById(int id)
//        {
//            Prescription result = this.unitOfWork.Prescriptions.Get(id);
//            if (result == null || result.IsDeleted == true)
//            {
//                throw new ArgumentNullException(PRESCRIPTION_IS_NOT_FOUND);
//            }

//            return result;
//        }

//        // TODO: Change Logic, add documentation
//        public async Task<bool> UpdateStatusAsync(int id)
//        {
//            Pres
//        }
//    }
//}