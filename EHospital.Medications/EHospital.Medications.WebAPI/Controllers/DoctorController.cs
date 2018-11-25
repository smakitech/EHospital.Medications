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
    // TODO : view temp
    [Route("[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService service;

        public DoctorController(IDoctorService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IQueryable<DoctorView> GetDoctors()
        {
            return this.service.GetDoctors();
        }
    }
}