using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Api.Campaign.Crm.Models;
using Api.Campaign.Crm.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Api.Campaign.Crm.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class FacebookController : ControllerBase
    {
        private IAudienceManagerService _audienceManagerService;
        private readonly ILogger<FacebookController> _logger;
        private IConfiguration _configuration;
        public FacebookAudienceManager SampleAudienceManager => new FacebookAudienceManager
        {
            Account = new FacebookAccount
            {
                Id = "exampleAdAccountId",
                AccessToken = "exampleAccessToken",
                CustomAudienceId = "exampleCustomAudienceId"
            },
            AddressId = 1,
            Description = "exampleDescription",
            Name = "exampleName",
            SiteId = 3,
            UseMock = true
        };

        public FacebookController(IAudienceManagerService audienceManagerService, ILogger<FacebookController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _audienceManagerService = audienceManagerService;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Get()
        {
            return new JsonResult(SampleAudienceManager);
        }

        [HttpPost]
        [Route("")]
        public IActionResult Post([FromBody] FacebookAudienceManager facebookAudienceManager)
        {
            string result = string.Empty;
            try
            {
                result = _audienceManagerService.CreateCustomAudience(facebookAudienceManager);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
            }

            return new JsonResult(result);
        }
    }
}