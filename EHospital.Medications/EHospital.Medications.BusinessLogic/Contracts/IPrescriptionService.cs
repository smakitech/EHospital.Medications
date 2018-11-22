using System.Threading.Tasks;
using EHospital.Medications.Model;

namespace EHospital.Medications.BusinessLogic.Contracts
{
    /// <summary>
    /// Provides CRUD operations methods for prescription service.
    /// </summary>
    /// <seealso cref="IService{Prescription}" />
    public interface IPrescriptionService : IService<Prescription>
    {
        // TODO: Change logic and add documentation
        Task<bool> UpdateStatusAsync(int id);
    }
}