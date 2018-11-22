using System;
using EHospital.Medications.Model;
using EHospital.Medications.BusinessLogic.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace EHospital.Medications.BusinessLogic.Services
{
    public class DrugService : IDrugService
    {
        private const string DRUG_IS_EXIST = "Such drug is already stored in the database.";
        private const string DRUG_IS_NOT_FOUND = "No drug with such id";
        private const string DRUGS_ARE_NOT_FOUND = "Any drug wasn't found.";
        private readonly IUnitOfWork unitOfWork;

        public DrugService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Drug> AddAsync(Drug item)
        {
            if (IsDrugExistInDatabase(item))
            {
                throw new ArgumentException(DRUG_IS_EXIST);
            }
            Drug result = this.unitOfWork.Drugs.Insert(item);
            await this.unitOfWork.Save();
            return result;
        }

        public async Task<Drug> UpdateAsync(int id, Drug item)
        {
            if (IsDrugExistInDatabase(item))
            {
                throw new ArgumentException(DRUG_IS_EXIST);
            }
            Drug result = this.unitOfWork.Drugs.Update(item);
            await this.unitOfWork.Save();
            return result;
        }

        public async Task<Drug> DeleteAsync(int id)
        {
            Drug result = this.unitOfWork.Drugs.Get(id);
            if (result == null)
            {
                throw new ArgumentNullException(DRUG_IS_NOT_FOUND);
            }
            if(result.IsDeleted == true)
            {
                throw new ArgumentNullException(DRUG_IS_NOT_FOUND);
            }

            result.IsDeleted = true;
            this.unitOfWork.Drugs.Delete(result);
            await this.unitOfWork.Save();
            return result;
            
        }

        public IQueryable<Drug> GetAll()
        {
            IQueryable<Drug> result = this.unitOfWork.Drugs.GetAll();
            if(result.Count() == 0)
            {
                throw new ArgumentNullException(DRUGS_ARE_NOT_FOUND);
            }
            result.OrderBy(d => d.Name);
            return result;

        }

        public IQueryable<Drug> GetAllByName(string name)
        {
            IQueryable<Drug> result = this.unitOfWork.Drugs.GetAll(d => d.Name == name);
            if (result.Count() == 0)
            {
                throw new ArgumentNullException(DRUGS_ARE_NOT_FOUND);
            }
            result.OrderBy(d => d.Name);
            return result;

        }

        public Drug GetById(int id)
        {
            Drug result = this.unitOfWork.Drugs.Get(id);
            if (result == null)
            {
                throw new ArgumentNullException(DRUG_IS_NOT_FOUND);
            }
            return result;
        }

        private bool IsDrugExistInDatabase(Drug item)
        {
            return this.unitOfWork.Drugs.GetAll()
                            .Any(d => d.Name == item.Name
                            && d.Type == item.Type
                            && d.Dose == item.Dose
                            && d.DoseUnit == item.DoseUnit);
        }
    }
}
