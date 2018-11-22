using EHospital.Medications.Model;
using System.Threading.Tasks;

namespace EHospital.Medications.BusinessLogic.Contracts
{
    public interface IPrescriptionService : IService<Prescription>
    {
        Task<bool> UpdateStatusAsync(int id);
    }
}
