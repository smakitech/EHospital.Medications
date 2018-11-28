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
    [Route("prescriptions")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionService service;

        public PrescriptionController(IPrescriptionService service)
        {
            this.service = service;
        }

        [HttpGet("{id}/guide")]
        public IActionResult GetPrescriptionById(int id)
        {
            try
            {
                PrescriptionGuide guide = this.service.GetGuideByIdAsync(id);
                return this.Ok(guide);
            }
            catch (ArgumentNullException ex)
            {
                return this.NotFound(ex.Message);
            }
        }
    }
}