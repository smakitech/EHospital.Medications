using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EHospital.Medications.BusinessLogic.Contracts;
using EHospital.Medications.Model;

namespace EHospital.Medications.WebAPI.Controllers
{
    // TODO: DrugsController - Add documentation
    // TODO: DrugsController - Remove previous version
    // TODO: Handle Exceptions
    [Route("api/[controller]")]
    [ApiController]
    public class DrugsController : ControllerBase
    {
        private readonly IDrugService service;

        public DrugsController(IDrugService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetDrugs()
        {
            IEnumerable<Drug> result = await this.service.GetAllAsync();
            return this.Ok(result);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetDrugsByName([FromQuery] string name)
        {
            IEnumerable<Drug> result = await this.service.GetAllByNameAsync(name);
            return this.Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDrugById(int id)
        {
            Drug result = await this.service.GetByIdAsync(id);
            return this.Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddDrug([FromBody] Drug drug)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ValidationProblem(this.ModelState);
            }

            Drug result = await this.service.AddAsync(drug);
            return this.Ok(result);
        }

        [HttpPut("edit/{id}")]
        public async Task<IActionResult> EditDrug(int id, [FromBody] Drug drug)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ValidationProblem(this.ModelState);
            }

            Drug result = await this.service.UpdateAsync(id, drug);
            return this.Ok(result);
        }

        [HttpDelete("remove/{id}")]
        public async Task<IActionResult> RemoveDrug(int id)
        {
            Drug result = await this.service.DeleteAsync(id);
            return this.Ok(result);
        }
    }
}