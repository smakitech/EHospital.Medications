using System;
using System.Threading.Tasks;

namespace EHospital.Medications.Model
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Drug> Drugs { get; }

        IRepository<Prescription> Prescriptions { get; }

        bool UpdatePrescriptionStatus(int id);

        Task Save();
    }
}
