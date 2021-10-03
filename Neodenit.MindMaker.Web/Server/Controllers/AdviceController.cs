using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public AdviceController(IMapper mapper, IAdviceService adviceService)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.adviceService = adviceService ?? throw new ArgumentNullException(nameof(adviceService));
        }

        [HttpPost("GetAdvice")]
        public async Task<IEnumerable<string>> GetAdviceAsync(AdviceRequestModel request)
        {
            if (request.Parents.Any())
            {
                var requestDTO = mapper.Map<AdviceRequestDTO>(request);

                IEnumerable<string> advice = await adviceService.GetAdviceAsync(requestDTO);
                return advice;
            }
            else
            {
                return Enumerable.Empty<string>();
            }
        }
    }
}
