using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Api.Campaign.Crm.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class SerilogController : ControllerBase
    {
        private readonly ILogger<SerilogController> _logger;

        public SerilogController(ILogger<SerilogController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                throw new Exception("Serilog Testing");
            }
            catch (System.Exception ex)
            {
                this._logger.LogError(ex.Message);
            }
            return new JsonResult("Serilog Test");
        }
    }
}