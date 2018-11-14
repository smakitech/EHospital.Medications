using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DP148.eHealth.API.Medications.Domain.DataAccess;
using DP148.eHealth.API.Medications.Domain.Models;
using DP148.eHealth.API.Medications.Domain.Managers;

namespace DP148.eHealth.API.Medications.Controllers
{
    [Route("patient/medications")]
    [ApiController]
    public class PatientMedicationsController : ControllerBase
    {
        private const string NO_CONTENT = "Patient medications were not found.";
        private const string NON_EXISTED_ID = "No patient medication with such id.";
        private const string VALIDATION_INVALID = "Imposible to use invalid data.";

        private IPatientMedicationsManager manager;

        public PatientMedicationsController(IPatientMedicationsManager manager)
        {
            this.manager = manager;
        }

        [HttpGet]
        public IActionResult GetPatientMedications()
        {
            IActionResult result = this.NotFound(NO_CONTENT);

            IEnumerable<PatientMedications> items = this.manager.GetAll();
            if (items.Any())
            {
                result = this.Ok(items);
            }

            return result;
        }

        [HttpGet("{patientMedicationId}")]
        public IActionResult GetById(long patientMedicationId)
        {
            IActionResult result;

            try
            {
                PatientMedications target = this.manager.GetById(patientMedicationId);
                result = this.Ok(target);
            }
            catch (ArgumentException ex)
            {
                result = this.NotFound(ex.Message);
            }

            return result;
        }

        [Route("add")]
        [HttpPost]
        public IActionResult AddPatientMedication([FromBody] PatientMedications prescription)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ValidationProblem(this.ModelState);
            }
            else
            {
                long newPatientMedicationId = this.manager.Add(prescription);
                return this.Created("patient/medications/", newPatientMedicationId);
            }
        }

        [Route("delete/{patientMedicationId}")]
        [HttpDelete]
        public IActionResult RemovePatientMedication(long patientMedicationId)
        {
            IActionResult result;

            try
            {
                this.manager.Delete(patientMedicationId);
                result = this.Ok(patientMedicationId);
            }
            catch (ArgumentException)
            {
                result = this.BadRequest(NON_EXISTED_ID);
            }

            return result;
        }

        [Route("edit/{patientMedicationId}")]
        [HttpPut]
        public IActionResult EditPatientMedication(long patientMedicationId, PatientMedications prescription)
        {
            IActionResult result;

            if (!this.ModelState.IsValid)
            {
                result = this.ValidationProblem(this.ModelState);
            }
            else
            {
                try
                {
                    this.manager.Update(patientMedicationId, prescription);
                    result = this.Ok(patientMedicationId);
                }
                catch (ArgumentException)
                {
                    result = this.BadRequest(NON_EXISTED_ID);
                }
            }

            return result;
        }

        [Route("edit/status/change/{patientMedicationId}")]
        [HttpPut]
        public IActionResult ChangePrescriptionStatus(long patientMedicationId)
        {
            IActionResult result;

            try
            {
                bool status = this.manager.ChangeStatus(patientMedicationId);
                if (status)
                {
                    result = this.Ok($"Changed prescription {patientMedicationId} status to historic.");
                }
                else
                {
                    result = this.Ok($"Changed prescription {patientMedicationId} status to current.");
                }
            }
            catch (ArgumentException)
            {
                result = this.NotFound(NON_EXISTED_ID);
            }

            return result;
        }
    }
}