using System.Collections.Generic;
using DP148.eHealth.API.Medications.Domain.Models;

namespace DP148.eHealth.API.Medications.Domain.DataAccess
{
    public interface IPatientMedicationsProvider
    {
        IEnumerable<PatientMedications> GetPatientMedications();

        PatientMedications GetPatientMedicationById(long id);

        long AddPatientMedication(PatientMedications item);

        long UpdatePatientMedication(long id, PatientMedications item);

        long DeletePatientMedication(long id);

        bool ChangePatientMedicationStatus(long id);

        bool IsIdentifierExists(long id);
    }
}
