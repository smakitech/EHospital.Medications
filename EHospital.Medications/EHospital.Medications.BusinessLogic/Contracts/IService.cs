using System.Linq;
using System.Threading.Tasks;

namespace EHospital.Medications.BusinessLogic.Contracts
{
    public interface IService<T>
    {
        // TODO: make async
        IQueryable<T> GetAll();

        T GetById(int id);

        Task<T> AddAsync(T item);

        Task<T> UpdateAsync(int id, T item);

        Task<T> DeleteAsync(int id);
    }
}