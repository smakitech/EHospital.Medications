using System;
using System.Linq;
using System.Linq.Expressions;

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
    public interface IRepository<T> : IDisposable where T : BaseEntity
    {
        /// <summary>Gets all entities.</summary>
        /// <returns>All entities.</returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Gets all entities by specified predicate.
        /// </summary>
        /// <param name="predicate">
        /// Predicate specifies search conditions.
        /// </param>
        /// <returns>Set of entities.</returns>
        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Gets the entity specified by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Concrete entity.</returns>
        T Get(int id);

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Inserted entity.</returns>
        T Insert(T entity);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="id">
        /// Identifier of the entity to update.
        /// </param>
        /// <param name="entity">The entity.</param>
        /// <returns>Updated entity.</returns>
        T Update(int id, T entity);

        // TODO: [Connect Change] Delete signature

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="id">Identifier of the entity to delete.</param>
        /// <returns>Deleted entity.</returns>
        T Delete(int id);
    }
}