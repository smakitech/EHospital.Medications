using System;
using System.Linq;
using System.Threading.Tasks;
using EHospital.Medications.BusinessLogic.Contracts;
using EHospital.Medications.Model;

namespace EHospital.Medications.BusinessLogic.Services
{
    /// <summary>Represents prescription service.</summary>
    /// <seealso cref="IPrescriptionService"/>
    public class PrescriptionService : IPrescriptionService
    {
        /// <summary>
        /// Exception message in case prescription with specified identifier isn't found.
        /// </summary>
        private const string PRESCRIPTION_IS_NOT_FOUND = "No prescription with such id.";

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
        /// Initializes a new instance of the <see cref="PrescriptionService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public PrescriptionService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>Adds prescription in asynchronous mode.</summary>
        /// <param name="item">Prescription to create.</param>
        /// <returns>Created prescription.</returns>
        public async Task<Prescription> AddAsync(Prescription item)
        {
            Prescription prescription = this.unitOfWork.Prescriptions.Insert(item);
            await this.unitOfWork.Save();
            return prescription;
        }

        /// <summary>
        /// Deletes prescription in asynchronous mode.
        /// </summary>
        /// <param name="id">The prescription identifier.</param>
        /// <returns>Deleted prescription.</returns>
        /// <exception cref="ArgumentNullException">
        /// No prescription with such id.
        /// </exception>
        public async Task<Prescription> DeleteAsync(int id)
        {
            Prescription prescription = this.unitOfWork.Prescriptions.Delete(id);

            if (prescription == null)
            {
                throw new ArgumentNullException(PRESCRIPTION_IS_NOT_FOUND);
            }

            await this.unitOfWork.Save();
            return prescription;
        }

        /// <summary>
        /// Gets prescription by identifier in asynchronous mode.
        /// </summary>
        /// <param name="id">The prescription identifier.</param>
        /// <returns>Concrete prescription.</returns>
        /// <exception cref="ArgumentNullException">
        /// No prescription with such id.
        /// </exception>
        public async Task<Prescription> GetByIdAsync(int id)
        {
            Prescription prescription = await this.unitOfWork.Prescriptions.GetAsync(id);

            if (prescription == null)
            {
                throw new ArgumentNullException(PRESCRIPTION_IS_NOT_FOUND);
            }

            return prescription;
        }

        public PrescriptionGuide GetGuideById(int id)
        {
            var guide = from p in this.unitOfWork.Prescriptions.GetAll()
                        where p.Id == id
                        join d in this.unitOfWork.Drugs.GetAll()
                        on p.DrugId equals d.Id
                        select new PrescriptionGuide { Instruction = d.Instruction, Notes = p.Notes };

            if (guide.FirstOrDefault() == null)
            {
                throw new ArgumentNullException(PRESCRIPTION_IS_NOT_FOUND);
            }

            return guide.FirstOrDefault();
        }

        public Task<PrescriptionDetails> GetPrescriptionDetailsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<PrescriptionDetails> GetPrescriptionsDetails(int patientId)
        {
            throw new NotImplementedException();
        }

        public Task<Prescription> UpdateAsync(int id, Prescription item)
        {
            throw new NotImplementedException();
        }

        public Task<Prescription> UpdateStatusAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
