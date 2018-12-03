using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using EHospital.Medications.Model;
using Microsoft.EntityFrameworkCore;

namespace EHospital.Medications.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// The medications database context.
        /// </summary>
        private static MedicationDbContext context;

        /// <summary>
        /// Lazy initialization of the drug repository.
        /// Initialize repository when it is need.
        /// </summary>
        private readonly Lazy<Repository<Drug>> drugs
            = new Lazy<Repository<Drug>>(() => new Repository<Drug>(UnitOfWork.context));

        /// <summary>
        /// Lazy initialization of the prescription repository.
        /// Initialize repository when it is need.
        /// </summary>
        private readonly Lazy<Repository<Prescription>> prescriptions
            = new Lazy<Repository<Prescription>>(() => new Repository<Prescription>(UnitOfWork.context));

        /// <summary>
        /// Track whether dispose method has been called.
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public UnitOfWork(MedicationDbContext context)
        {
            UnitOfWork.context = context;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        ~UnitOfWork()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Gets the drugs.
        /// Provides access to drug repository functionality.
        /// </summary>
        public IRepository<Drug> Drugs
        {
            get
            {
                return this.drugs.Value;
            }
        }

        /// <summary>
        /// Gets the prescriptions.
        /// Provides access to prescription repository functionality.
        /// </summary>
        public IRepository<Prescription> Prescriptions
        {
            get
            {
                return this.prescriptions.Value;
            }
        }

        /// <summary>
        /// Gets the doctors  in asynchronous mode.
        /// </summary>
        /// <returns>
        /// Set of doctors.
        /// </returns>
        public async Task<IQueryable<DoctorView>> GetAllDoctorsAsync()
        {
            return await Task.Run(() => UnitOfWork.context.DoctorsView);
        }

        /// <summary>
        /// Save changes to database in asynchronous mode.
        /// </summary>
        /// <returns>Task object.</returns>
        public async Task Save()
        {
            await UnitOfWork.context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates the prescription status to historic
        /// automatically using SQL store procedure.
        /// Method aimed to be used in get methods and as part of
        /// UpdateStatusManully method.
        /// </summary>
        /// <returns>
        /// Task object.
        /// </returns>
        public async Task UpdateStatusAutomatically()
        {
            string procedure = "UpdateStatusAuthomaticallyPrescription";
            await UnitOfWork.context.Database.ExecuteSqlCommandAsync(procedure);
        }

        /// <summary>
        /// Updates the prescription status to historic
        /// manually using SQL store procedure.
        /// </summary>
        /// <param name="id">The prescription identifier.</param>
        /// <returns>
        /// Task object.
        /// </returns>
        public async Task UpdateStatusManually(int id)
        {
            var parameterId = new SqlParameter("@Id", id);
            string procedure = "UpdateStatusManuallyPrescription @Id";
            await UnitOfWork.context.Database.ExecuteSqlCommandAsync(procedure, parameters: parameterId);

            // Enforce Entity Framework to reload entity after store procedure has been performed
            Prescription prescription = await UnitOfWork.context.Prescriptions.FindAsync(id);
            await UnitOfWork.context.Entry(prescription).ReloadAsync();
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
                    // Disposes managed resources.
                    UnitOfWork.context.Dispose();
                }
            }

            // Disposes unmanaged resources.
            this.disposed = true;
        }
    }
}