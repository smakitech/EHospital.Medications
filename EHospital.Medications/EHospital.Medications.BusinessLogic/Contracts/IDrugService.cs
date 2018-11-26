using System.Linq;
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
        /// Gets all drugs.
        /// </summary>
        /// <returns>Drugs.</returns>
        IQueryable<Drug> GetAll();

        /// <summary>
        /// Gets all drugs by specified name.
        /// </summary>
        /// <param name="name">Specified name.</param>
        /// <returns>Drugs.</returns>
        IQueryable<Drug> GetAllByName(string name);
    }
}