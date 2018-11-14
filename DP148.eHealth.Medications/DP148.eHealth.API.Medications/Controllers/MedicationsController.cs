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
    [Route("medications")]
    [ApiController]
    public class MedicationsController : ControllerBase
    {
        private const string NO_MEDICATIONS = "Medications was not founded.";
        private const string NON_EXISTED_ID = "No medication with such id.";
        private const string VALIDATION_INVALID = "Imposible to use invalid data.";

        private IMedicationsManager manager;

        public MedicationsController(IMedicationsManager manager)
        {
            this.manager = manager;
        }

        [HttpGet]
        public IActionResult GetMedications()
        {
            IActionResult result = this.NotFound(NO_MEDICATIONS);

            IEnumerable<Domain.Models.Medications> items = this.manager.GetAll();
            if (items.Any())
            {
                result = this.Ok(items);
            }

            return result;
        }

        [HttpGet("{medicationId}")]
        public IActionResult GetById(long medicationId)
        {
            IActionResult result;

            try
            {
                Domain.Models.Medications target = this.manager.GetById(medicationId);
                result = this.Ok(target);
            }
            catch (ArgumentException ex)
            {
                result = this.NotFound(ex.Message);
            }

            return result;
        }

        [Route("filter")]
        [HttpGet]
        public IActionResult GetByName([FromQuery] string medicationName)
        {
            IActionResult result = this.NotFound(NO_MEDICATIONS);

            IEnumerable<Domain.Models.Medications> items = this.manager.GetByName(medicationName);
            if (items.Any())
            {
                result = this.Ok(items);
            }

            return result;
        }

        [Route("add")]
        [HttpPost]
        public IActionResult AddMedication([FromBody] Domain.Models.Medications medicine)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ValidationProblem(this.ModelState);
            }
            else
            {
                long newMedicationId = this.manager.Add(medicine);
                return this.Created("medications/", newMedicationId);
            }
        }

        [Route("delete/{medicationId}")]
        [HttpDelete]
        public IActionResult RemoveMedication(long medicationId)
        {
            IActionResult result;

            try
            {
                long removedMedicationId = this.manager.Delete(medicationId);
                result = this.Ok(removedMedicationId);
            }
            catch (ArgumentException)
            {
                result = this.BadRequest(NON_EXISTED_ID);
            }

            return result;
        }

        [Route("edit/{medicationId}")]
        [HttpPut]
        public IActionResult EditMedication(long medicationId, Domain.Models.Medications medication)
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
                    long updatedMedicationId = this.manager.Update(medicationId, medication);
                    result = this.Ok(updatedMedicationId);
                }
                catch (ArgumentException)
                {
                    result = this.BadRequest(NON_EXISTED_ID);
                }
            }

            return result;
        }

    }
}
