using System.Linq;
using System.Threading.Tasks;

namespace EHospital.Medications.BusinessLogic.Contracts
{
    /// <summary>
    /// Provides prototypes of methods,
    /// which serve to perform common CRUD operations on entity.
    /// </summary>
    /// <typeparam name="T">Entity type.</typeparam>
    public interface IService<T>
    {
        /// <summary>
        /// Gets entity by the identifier in asynchronous mode.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Concrete entity.</returns>
        Task<T> GetByIdAsync(int id);

        /// <summary>
        /// Creates entity in asynchronous mode.
        /// </summary>
        /// <param name="item">Entity to create.</param>
        /// <returns>Created entity.</returns>
        Task<T> AddAsync(T item);

        /// <summary>
        /// Updates entity in asynchronous mode.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="item">The entity with updated properties.</param>
        /// <returns>Updated entity.</returns>
        Task<T> UpdateAsync(int id, T item);

        /// <summary>
        /// Deletes entity in asynchronous mode.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Deleted entity.</returns>
        Task<T> DeleteAsync(int id);
    }
}