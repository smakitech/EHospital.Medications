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
        /// Exception message in case any prescription was found in the database.
        /// </summary>
        private const string PRESCRIPTIONS_ARE_NOT_FOUND = "No prescriptions found.";

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

        public async Task<Prescription> AddAsync(Prescription item)
        {
            Prescription result = this.unitOfWork.Prescriptions.Insert(item);
            await this.unitOfWork.Save();
            return result;
        }

        public async Task<Prescription> DeleteAsync(int id)
        {
            Prescription result = await this.unitOfWork.Prescriptions.DeleteAsync(id);
            if (result == null)
            {
                throw new ArgumentNullException(PRESCRIPTION_IS_NOT_FOUND);
            }

            await this.unitOfWork.Save();
            return result;
        }

        public async Task<Prescription> GetByIdAsync(int id)
        {
            Prescription result = await this.unitOfWork.Prescriptions.GetAsync(id);
            if (result == null)
            {
                throw new ArgumentNullException(PRESCRIPTION_IS_NOT_FOUND);
            }

            return result;
        }

        public async Task<PrescriptionGuide> GetGuideById(int id)
        {
            // Return IQueryable<Prescription> with one entity
            var prescriptions = await this.unitOfWork.Prescriptions.GetAllAsync(p => p.Id == id);
            if (prescriptions == null)
            {
                throw new ArgumentNullException(PRESCRIPTION_IS_NOT_FOUND);
            }

            // Return IQueryable<Drug> with one entity
            var drugs = await this.unitOfWork.Drugs.GetAllAsync(d => d.Id == prescriptions.First().DrugId);

            // Return IQueryable<PrescriptionGuide> with one entity
            var guide = prescriptions.Join(
                drugs,
                p => p.DrugId,
                d => d.Id,
                (p, d) => new PrescriptionGuide
                {
                    Notes = p.Notes,
                    Instruction = d.Instruction
                });

            PrescriptionGuide result = guide.FirstOrDefault();

            if (result == null)
            {
                throw new ArgumentNullException(PRESCRIPTION_IS_NOT_FOUND);
            }

            return result;
        }

        public async Task<IEnumerable<PrescriptionDetails>> GetPrescriptionsDetails(int patientId)
        {
            // Return IQueryable<Prescription> with prescription of concrete patient which are not deleted
            var prescriptions = await this.unitOfWork.Prescriptions.GetAllAsync(p => p.PatientId == patientId && p.IsDeleted == false);
            if (prescriptions == null)
            {
                throw new ArgumentNullException(PRESCRIPTIONS_ARE_NOT_FOUND);
            }

            // Return IQueryable<Drug> with drugs
            Task<IQueryable<Drug>> drugs = Task.Run(() => this.unitOfWork.Drugs.GetAllAsync());

            // Return IQueryable<DoctorView> with drugs
            Task<IQueryable<DoctorView>> doctors = Task.Run(() => this.unitOfWork.GetAllDoctorsAsync());
            await Task.WhenAll(drugs, doctors);

            // Return IQueryable with one entity
            var details = from prescription in prescriptions
                          join drug in drugs.Result
                          on prescription.DrugId equals drug.Id
                          join doctor in doctors.Result
                          on prescription.UserId equals doctor.Id
                          select new PrescriptionDetails
                          {
                              Id = prescription.Id,
                              FirstName = doctor.FirstName,
                              LastName = doctor.LastName,
                              Name = drug.Name,
                              Type = drug.Type,
                              Dose = drug.Dose,
                              DoseUnit = drug.DoseUnit,
                              Direction = drug.Direction,
                              IsDeleted = drug.IsDeleted,
                              AssignmentDate = prescription.AssignmentDate,
                              Duration = prescription.Duration,
                              IsFinished = prescription.IsFinished
                          };

            if (details == null)
            {
                throw new ArgumentNullException(PRESCRIPTIONS_ARE_NOT_FOUND);
            }

            return details;
        }

        public async Task<Prescription> UpdateAsync(int id, Prescription item)
        {
            Prescription result = await this.unitOfWork.Prescriptions.UpdateAsync(id, item);
            if (result == null)
            {
                throw new ArgumentNullException(PRESCRIPTION_IS_NOT_FOUND);
            }

            await this.unitOfWork.Save();
            return result;
        }

        public async Task<Prescription> UpdateStatusAsync(int id)
        {
            Prescription result = await this.unitOfWork.Prescriptions.GetAsync(id);
            if (result == null)
            {
                throw new ArgumentNullException(PRESCRIPTION_IS_NOT_FOUND);
            }

            await this.unitOfWork.UpdateStatusManually(id);
            result = await this.unitOfWork.Prescriptions.GetAsync(id);
            return result;
        }
    }
}
