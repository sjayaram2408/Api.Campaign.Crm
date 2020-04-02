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

        public FacebookController(IAudienceManagerService audienceManagerService, ILogger<FacebookController> logger, IRepositoryWrapper repoWrapper)
        {
            _logger = logger;
            _audienceManagerService = audienceManagerService;
            _repoWrapper = repoWrapper;
        }

        [HttpPost]
        [Route("CreateCustomAudience")]
        public IActionResult CreateCustomAudienceIntegration([FromBody] FacebookAudienceManager facebookAudienceManager)
        {
            FacebookAudienceResponse result = new FacebookAudienceResponse();
            SiteAudience siteAudience = new SiteAudience();
            try
            {
                result = _audienceManagerService.CreateCustomAudienceIntegration(facebookAudienceManager);
                if (result.Error == null)
                {
                    siteAudience.SiteId = facebookAudienceManager.SiteId;
                    siteAudience.AudienceId = result.AudienceId;
                    siteAudience.AudienceName = result.AudienceName;
                    siteAudience.RetentionDays = result.RetentionDays;
                    siteAudience.CreatedDate = DateTimeExtensions.ToDateTimeFromEpoch(result.CreatedDate);
                    siteAudience.UpdatedDate = DateTimeExtensions.ToDateTimeFromEpoch(result.UpdatedDate);
                    _repoWrapper.SiteAudience.Create(siteAudience);
                    _repoWrapper.Save();
                }
                else 
                {
                    throw new Exception(result.Error.Type  + "|message-" + result.Error.Message + "|fbtrace_id-" + result.Error.Type);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
                return new JsonResult("Server Error");
            }

            return new JsonResult(siteAudience);
        }
    }
}