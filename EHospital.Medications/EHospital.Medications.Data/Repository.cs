﻿using System;
using System.Linq;
using System.Linq.Expressions;
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
    public class Repository<T> : IRepository<T> where T : BaseEntity
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
        /// Gets all entities.
        /// </summary>
        /// <returns>
        /// All entities.
        /// </returns>
        public IQueryable<T> GetAll()
        {
            return this.entities.AsNoTracking();
        }

        /// <summary>
        /// Gets all entities by specified predicate.
        /// </summary>
        /// <param name="predicate">Predicate specifies search conditions.</param>
        /// <returns>
        /// Set of entities.
        /// </returns>
        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return this.entities.Where(predicate).AsNoTracking();
        }

        /// <summary>
        /// Gets the entity specified by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// Concrete entity.
        /// </returns>
        public T Get(int id)
        {
            T item = this.entities.Find(id);
            if (item == null)
            {
                return null;
            }
            else
            {
                return item;
            }
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
        /// </summary>
        /// <param name="id">
        /// Identifier of the entity to update.
        /// </param>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// Updated entity.
        /// </returns>
        public T Update(int id, T entity)
        {
            // TODO: Don't update id
            T original = this.entities.Find(id);
            entity.Id = id;
            if (original != null)
            {
                this.context.Entry(original).CurrentValues.SetValues(entity);
            }

            return entity;
        }

        /// <summary>
        /// Performs soft deletion of the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// Deleted entity.
        /// </returns>
        public T Delete(T entity)
        {
            entity.IsDeleted = true;
            return this.Update(entity.Id, entity);
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
