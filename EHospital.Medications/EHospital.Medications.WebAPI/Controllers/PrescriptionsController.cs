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
    [Route("[Controller]")]
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IPrescriptionService service;

        public PrescriptionsController(IPrescriptionService service)
        {
            this.service = service;
        }

        [Route("details/{patientId}")]
        [HttpGet]
        public async Task<IActionResult> GetPrescriptionsDetailsByPatientId(int patientId)
        {
            try
            {
                IEnumerable<PrescriptionDetails> result = await this.service.GetPrescriptionsDetails(patientId);
                return this.Ok(result);
            }
            catch (ArgumentNullException ex)
            {
                return this.NotFound(ex.Message);
            }
        }
    }
}