using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Neodenit.MindMaker.Web.Server.Models;
using Neodenit.MindMaker.Web.Shared;

namespace Neodenit.MindMaker.Web.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MindMapController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IMindMappingService mindMappingService;
        private readonly UserManager<ApplicationUser> userManager;

        public MindMapController(IMapper mapper, IMindMappingService mindMappingService, UserManager<ApplicationUser> userManager)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.mindMappingService = mindMappingService;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IEnumerable<NodeModel>> GetAsync()
        {
            string userName = await GetUserName();
            IEnumerable<NodeDTO> nodeListDTO = await mindMappingService.GetMindMapsAsync(userName);
            var nodeListModel = mapper.Map<IEnumerable<NodeModel>>(nodeListDTO);
            return nodeListModel;
        }

        [HttpPost]
        public async Task<NodeModel> PostAsync([FromBody] CreateItemRequestModel request)
        {
            if (request.Path.Any())
            {
                CreateNodeRequestDTO requestDTO = new()
                {
                    NewNodeName = request.NewItemName,
                    Owner = await GetUserName(),
                    RootId = request.Path.First(),
                    SubPath = request.Path.Skip(1)
                };

                var nodeDTO = await mindMappingService.PostNodeAsync(requestDTO);
                var nodeModel = mapper.Map<NodeModel>(nodeDTO);
                return nodeModel;
            }
            else
            {
                CreateMindMapRequestDTO requestDTO = new()
                {
                    NewMindMapName = request.NewItemName,
                    Owner = await GetUserName()
                };

                NodeDTO nodeDTO = await mindMappingService.PostMindMapAsync(requestDTO);
                var nodeModel = mapper.Map<NodeModel>(nodeDTO);
                return nodeModel;
            }
        }

        [HttpPut]
        public async Task<NodeModel> PutAsync([FromBody] UpdateNodeRequestModel request)
        {
            UpdateNodeRequestDTO requestDTO = new()
            {
                UpdatedNodeName = request.UpdatedNodeName,
                Owner = await GetUserName(),
                RootId = request.Path.First(),
                SubPath = request.Path.Skip(1)
            };

            var nodeDTO = await mindMappingService.PutNodeAsync(requestDTO);

            var nodeModel = mapper.Map<NodeModel>(nodeDTO);
            return nodeModel;
        }

        [HttpPost("Delete")]
        public async Task DeleteAsync([FromBody] DeleteItemRequestModel request)
        {
            if (request.Path.Count() > 1)
            {
                DeleteNodeRequestDTO requestDTO = new()
                {
                    Owner = await GetUserName(),
                    RootId = request.Path.First(),
                    SubPath = request.Path.Skip(1)
                };

                await mindMappingService.DeleteNodeAsync(requestDTO);
            }
            else
            {
                DeleteMindMapRequestDTO requestDTO = new()
                {
                    MindMapId = request.Path.Single(),
                    Owner = await GetUserName()
                };

                await mindMappingService.DeleteMindMapAsync(requestDTO);
            }
        }

        private async Task<string> GetUserName()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await userManager.FindByIdAsync(userId);
            return user.UserName;
        }
    }
}
