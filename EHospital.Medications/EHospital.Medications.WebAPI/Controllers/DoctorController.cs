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
    [Route("[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IService<DoctorView> service;

        public DoctorController(IService<DoctorView> service)
        {
            this.service = service;
        }

        [HttpGet]
        public IQueryable<DoctorView> GetDoctors()
        {
            return this.service.GetAll();
        }
    }
}