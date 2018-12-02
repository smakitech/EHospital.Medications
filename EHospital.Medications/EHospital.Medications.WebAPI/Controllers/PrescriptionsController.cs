using System;
using System.Collections.Generic;
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
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {
        /// <summary>
        /// Represents default route, which contain all REST requests.
        /// </summary>
        private const string DEFAULT_ROUTE = @"api/prescriptions/";

        /// <summary>
        /// Interface link on drug service.
        /// </summary>
        private readonly IPrescriptionService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrescriptionsController"/> class.
        /// </summary>
        /// <param name="service">The prescription service.</param>
        public PrescriptionsController(IPrescriptionService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Handles request [GET] api/prescriptions/details/{patientId}
        /// Retrieves prescriptions records from database in JSON format
        /// with extended details including information about drug and doctor
        /// specified by patient identifier.
        /// Works in asynchronous mode.
        /// </summary>
        /// <param name="patientId">
        /// Patient identifier.
        /// </param>
        /// <returns>
        /// Returns one of two action results.
        /// [Ok] with all prescriptions records with details in JSON format and [Status Code] 200.
        /// [NoContent] and [Status Code] 204.
        /// </returns>
        [HttpGet(DEFAULT_ROUTE + "details/{patientId}")]
        public async Task<IActionResult> GetPrescriptionsDetailsByPatientId(int patientId)
        {
            try
            {
                IEnumerable<PrescriptionDetails> result = await this.service.GetPrescriptionsDetails(patientId);
                return this.Ok(result);
            }
            catch (ArgumentException)
            {
                return this.NoContent();
            }
        }

        /// <summary>
        /// Handles request [GET] api/prescriptions/{id}
        /// Retrieves prescription record from database in JSON format
        /// by specified identifier.
        /// Works in asynchronous mode.
        /// </summary>
        /// <param name="id">
        /// Prescription identifier.
        /// </param>
        /// <returns>
        /// Returns one of two action results.
        /// [Ok] with concrete prescription in JSON format and [Status Code] 200.
        /// [NotFound] with message and [Status Code] 404.
        /// </returns>
        [HttpGet(DEFAULT_ROUTE + "{id}")]
        public async Task<IActionResult> GetPrescriptionById(int id)
        {
            try
            {
                Prescription result = await this.service.GetByIdAsync(id);
                return this.Ok(result);
            }
            catch (ArgumentException ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Handles request [GET] api/prescriptions/guide/{id}
        /// Retrieves drug instruction and doctors notes for
        /// specified prescription by identifier.
        /// Works in asynchronous mode.
        /// </summary>
        /// <param name="id">
        /// Prescription identifier.
        /// </param>
        /// <returns>
        /// Returns one of two action results.
        /// [Ok] with instruction and notes in JSON format and [Status Code] 200.
        /// [NotFound] with message and [Status Code] 404.
        /// </returns>
        [HttpGet(DEFAULT_ROUTE + "guide/{id}")]
        public async Task<IActionResult> GetPrescriptionGuideById(int id)
        {
            try
            {
                PrescriptionGuide result = await this.service.GetGuideById(id);
                return this.Ok(result);
            }
            catch (ArgumentException ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Handles request [POST] api/prescriptions/add
        /// Tries to add prescription record to database.
        /// Works in asynchronous mode.
        /// </summary>
        /// <param name="prescription">
        /// Prescription to add to the database. Binding [FromBody].
        /// </param>
        /// <returns>
        /// Returns one of three action results.
        /// [Created] with id of the created record and [Status Code] 201.
        /// [BadReques] with message and [Status Code] 400..
        /// [ValidationProblem] with the cause of validation error and [Status Code] 400.
        /// </returns>
        [HttpPost(DEFAULT_ROUTE + "add")]
        public async Task<IActionResult> AddPrescription([FromBody] Prescription prescription)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ValidationProblem(this.ModelState);
            }

            Prescription result = await this.service.AddAsync(prescription);
            return this.Created(DEFAULT_ROUTE, prescription.Id);
        }

        /// <summary>
        /// Handles request [PUT] api/prescriptions/edit/{id}
        /// Tries to update prescription record in database
        /// with specified id.
        /// Works in asynchronous mode.
        /// </summary>
        /// <param name="id">The prescription identifier.</param>
        /// <param name="prescription">
        /// Prescription which contains updated properties. Binding [FromBody].
        /// </param>
        /// <returns>
        /// Returns one of three action results.
        /// [Ok] with updated prescription and [Status Code] 200.
        /// [BadReques] with message and [Status Code] 400.
        /// [ValidationProblem] with the cause of validation error and [Status Code] 400.
        [HttpPut(DEFAULT_ROUTE + "edit/{id}")]
        public async Task<IActionResult> EditPrescription(int id, [FromBody] Prescription prescription)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ValidationProblem(this.ModelState);
            }

            try
            {
                Prescription result = await this.service.UpdateAsync(id, prescription);
                return this.Ok(result);
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Handles request [DELETE] api/prescriptions/remove/{id}
        /// Perform soft deletion of prescription record in database.
        /// Works in asynchronous mode.
        /// </summary>
        /// <param name="id">The prescription identifier.</param>
        /// <returns>
        /// Returns one of two action results.
        /// [Ok] with deleted prescription and [Status Code] 200.
        /// [BadReques] with message and [Status Code] 400.
        [HttpDelete(DEFAULT_ROUTE + "remove/{id}")]
        public async Task<IActionResult> RemovePrescription(int id)
        {
            try
            {
                Prescription prescription = await this.service.DeleteAsync(id);
                return this.Ok(prescription);
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Handles request [PUT] api/prescriptions/edit/status/{id}
        /// Manually update of prescription status to historic.
        /// Changes duration of prescription if it is need and status of prescription.
        /// Works in asynchronous mode.
        /// </summary>
        /// <param name="id">The prescription identifier.</param>
        /// <returns>
        /// Returns one of three action results.
        /// [Ok] with updated prescription and [Status Code] 200.
        /// [BadReques] with message and [Status Code] 400.
        [HttpPut(DEFAULT_ROUTE + "edit/status/{id}")]
        public async Task<IActionResult> EditPrescriptionStatus(int id)
        {
            try
            {
                Prescription prescription = await this.service.UpdateStatusAsync(id);
                return this.Ok(prescription);
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}