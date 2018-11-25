using System.Linq;
using EHospital.Medications.BusinessLogic.Contracts;
using EHospital.Medications.Model;

namespace EHospital.Medications.BusinessLogic.Services
{
    // TODO: view temp
    public class DoctorsService : IDoctorService
    {
        private readonly IUnitOfWork unitOfWork;

        public DoctorsService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IQueryable<DoctorView> GetDoctors()
        {
            return this.unitOfWork.GetDoctors();
        }
    }
}
