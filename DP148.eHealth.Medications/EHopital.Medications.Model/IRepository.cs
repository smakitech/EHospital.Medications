using System;
using System.Linq;
using System.Linq.Expressions;

namespace EHospital.Medications.Model
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll();

        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate);

        T Get(int id);

        T Insert(T entity);

        T Update(T entity);

        // TODO: Softdelete & full delete
        T Delete(T entity);
    }
}
