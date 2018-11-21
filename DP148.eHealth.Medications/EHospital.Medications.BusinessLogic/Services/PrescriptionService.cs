using System;
using EHospital.Medications.Model;
using EHospital.Medications.BusinessLogic.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace EHospital.Medications.BusinessLogic.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private const string PRESCRIPTION_IS_NOT_FOUND = "No prescription with such id";
        private const string PRESCRIPTIONS_ARE_NOT_FOUND = "Any prescription wasn't found.";

        private readonly IUnitOfWork unitOfWork;

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

        public async Task<Prescription> UpdateAsync(int id, Prescription item)
        {
            Prescription result = this.unitOfWork.Prescriptions.Update(item);
            await this.unitOfWork.Save();
            return result;
        }

        public async Task<Prescription> DeleteAsync(int id)
        {
            Prescription result = this.unitOfWork.Prescriptions.Get(id);
            if (result == null)
            {
                throw new ArgumentNullException(PRESCRIPTION_IS_NOT_FOUND);
            }
            if (result.IsDeleted == true)
            {
                throw new ArgumentNullException(PRESCRIPTION_IS_NOT_FOUND);
            }

            result.IsDeleted = true;
            this.unitOfWork.Prescriptions.Delete(result);
            await this.unitOfWork.Save();
            return result;

        }

        public IQueryable<Prescription> GetAll()
        {
            IQueryable<Prescription> result = this.unitOfWork.Prescriptions.GetAll();
            if (result.Count() == 0)
            {
                throw new ArgumentNullException(PRESCRIPTIONS_ARE_NOT_FOUND);
            }
            result.OrderBy(d => d.AssignmentDate);
            return result;
        }

        public Prescription GetById(int id)
        {
            Prescription result = this.unitOfWork.Prescriptions.Get(id);
            if (result == null)
            {
                throw new ArgumentNullException(PRESCRIPTION_IS_NOT_FOUND);
            }
            return result;
        }

        public async Task<bool> UpdateStatusAsync(int id)
        {
            Prescription target = this.unitOfWork.Prescriptions.Get(id);
            if (target == null)
            {
                throw new ArgumentNullException(PRESCRIPTION_IS_NOT_FOUND);
            }
            bool result = this.unitOfWork.UpdatePrescriptionStatus(id);
            await this.unitOfWork.Save();
            return result;
        }
    }
}
