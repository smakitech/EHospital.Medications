using System;
using System.Collections.Generic;
using System.Text;
using DP148.eHealth.API.Medications.Domain.Models;

namespace DP148.eHealth.API.Medications.Domain.DataAccess
{
    public interface IMedicationsProvider
    {
        IEnumerable<Models.Medications> GetMedications();

        Models.Medications GetMedicationById(long id);

        IEnumerable<Models.Medications> GetMedicationsByName(string name);

        long AddMedication(Models.Medications item);

        long UpdateMedication(long id, Models.Medications item);

        long DeleteMedication(long id);

        bool IsIdentifierExists(long id);
    }
}
