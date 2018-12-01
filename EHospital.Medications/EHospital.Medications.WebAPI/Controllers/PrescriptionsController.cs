using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EHospital.Medications.BusinessLogic.Contracts;
using EHospital.Medications.Model;

namespace EHospital.Medications.WebAPI.Controllers
{
    // TODO: PrescriptionsController - Add documentation
    // TODO: PrescriptionsController - Remove previous version
    // TODO: Handle Exceptions
    [Route("api/prescriptions")]
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IPrescriptionService service;

        public PrescriptionsController(IPrescriptionService service)
        {
            this.service = service;
        }

        [HttpGet("details/{patientId}")]
        public async Task<IActionResult> GetPrescriptionsDetailsByPatientId(int patientId)
        {
            // TODO: GetPrescriptionsDetailsByPatientId - Handle invalid id
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

        [HttpGet("{id}")]
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

        [HttpGet("guide/{id}")]
        public async Task<IActionResult> GetPrescriptionGuideById(int id)
        {
            // TODO: GetPrescriptionGuideById - Handle invalid id
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

        [HttpPost("add")]
        public async Task<IActionResult> AddPrescription([FromBody] Prescription prescription)
        {
            if (!this.ModelState.IsValid)
            {
                return this.ValidationProblem(this.ModelState);
            }

            Prescription result = await this.service.AddAsync(prescription);
            return this.Created("[controller]", prescription.Id);
        }

        [HttpPut("edit/{id}")]
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

        [HttpDelete("remove/{id}")]
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

        [HttpPut("edit/status/{id}")]
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