using EHospital.Medications.Model;
using System.Threading.Tasks;

namespace EHospital.Medications.BusinessLogic.Contracts
{
    public interface IPrescriptionService : IEntityManager<Prescription>
    {
        Task<bool> UpdateStatusAsync(int id);
    }
}
