using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EHospital.Medications.Model
{
    /// <summary>
    /// Represents interface of repository,
    /// which serves as mediate tier between
    /// business logic and data source.
    /// </summary>
    /// <typeparam name="T">
    /// Entity type, which inherit <see cref="BaseEntity"/>.
    /// </typeparam>
    /// <seealso cref="IDisposable"/>
    public interface IRepository<T> : IDisposable where T : BaseEntity, ISoftDeletion
    {
        /// <summary>Gets all entities in asynchronous mode.</summary>
        /// <returns>All entities.</returns>
        Task<ICollection<T>> GetAllAsync();

        /// <summary>
        /// Gets all entities by specified predicate
        /// in asynchronous mode.
        /// </summary>
        /// <param name="predicate">
        /// Predicate specifies search conditions.
        /// </param>
        /// <returns>Set of entities.</returns>
        Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Gets the entity specified by identifier in asynchronous mode.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Concrete entity.</returns>
        Task<T> GetAsync(int id);

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Inserted entity.</returns>
        T Insert(T entity);

        /// <summary>
        /// Updates the specified entity
        /// Uses an asynchronous.
        /// </summary>
        /// <param name="id">
        /// Identifier of the entity to update.
        /// </param>
        /// <param name="entity">The entity.</param>
        /// <returns>Updated entity.</returns>
        Task<T> UpdateAsync(int id, T entity);

        /// <summary>
        /// Deletes the specified entity.
        /// Uses an asynchronous.
        /// </summary>
        /// <param name="id">Identifier of the entity to delete.</param>
        /// <returns>Deleted entity.</returns>
        Task<T> DeleteAsync(int id);
    }
}