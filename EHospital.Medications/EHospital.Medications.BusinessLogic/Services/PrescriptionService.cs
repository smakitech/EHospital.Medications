using System;
using System.Collections.Generic;
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

        public Task<Prescription> AddAsync(Prescription item)
        {
            throw new NotImplementedException();
        }

        public Task<Prescription> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Prescription> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PrescriptionGuide> GetGuideById(int id)
        {
            //IQueryable<PrescriptionGuide> guide = from p in this.unitOfWork.Prescriptions.GetAll()
            //                                      where p.Id == id
            //                                      join d in this.unitOfWork.Drugs.GetAll()
            //                                      on p.DrugId equals d.Id
            //                                      select new PrescriptionGuide { Instruction = d.Instruction, Notes = p.Notes };
            IQueryable<PrescriptionGuide> guide;
            try
            {
                var prescriptions = await this.unitOfWork.Prescriptions.GetAllAsync(p => p.Id == id);
                var drugs = await this.unitOfWork.Drugs.GetAllAsync();

                guide = prescriptions.AsQueryable()
                .Join(
                drugs.AsQueryable(),
                p => p.DrugId,
                d => d.Id,
                (prescription, drug) => new PrescriptionGuide
                {
                    Notes = prescription.Notes,
                    Instruction = drug.Instruction
                });
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException(PRESCRIPTION_IS_NOT_FOUND);
            }

            if (guide.FirstOrDefault() == null)
            {
                throw new ArgumentNullException(PRESCRIPTION_IS_NOT_FOUND);
            }

            return guide.First();
        }

        public Task<PrescriptionDetails> GetPrescriptionDetailsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PrescriptionDetails>> GetPrescriptionsDetails(int patientId)
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
