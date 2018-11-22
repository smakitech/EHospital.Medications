using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EHospital.Medications.BusinessLogic.Contracts;
using EHospital.Medications.Model;


namespace EHospital.Medications.WebAPI.Controllers
{
    [Route("drugs")]
    [ApiController]
    public class DrugController : ControllerBase
    {
        private const string NO_CONTENT = "Drugs were not found.";

        private readonly IDrugService service;

        public DrugController(IDrugService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult GetDrugs()
        {
            IQueryable<Drug> drugs = this.service.GetAll();
            if (!drugs.Any())
            {
                return this.NotFound(NO_CONTENT); ;
            }

            return Ok(drugs);
        }

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
                return Ok(drugs);
            }
            catch (ArgumentNullException ex)
            {
                return this.NotFound(ex.Message);
            }
        }

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
            catch(ArgumentException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

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