using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EHospital.Medications.Model;

namespace EHospital.Medications.Data
{
    /// <summary>
    /// Represents medications repository,
    /// which serves as mediate tier between
    /// business logic and data source.
    /// </summary>
    /// <typeparam name="T">
    /// Entity type, which inherits <see cref="BaseEntity"/>
    /// </typeparam>
    /// <seealso cref="IRepository{T}" />
    public class Repository<T> : IRepository<T> where T : BaseEntity, ISoftDeletion
    {
        /// <summary>
        /// The medication database context.
        /// </summary>
        private readonly MedicationDbContext context;

        /// <summary>
        /// Gets or sets the entities.
        /// Represents set of entities which store in the database.
        /// Helps to interact with some table placed in the database.
        /// </summary>
        private DbSet<T> entities;

        /// <summary>
        /// Track whether dispose method has been called.
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{T}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public Repository(MedicationDbContext context)
        {
            this.context = context;
            this.entities = context.Set<T>();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Repository{T}"/> class.
        /// </summary>
        ~Repository()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Gets all entities in asynchronous mode.
        /// </summary>
        /// <returns>
        /// All entities.
        /// </returns>
        public async Task<IQueryable<T>> GetAllAsync()
        {
            return await Task.Run(() => this.entities);
        }

        /// <summary>
        /// Gets all entities by specified predicate
        /// in asynchronous mode.
        /// </summary>
        /// <param name="predicate">
        /// Predicate specifies search conditions.
        /// </param>
        /// <returns>
        /// Set of entities.
        /// </returns>
        public async Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await Task.Run(() => this.entities.Where(predicate));
        }

        /// <summary>
        /// Gets the entity specified by identifier in asynchronous mode.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Concrete entity.
        /// </returns>
        public async Task<T> GetAsync(int id)
        {
            return await this.entities.FindAsync(id);
        }

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// Inserted entity.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Impossible to insert entity, which is equal null
        /// </exception>
        public T Insert(T entity)
        {
            this.entities.Add(entity);
            return entity;
        }

        /// <summary>
        /// Updates the specified entity.
        /// Uses an asynchronous.
        /// </summary>
        /// <param name="id">
        /// Identifier of the entity to update.
        /// </param>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// Updated entity.
        /// </returns>
        public async Task<T> UpdateAsync(int id, T entity)
        {
            T target = await this.entities.FindAsync(id);
            if (target != null)
            {
                entity.Id = id;
                this.context.Entry(target).CurrentValues.SetValues(entity);
                return entity;
            }

            return target;
        }

        /// <summary>
        /// Performs soft deletion of the specified entity.
        /// Uses an asynchronous.
        /// </summary>
        /// <param name="id">
        /// Identifier of the entity to delete.
        /// </param>
        /// <returns>
        /// Deleted entity.
        /// </returns>
        public async Task<T> DeleteAsync(int id)
        {
            T target = await this.entities.FindAsync(id);
            if (target != null)
            {
                target.IsDeleted = true;
                this.context.Attach(target);
                var entry = this.context.Entry(target);
                entry.Property(e => e.IsDeleted).IsModified = true;
            }

            return target;
        }

        /// <summary>
        /// Disposes all resources of instance.
        /// <see cref="IRepository{T}"/>
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Implements logic of the cleanup.
        /// </summary>
        /// <param name="disposing">
        /// Flag defines how dispose method has been called.
        /// If <code>true</code> method called by users code,
        /// managed and unmanaged resources will be released.
        /// If <code>false</code> method called by runtime,
        /// only unmanaged resources will be released.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources
                    this.context.Dispose();
                }
            }

            // Disposed unmanaged resources
            this.disposed = true;
        }
    }
}