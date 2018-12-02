using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EHospital.Medications.BusinessLogic.Contracts;
using EHospital.Medications.BusinessLogic.Services;
using EHospital.Medications.Model;

namespace EHospital.Medications.WebAPI.Controllers
{
    /// <summary>
    /// Represents drug controller.
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [Route("api/drugs")]
    [ApiController]
    public class DrugsController : ControllerBase
    {
        /// <summary>
        /// Interface link on drug service.
        /// </summary>
        private readonly IDrugService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrugsController"/> class.
        /// </summary>
        /// <param name="service">The drug service.</param>
        public DrugsController(IDrugService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Handles request [GET] api/drugs
        /// Retrieves all drugs records from database in JSON format.
        /// Works in asynchronous mode.
        /// </summary>
        /// <returns>
        /// Returns one of two action results.
        /// [Ok] with all drugs records in JSON format and [Status Code] 200.
        /// [NoContent] and [Status Code] 204.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetDrugs()
        {
            try
            {
                IEnumerable<Drug> result = await this.service.GetAllAsync();
                return this.Ok(result);
            }
            catch (NoContentException)
            {
                return this.NoContent();
            }
        }

        /// <summary>
        /// Handles request [GET] api/drugs/filter?{name}
        /// Performs search for records by drug name.
        /// Retrieves drug record from database in JSON format.
        /// Works in asynchronous mode.
        /// </summary>
        /// <param name="name">
        /// Drug name. Binding [FromQuery].
        /// </param>
        /// <returns>
        /// Returns one of two action results.
        /// [Ok] with drugs match the specified name in JSON format and [Status Code] 200.
        /// [NoContent] and [Status Code] 204.
        /// </returns>
        [HttpGet("filter")]
        public async Task<IActionResult> GetDrugsByName([FromQuery] string name)
        {
            try
            {
                IEnumerable<Drug> result = await this.service.GetAllByNameAsync(name);
                return this.Ok(result);
            }
            catch (NoContentException)
            {
                return this.NoContent();
            }
        }

        /// <summary>
        /// Handles request [GET] api/drugs/{id}
        /// Retrieves drug record from database in JSON format
        /// specified by identifier.
        /// Works in asynchronous mode.
        /// </summary>
        /// <param name="id">The drug identifier.</param>
        /// <returns>
        /// Returns one of two action results.
        /// [Ok] with concrete drug in JSON format and [Status Code] 200.
        /// [NotFound] with message and [Status Code] 404.
        /// </returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDrugById(int id)
        {
            try
            {
                Drug result = await this.service.GetByIdAsync(id);
                return this.Ok(result);
            }
            catch (ArgumentException ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Handles request [POST] api/drugs/add/
        /// Tries to add drug record to database.
        /// Works in asynchronous mode.
        /// </summary>
        /// <param name="drug">
        /// Drug to add to the database. Binding [FromBody].
        /// </param>
        /// <returns>
        /// Returns one of three action results.
        /// [Created] with id of the created record and [Status Code] 201.
        /// [BadReques] with message and [Status Code] 400..
        /// [ValidationProblem] with the cause of validation error and [Status Code] 400.
        /// </returns>
        [HttpPost("add")]
        public async Task<IActionResult> AddDrug([FromBody] Drug drug)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ValidationProblem(this.ModelState);
            }

            try
            {
                Drug result = await this.service.AddAsync(drug);
                // TODO: extract this route string somehow
                return this.Created("api/drugs", result.Id);
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Handles request [PUT] api/drugs/edit/{id}
        /// Tries to update drug record in database
        /// with specified id.
        /// Works in asynchronous mode.
        /// </summary>
        /// <param name="id">The drug identifier.</param>
        /// <param name="drug">
        /// Drug which contains updated properties. Binding [FromBody].
        /// </param>
        /// <returns>
        /// Returns one of three action results.
        /// [Ok] with updated drug and [Status Code] 200.
        /// [BadReques] with message and [Status Code] 400.
        /// [ValidationProblem] with the cause of validation error and [Status Code] 400.
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> EditDrug(int id, [FromBody] Drug drug)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ValidationProblem(this.ModelState);
            }

            try
            {
                Drug result = await this.service.UpdateAsync(id, drug);
                return this.Ok(result);
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Handles request [DELETE] api/drugs/remove/{id}
        /// Perform soft deletion of drug record in database.
        /// Works in asynchronous mode.
        /// </summary>
        /// <param name="id">The drug identifier.</param>
        /// <returns>
        /// Returns one of two action results.
        /// [Ok] with deleted drug and [Status Code] 200.
        /// [BadReques] with message and [Status Code] 400.
        [HttpDelete("remove/{id}")]
        public async Task<IActionResult> RemoveDrug(int id)
        {
            try
            {
                Drug result = await this.service.DeleteAsync(id);
                return this.Ok(result);
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}