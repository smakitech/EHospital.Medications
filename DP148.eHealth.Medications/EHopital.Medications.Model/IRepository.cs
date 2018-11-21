using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace EHospital.Medications.Model
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll();

        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate);

        T Get(int id);

        T Insert(T entity);

        T Update(T entity);

        T Delete(int id);

    }
}
