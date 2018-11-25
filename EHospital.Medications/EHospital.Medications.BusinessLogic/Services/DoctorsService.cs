using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EHospital.Medications.BusinessLogic.Contracts;
using EHospital.Medications.Model;
using EHospital.Medications.Data;

namespace EHospital.Medications.BusinessLogic.Services
{
    public class DoctorsService : IService<DoctorView>
    {
        private readonly IUnitOfWork unitOfWork;

        public DoctorsService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Task<DoctorView> AddAsync(DoctorView item)
        {
            throw new NotImplementedException();
        }

        public Task<DoctorView> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<DoctorView> GetAll()
        {
            return this.unitOfWork.Doctors.GetAll();
        }

        public DoctorView GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<DoctorView> UpdateAsync(int id, DoctorView item)
        {
            throw new NotImplementedException();
        }
    }
}
