using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EHospital.Medications.BusinessLogic.Contracts;
using EHospital.Medications.Model;

namespace EHospital.Medications.WebAPI.Controllers
{
    /// <summary>
    /// Represents drug controller.
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [Route("drugs")]
    [ApiController]
    public class DrugController : ControllerBase
    {
        /// <summary>
        /// Response message in case drugs were not found.
        /// </summary>
        private const string NO_CONTENT = "Drugs were not found.";

        /// <summary>
        /// Interface link on drug service.
        /// </summary>
        private readonly IDrugService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrugController"/> class.
        /// </summary>
        /// <param name="service">Drug service.</param>
        public DrugController(IDrugService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Handles request GET /drugs/
        /// Handles request GET /drugs
        /// Retrieves all drugs records from database in JSON format.
        /// </summary>
        /// <returns>
        /// Returns one of two action results.
        /// Ok with all drugs records in JSON format.
        /// NotFound with message.
        /// </returns>
        [HttpGet]
        public IActionResult GetDrugs()
        {
            IQueryable<Drug> drugs = this.service.GetAll();
            if (!drugs.Any())
            {
                return this.NotFound(NO_CONTENT);
            }

            return this.Ok(drugs);
        }

        /// <summary>
        /// Handles request GET /drugs/{drugId}
        /// Retrieves drug record from database in JSON format
        /// specified by identifier.
        /// </summary>
        /// <param name="drugId">The drug identifier.</param>
        /// <returns>
        /// Returns one of two action results.
        /// Ok with concrete drug in JSON format.
        /// NotFound with message.
        /// </returns>
        [HttpGet("{drugId}")]
        public IActionResult GetDrugById(int drugId)
        {
            try
            {
                Drug drug = this.service.GetById(drugId);
                return this.Ok(drug);
            }
            catch (ArgumentException ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Handles request GET /drugs/filter?{drugName} [FromQuery]
        /// Retrieves drug record from database in JSON format
        /// specified by name.
        /// </summary>
        /// <param name="drugName">Drug name.</param>
        /// <returns>
        /// Returns one of two action results.
        /// Ok with drugs match the drugName in JSON format.
        /// NotFound with message.
        /// </returns>
        [Route("filter")]
        [HttpGet]
        public IActionResult GetDrugsByName([FromQuery] string drugName)
        {
            try
            {
                IQueryable<Drug> drugs = this.service.GetAllByName(drugName);
                if (!drugs.Any())
                {
                    return this.NotFound(NO_CONTENT);
                }

                return this.Ok(drugs);
            }
            catch (ArgumentNullException ex)
            {
                return this.NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Handles request POST /drugs/add/
        /// Tries to add drug record to database.
        /// Works in asynchronous mode.
        /// </summary>
        /// <param name="drugForCreate">
        /// [FromBody] Drug is need to add to database.
        /// </param>
        /// <returns>
        /// Returns one of three action results.
        /// Created with id.
        /// BadRequest with exception message.
        /// ValidationProblem with the cause of validation error.
        /// </returns>
        [Route("add")]
        [HttpPost]
        public async Task<IActionResult> AddDrug([FromBody] Drug drugForCreate)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ValidationProblem(this.ModelState);
            }

            try
            {
                Drug drug = await this.service.AddAsync(drugForCreate);
                return this.Created("drugs/", drug.Id);
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Handles request DELETE /drugs/delete/{drugId}
        /// Perform soft delete of drug record in database.
        /// Works in asynchronous mode.
        /// </summary>
        /// <param name="drugId">The drug identifier.</param>
        /// <returns>
        /// Returns one of two action results.
        /// Ok with deleted drug.
        /// BadRequest with exception message.
        /// </returns>
        [Route("delete/{drugId}")]
        [HttpDelete]
        public async Task<IActionResult> RemoveDrug(int drugId)
        {
            try
            {
                Drug drug = await this.service.DeleteAsync(drugId);
                return this.Ok(drug);
            }
            catch (ArgumentNullException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Handles request PUT /drugs/edit/{drugId}
        /// Tries to update drug record in database
        /// with specified id.
        /// Works in asynchronous mode.
        /// </summary>
        /// <param name="drugId">The drug identifier.</param>
        /// <param name="drugForUpdate">
        /// [FromBody] Drug which contains updated properties.
        /// </param>
        /// <returns>
        /// Returns one of three action results.
        /// Ok with updated drug.
        /// BadRequest with exception message.
        /// ValidationProblem with the cause of validation error.
        [Route("edit/{drugId}")]
        [HttpPut]
        public async Task<IActionResult> EditDrug(int drugId, Drug drugForUpdate)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ValidationProblem(this.ModelState);
            }

            try
            {
                Drug drug = await this.service.UpdateAsync(drugId, drugForUpdate);
                return this.Ok(drug);
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}