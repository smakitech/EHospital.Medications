using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EHospital.Medications.BusinessLogic.Contracts;
using EHospital.Medications.Model;

namespace EHospital.Medications.WebAPI.Controllers
{
    [Route("patient/drugs")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private const string NO_CONTENT = "Patient medications were not found.";

        private IPrescriptionService service;

        public PrescriptionController(IPrescriptionService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult GetPrescriptions()
        {
            IQueryable<Prescription> prescriptions = this.service.GetAll();
            if (!prescriptions.Any())
            {
                return this.NotFound(NO_CONTENT);
            }

            return Ok(prescriptions);
        }

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
                Prescription prescription =  await this.service.UpdateAsync(prescriptionId, prescriptionForUpdate);
                return this.Ok(prescription);
            }
            catch (ArgumentNullException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        // TODO: Change logic of this request
        [Route("edit/status/change/{prescriptionId}")]
        [HttpPut]
        public async Task<IActionResult> UpdatePrescriptionStatus(int prescriptionId)
        {
            try
            {
                await this.service.UpdateStatusAsync(prescriptionId);
                return Ok(this.service.GetById(prescriptionId));
            }
            catch (ArgumentNullException ex)
            {
                return this.NotFound(ex.Message);
            }
        }
    }
}