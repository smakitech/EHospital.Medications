using EHospital.Medications.Model;
using System.Linq;

namespace EHospital.Medications.BusinessLogic.Contracts
{
    public interface IDrugService : IEntityManager<Drug>
    {
        IQueryable<Drug> GetAllByName(string name);
    }
}
