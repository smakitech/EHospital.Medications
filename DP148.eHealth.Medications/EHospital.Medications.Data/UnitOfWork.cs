using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EHospital.Medications.Model;

namespace EHospital.Medications.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly static  MedicationDbContext context;

        private readonly Lazy<Repository<Drug>> drugs
            = new Lazy<Repository<Drug>>(() => new Repository<Drug>(UnitOfWork.context));
        private readonly Lazy<Repository<Prescription>> prescriptions
            = new Lazy<Repository<Prescription>>(() => new Repository<Prescription>(UnitOfWork.context));

        private bool disposed = false;

        static UnitOfWork()
        {
            UnitOfWork.context = new MedicationDbContext();
        }

        public UnitOfWork()
        {
        }

        public IRepository<Drug> Drugs
        {
            get
            {
                return this.drugs.Value;
            }
        }

        public IRepository<Prescription> Prescriptions
        {
            get
            {
                return this.prescriptions.Value;
            }
        }

        public async Task Save()
        {
            await UnitOfWork.context.SaveChangesAsync();
        }

        public bool UpdatePrescriptionStatus(int id)
        {
            // TODO: Replace store procedure
            Prescription item = this.prescriptions.Value.Get(id);
            if (item.IsFinished == true)
            {
                item.IsFinished = false;
            }
            else
            {
                item.IsFinished = true;
            }
            this.prescriptions.Value.Update(item);
            return item.IsFinished;
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    UnitOfWork.context.Dispose();
                }
            }
            this.disposed = true;
        }
    }
}
