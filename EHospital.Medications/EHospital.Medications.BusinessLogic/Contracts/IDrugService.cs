using System.Linq;
using System.Threading.Tasks;
using EHospital.Medications.Model;

namespace EHospital.Medications.BusinessLogic.Contracts
{
    /// <summary>
    /// Provides CRUD operations methods for drug service.
    /// </summary>
    /// <seealso cref="IService{Drug}"/>
    public interface IDrugService : IService<Drug>
    {
        /// <summary>
        /// Gets all drugs  in asynchronous mode.
        /// </summary>
        /// <returns>Drugs.</returns>
        Task<IQueryable<Drug>> GetAllAsync();

        /// <summary>
        /// Gets all drugs by specified name in asynchronous mode.
        /// </summary>
        /// <param name="name">Specified name.</param>
        /// <returns>Drugs.</returns>
        Task<IQueryable<Drug>> GetAllByNameAsync(string name);
    }
}