using System.Collections.Generic;

namespace DP148.eHealth.API.Medications.Domain.Managers
{
    /// <summary>
    /// Provides prototypes of methods
    /// for base CRUD operations.
    /// </summary>
    /// <typeparam name="T">
    /// Entity specified type.
    /// </typeparam>
    /// <typeparam name="V">
    /// Identifier specified type.
    /// </typeparam>
    public interface IEntityManager<T, V>
    {
        /// <summary>
        /// Gets all items.
        /// </summary>
        /// <returns>All of the items.</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Gets the item by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Item.</returns>
        T GetById(V id);

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        /// Operation success.
        /// </returns>
        V Add(T item);

        /// <summary>
        /// Updates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="item">The item.</param>
        /// <returns>
        /// Operation success.
        /// </returns>
        V Update(V id, T item);

        /// <summary>
        /// Deletes the specified item by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Operation success.
        /// </returns>
        V Delete(V id);
    }
}