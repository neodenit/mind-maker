using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Neodenit.MindMaker.Web.Server.Helpers;
using Neodenit.MindMaker.Web.Server.Models;
using Neodenit.MindMaker.Web.Server.Services;
using Neodenit.MindMaker.Web.Shared;

namespace Neodenit.MindMaker.Web.Server.Controllers
{
    [Authorize]
    [Route("api")]
    [ApiController]
    public class AdviceController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IAdviceService adviceService;
        private readonly UserHelper userHelper;

        public AdviceController(IMapper mapper, IAdviceService adviceService, UserManager<ApplicationUser> userManager)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.adviceService = adviceService ?? throw new ArgumentNullException(nameof(adviceService));
            userHelper = new UserHelper(userManager);
        }

        [HttpPost("GetAdvice")]
        public async Task<IEnumerable<string>> GetAdviceAsync(AdviceRequestModel request)
        {
            if (!Creativity.Levels.Contains(request.Creativity))
            {
                throw new ArgumentException(nameof(request.Creativity));
            }

            if (request.Parents.Any())
            {
                var requestDTO = mapper.Map<AdviceRequestDTO>(request);

                requestDTO.Owner = await userHelper.GetUserNameAwait(User);

                IEnumerable<string> advice = await adviceService.GetAdviceAsync(requestDTO);
                return advice;
            }
            else
            {
                return Enumerable.Empty<string>();
            }
        }

        [HttpGet("GetPromptSettings")]
        public async Task<PromptSettingsModel> GetPromptSettingsAsync()
        {
            string owner = await userHelper.GetUserNameAwait(User);
            PromptSettingsDTO promptSettings = await adviceService.GetPromptSettingsAsync(owner);
            var model = mapper.Map<PromptSettingsModel>(promptSettings);
            return model;
        }
    }
}
