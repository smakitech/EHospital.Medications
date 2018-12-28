using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EHospital.Medications.BusinessLogic.Contracts;
using EHospital.Medications.Model;
using EHospital.Shared.AuditTrail;
using EHospital.Shared.AuditTrail.Models;
using EHospital.Shared.Authorization;
using EHospital.Shared.Configuration;
using EHospital.Shared.Logging;
using EHospital.Shared.Logging.Models;
using Microsoft.AspNetCore.Http.Extensions;

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
        /// The authentication details provider.
        /// </summary>
        private readonly IAuthDetailsProvider authDetailsProvider;

        /// <summary>
        /// The configuration provider.
        /// </summary>
        private readonly IConfigurationProvider configurationProvider;

        /// <summary>
        /// The logging provider.
        /// </summary>
        private readonly ILoggingProvider loggingProvider;

        /// <summary>
        /// The audit trail provider
        /// </summary>
        private readonly IAuditTrailProvider auditTrailProvider;

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
        /// <param name="authDetailsProvider">The authentication details provider.</param>
        /// <param name="configurationProvider">The configuration provider.</param>
        /// <param name="loggingProvider">The logging provider.</param>
        /// <param name="auditTrailProvider">The audit trail provider.</param>
        public PrescriptionsController(
            IPrescriptionService service,
            IAuthDetailsProvider authDetailsProvider,
            IConfigurationProvider configurationProvider,
            ILoggingProvider loggingProvider,
            IAuditTrailProvider auditTrailProvider)
        {
            this.service = service;
            this.authDetailsProvider = authDetailsProvider;
            this.configurationProvider = configurationProvider;
            this.loggingProvider = loggingProvider;
            this.auditTrailProvider = auditTrailProvider;
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
                var authInfo = await this.authDetailsProvider.GetUserAuthInfoAsync(this.configurationProvider.BaseUrl, this.Request.Headers["Authorization"]);
                if (authInfo == null)
                {
                    return this.Unauthorized();
                }

                IEnumerable<PrescriptionDetails> result = await this.service.GetPrescriptionsDetails(patientId);
                return this.Ok(result);
            }
            catch (ArgumentException)
            {
                return this.NoContent();
            }
            catch (Exception ex)
            {
                await this.loggingProvider.LogErrorMessage(this.configurationProvider.BaseUrl, new LoggingMessage
                {
                    ErrorMessage = ex.Message,
                    Exception = ex,
                    RequestType = HttpContext.Request.Method,
                    RequestUri = HttpContext.Request.GetDisplayUrl()
                });

                return this.BadRequest("Some error was thrown:" + ex.Message);
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
                var authInfo = await this.authDetailsProvider.GetUserAuthInfoAsync(this.configurationProvider.BaseUrl, this.Request.Headers["Authorization"]);
                if (authInfo == null)
                {
                    return this.Unauthorized();
                }

                Prescription result = await this.service.GetByIdAsync(id);
                return this.Ok(result);
            }
            catch (ArgumentException ex)
            {
                return this.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                await this.loggingProvider.LogErrorMessage(this.configurationProvider.BaseUrl, new LoggingMessage
                {
                    ErrorMessage = ex.Message,
                    Exception = ex,
                    RequestType = HttpContext.Request.Method,
                    RequestUri = HttpContext.Request.GetDisplayUrl()
                });

                return this.BadRequest("Some error was thrown:" + ex.Message);
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
                var authInfo = await this.authDetailsProvider.GetUserAuthInfoAsync(this.configurationProvider.BaseUrl, this.Request.Headers["Authorization"]);
                if (authInfo == null)
                {
                    return this.Unauthorized();
                }

                PrescriptionGuide result = await this.service.GetGuideById(id);
                return this.Ok(result);
            }
            catch (ArgumentException ex)
            {
                return this.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                await this.loggingProvider.LogErrorMessage(this.configurationProvider.BaseUrl, new LoggingMessage
                {
                    ErrorMessage = ex.Message,
                    Exception = ex,
                    RequestType = HttpContext.Request.Method,
                    RequestUri = HttpContext.Request.GetDisplayUrl()
                });

                return this.BadRequest("Some error was thrown:" + ex.Message);
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
            try
            {
                var authInfo = await this.authDetailsProvider.GetUserAuthInfoAsync(this.configurationProvider.BaseUrl, this.Request.Headers["Authorization"]);
                if (authInfo == null)
                {
                    return this.Unauthorized();
                }

                if (!this.ModelState.IsValid)
                {
                    return this.ValidationProblem(this.ModelState);
                }

                Prescription result = await this.service.AddAsync(prescription);

                await this.auditTrailProvider.LogAuditTrailMessage(this.configurationProvider.BaseUrl, this.Request.Headers["Authorization"], new AuditTrailModel
                {
                    ActionItem = result.GetType().Name,
                    ActionType = ActionMode.Create,
                    Description = $"Prescription has been created with id = {result.Id}.",
                    UserId = authInfo.Id,
                    ItemId = result.Id,
                    ItemState = result,
                    Module = "Medications.API"
                });

                return this.Created(DEFAULT_ROUTE, prescription.Id);
            }
            catch (Exception ex)
            {
                await this.loggingProvider.LogErrorMessage(this.configurationProvider.BaseUrl, new LoggingMessage
                {
                    ErrorMessage = ex.Message,
                    Exception = ex,
                    RequestType = HttpContext.Request.Method,
                    RequestUri = HttpContext.Request.GetDisplayUrl()
                });

                return this.BadRequest("Some error was thrown:" + ex.Message);
            }

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
        /// </returns>
        [HttpPut(DEFAULT_ROUTE + "edit/{id}")]
        public async Task<IActionResult> EditPrescription(int id, [FromBody] Prescription prescription)
        {
            try
            {
                var authInfo = await this.authDetailsProvider.GetUserAuthInfoAsync(this.configurationProvider.BaseUrl, this.Request.Headers["Authorization"]);
                if (authInfo == null)
                {
                    return this.Unauthorized();
                }

                if (!this.ModelState.IsValid)
                {
                    return this.ValidationProblem(this.ModelState);
                }

                Prescription result = await this.service.UpdateAsync(id, prescription);

                await this.auditTrailProvider.LogAuditTrailMessage(this.configurationProvider.BaseUrl, this.Request.Headers["Authorization"], new AuditTrailModel
                {
                    ActionItem = result.GetType().Name,
                    ActionType = ActionMode.Update,
                    Description = $"Prescription with id = {result.Id} has been updated.",
                    UserId = authInfo.Id,
                    ItemId = result.Id,
                    ItemState = result,
                    Module = "Medications.API"
                });

                return this.Ok(result);
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                await this.loggingProvider.LogErrorMessage(this.configurationProvider.BaseUrl, new LoggingMessage
                {
                    ErrorMessage = ex.Message,
                    Exception = ex,
                    RequestType = HttpContext.Request.Method,
                    RequestUri = HttpContext.Request.GetDisplayUrl()
                });

                return this.BadRequest("Some error was thrown:" + ex.Message);
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
        /// </returns>
        [HttpDelete(DEFAULT_ROUTE + "remove/{id}")]
        public async Task<IActionResult> RemovePrescription(int id)
        {
            try
            {
                var authInfo = await this.authDetailsProvider.GetUserAuthInfoAsync(this.configurationProvider.BaseUrl, this.Request.Headers["Authorization"]);
                if (authInfo == null)
                {
                    return this.Unauthorized();
                }

                Prescription result = await this.service.DeleteAsync(id);

                await this.auditTrailProvider.LogAuditTrailMessage(this.configurationProvider.BaseUrl, this.Request.Headers["Authorization"], new AuditTrailModel
                {
                    ActionItem = result.GetType().Name,
                    ActionType = ActionMode.Delete,
                    Description = $"Prescription with id = {result.Id} has been soft deleted.",
                    UserId = authInfo.Id,
                    ItemId = result.Id,
                    ItemState = result,
                    Module = "Medications.API"
                });

                return this.Ok(result);
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                await this.loggingProvider.LogErrorMessage(this.configurationProvider.BaseUrl, new LoggingMessage
                {
                    ErrorMessage = ex.Message,
                    Exception = ex,
                    RequestType = HttpContext.Request.Method,
                    RequestUri = HttpContext.Request.GetDisplayUrl()
                });

                return this.BadRequest("Some error was thrown:" + ex.Message);
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
        /// </returns>
        [HttpPut(DEFAULT_ROUTE + "edit/status/{id}")]
        public async Task<IActionResult> EditPrescriptionStatus(int id)
        {
            try
            {
                var authInfo = await this.authDetailsProvider.GetUserAuthInfoAsync(this.configurationProvider.BaseUrl, this.Request.Headers["Authorization"]);
                if (authInfo == null)
                {
                    return this.Unauthorized();
                }

                Prescription result = await this.service.UpdateStatusAsync(id);

                await this.auditTrailProvider.LogAuditTrailMessage(this.configurationProvider.BaseUrl, this.Request.Headers["Authorization"], new AuditTrailModel
                {
                    ActionItem = result.GetType().Name,
                    ActionType = ActionMode.Update,
                    Description = $"Prescription status has been updated. Id = {result.Id}.",
                    UserId = authInfo.Id,
                    ItemId = result.Id,
                    ItemState = result,
                    Module = "Medications.API"
                });

                return this.Ok(result);
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                await this.loggingProvider.LogErrorMessage(this.configurationProvider.BaseUrl, new LoggingMessage
                {
                    ErrorMessage = ex.Message,
                    Exception = ex,
                    RequestType = HttpContext.Request.Method,
                    RequestUri = HttpContext.Request.GetDisplayUrl()
                });

                return this.BadRequest("Some error was thrown:" + ex.Message);
            }
        }
    }
}