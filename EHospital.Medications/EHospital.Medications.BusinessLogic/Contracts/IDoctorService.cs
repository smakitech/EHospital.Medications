using System.Linq;
using EHospital.Medications.Model;

namespace EHospital.Medications.BusinessLogic.Contracts
{
    // TODO: view temp
    public interface IDoctorService
    {
        IQueryable<DoctorView> GetDoctors();
    }
}