using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using breezenetcore21.Contexts;
using breezenetcore21.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace breezenetcore21.Controllers
{
    [Route("breeze/[controller]/[action]")]
    [Authorize(Policy = "Admin")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository adminRepository;

        public AdminController(IAdminRepository adminRepository)
        {       
            this.adminRepository = adminRepository;
        }

        [HttpGet]
        public IActionResult Metadata()
        {
            return Ok(adminRepository.GetMetadata);
        }
    }
}