using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Api.Campaign.Crm.Interfaces;
using Api.Campaign.Crm.Models;
using Api.Campaign.Crm.Services.Interfaces;
using IdentityModel;
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
        private IRepositoryWrapper _repoWrapper;
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

        public FacebookController(IAudienceManagerService audienceManagerService, ILogger<FacebookController> logger, IRepositoryWrapper repoWrapper)
        {
            _logger = logger;
            _audienceManagerService = audienceManagerService;
            _repoWrapper = repoWrapper;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Get()
        {
            return new JsonResult(SampleAudienceManager);
        }

        [HttpPost]
        [Route("CreateCustomAudience")]
        public IActionResult CreateCustomAudience([FromBody] FacebookAudienceManager facebookAudienceManager)
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

        [HttpPost]
        [Route("CreateCustomAudienceIntegration")]
        public IActionResult CreateCustomAudienceIntegration([FromBody] FacebookAudienceManager facebookAudienceManager)
        {
            FacebookAudienceResponse result = new FacebookAudienceResponse();
            SiteAudience siteAudience = new SiteAudience();
            try
            {
                result = _audienceManagerService.CreateCustomAudienceIntegration(facebookAudienceManager);
                siteAudience.SiteId = facebookAudienceManager.SiteId;
                siteAudience.AudienceId = result.AudienceId;
                siteAudience.AudienceName = result.AudienceName;
                siteAudience.RetentionDays = result.RetentionDays;
                siteAudience.CreatedDate = DateTimeExtensions.ToDateTimeFromEpoch(result.CreatedDate);
                siteAudience.UpdatedDate = DateTimeExtensions.ToDateTimeFromEpoch(result.UpdatedDate);
                _repoWrapper.SiteAudience.Create(siteAudience);
                _repoWrapper.Save();
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
            }

            return new JsonResult(siteAudience);
        }
    }
}