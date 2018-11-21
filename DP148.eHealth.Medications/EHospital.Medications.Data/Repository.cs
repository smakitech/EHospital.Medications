using System;
using System.Linq;
using System.Linq.Expressions;
using EHospital.Medications.Model;
using Microsoft.EntityFrameworkCore;

namespace EHospital.Medications.Data
{
    public class Repository<T> : IRepository<T> where T: BaseEntity
    {
        private readonly MedicationDbContext context;

        private DbSet<T> entities;

        public Repository(MedicationDbContext context)
        {
            this.context = context;
            this.entities = context.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return this.entities.Where(e => e.IsDeleted != true).AsNoTracking();
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return this.entities.Where(e => e.IsDeleted != true).Where(predicate).AsNoTracking();
        }

        public T Get(int id)
        {
            T item = this.entities.Find(id);
            if(item == null && item.IsDeleted == true)
            {
                return null;
            }
            else
            {
                return item;
            }
        }

        public T Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Impossible to insert entity, which is equal null");
            }
            this.entities.Add(entity);
            return entity;
        }

        public T Update(T entity)
        {
            this.entities.Attach(entity);
            this.context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public T Delete(T entity)
        {
            //TODO: Store procedure
            T item = this.entities.Find(entity.Id);
            if (item != null)
            {
                this.entities.Remove(entity);
                return entity;
            }
            else
            {
                return null;
            }

        }
    }
}
