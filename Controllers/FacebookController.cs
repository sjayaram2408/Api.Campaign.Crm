using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Api.Campaign.Crm.Models;
using DealerSocket.Crm.Integrations.FacebookAudience.Interfaces;
using DealerSocket.Crm.Integrations.FacebookAudience.Models;
using DealerSocket.Crm.Integrations.FacebookAudience.Services;
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
        public AudienceManager SampleAudienceManager => new AudienceManager
        {
            Account = new Account
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
                ConfigurationManager.AppSettings["FacebookAudience.AccessToken"] = _configuration["ApplicationSettings:Facebook:AccessToken"];
                ConfigurationManager.AppSettings["FacebookAudience.AppSecret"] = _configuration["ApplicationSettings:Facebook:AppSecret"];
                AudienceManager fbAudienceManager = new AudienceManager
                {
                    Account = new Account
                    {
                        Id = facebookAudienceManager.Account.Id,
                        AccessToken = facebookAudienceManager.Account.AccessToken,
                        CustomAudienceId = facebookAudienceManager.Account.CustomAudienceId
                    },
                    AddressId = facebookAudienceManager.AddressId,
                    Description = facebookAudienceManager.Description,
                    Name = facebookAudienceManager.Name,
                    SiteId = facebookAudienceManager.SiteId,
                    UseMock = facebookAudienceManager.UseMock
                };
                result = _audienceManagerService.CreateCustomAudience(fbAudienceManager);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
            }

            return new JsonResult(result);
        }
    }
}