using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EHospital.Medications.BusinessLogic.Contracts;
using EHospital.Medications.Model;

namespace EHospital.Medications.BusinessLogic.Services
{
    public class DrugService : IDrugService
    {
        public Task<Drug> AddAsync(Drug item)
        {
            throw new NotImplementedException();
        }

        public Task<Drug> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Drug> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Drug> GetAllByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<Drug> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Drug> UpdateAsync(int id, Drug item)
        {
            throw new NotImplementedException();
        }
    }
}
