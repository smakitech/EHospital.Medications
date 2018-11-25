using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EHospital.Medications.BusinessLogic.Contracts;
using EHospital.Medications.Model;

namespace EHospital.Medications.WebAPI.Controllers
{
    /// <summary>
    /// Represents prescription controller.
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [Route("patient/drugs")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        /// <summary>
        /// Response message in case prescriptions were not found.
        /// </summary>
        private const string NO_CONTENT = "Patient medications were not found.";

        /// <summary>
        /// Interface link on drug service.
        /// </summary>
        private IPrescriptionService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrescriptionController"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public PrescriptionController(IPrescriptionService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Handles request GET /patient/drugs/
        /// Handles request GET /patient/drugs
        /// Retrieves all prescriptions records from database in JSON format.
        /// </summary>
        /// <returns>
        /// Returns one of two action results.
        /// Ok with all prescriptions records in JSON format.
        /// NotFound with message.
        /// </returns>
        [HttpGet]
        public IActionResult GetPrescriptions()
        {
            IQueryable<Prescription> prescriptions = this.service.GetAll();
            if (!prescriptions.Any())
            {
                return this.NotFound(NO_CONTENT);
            }

            return this.Ok(prescriptions);
        }

        /// <summary>
        /// Handles request GET /patient/drugs/{prescriptionId}
        /// Retrieves prescription record from database in JSON format
        /// specified by identifier.
        /// </summary>
        /// <param name="prescriptionId">The prescription identifier.</param>
        /// <returns>
        /// Returns one of two action results.
        /// Ok with concrete prescription in JSON format.
        /// NotFound with message.
        /// </returns>
        [HttpGet("{prescriptionId}")]
        public IActionResult GetPrescriptionById(int prescriptionId)
        {
            try
            {
                Prescription prescription = this.service.GetById(prescriptionId);
                return this.Ok(prescription);
            }
            catch (ArgumentNullException ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Handles request POST /patient/drugs/add/
        /// Tries to add prescription record to database.
        /// Works in asynchronous mode.
        /// </summary>
        /// <param name="prescriptionForCreate">
        /// [FromBody] Drug is need to add to database.
        /// </param>
        /// <returns>
        /// Returns one of two action results.
        /// Created with id.
        /// ValidationProblem with the cause of validation error.
        /// </returns>
        [Route("add")]
        [HttpPost]
        public async Task<IActionResult> AddPrescription([FromBody] Prescription prescriptionForCreate)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ValidationProblem(this.ModelState);
            }

            Prescription prescription = await this.service.AddAsync(prescriptionForCreate);
            return this.Created("patient/drugs/", prescription.Id);
        }

        /// <summary>
        /// Handles request DELETE /patient/drugs/delete/{drugId}
        /// Perform soft delete of prescription record in database.
        /// Works in asynchronous mode.
        /// </summary>
        /// <param name="prescriptionId">The prescription identifier.</param>
        /// <returns>
        /// Returns one of two action results.
        /// Ok with deleted prescriptionId.
        /// BadRequest with exception message.
        /// </returns>
        [Route("delete/{prescriptionId}")]
        [HttpDelete]
        public async Task<IActionResult> RemovePrescription(int prescriptionId)
        {
            try
            {
                Prescription prescription = await this.service.DeleteAsync(prescriptionId);
                return this.Ok(prescription);
            }
            catch (ArgumentNullException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Handles request PUT /patient/drugs/edit/{drugId}
        /// Tries to update prescription record in database
        /// with specified id.
        /// Works in asynchronous mode.
        /// </summary>
        /// <param name="prescriptionId">The prescription identifier.</param>
        /// <param name="prescriptionForUpdate">
        /// [FromBody] Prescription which contains updated properties.
        /// </param>
        /// <returns>
        /// Returns one of three action results.
        /// Ok with updated prescription.
        /// BadRequest with exception message.
        /// ValidationProblem with the cause of validation error.
        [Route("edit/{prescriptionId}")]
        [HttpPut]
        public async Task<IActionResult> EditPrescription(int prescriptionId, Prescription prescriptionForUpdate)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ValidationProblem(this.ModelState);
            }

            try
            {
                Prescription prescription = await this.service.UpdateAsync(prescriptionId, prescriptionForUpdate);
                return this.Ok(prescription);
            }
            catch (ArgumentNullException ex)
            {
                return this.BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        // TODO: Change logic of this request
        // TODO: Add documentation
        [Route("edit/status/change/{prescriptionId}")]
        [HttpPut]
        public async Task<IActionResult> UpdatePrescriptionStatus(int prescriptionId)
        {
            try
            {
                await this.service.UpdateStatusAsync(prescriptionId);
                return this.Ok(this.service.GetById(prescriptionId));
            }
            catch (ArgumentNullException ex)
            {
                return this.NotFound(ex.Message);
            }
        }
    }
}