using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        private string UserName => HttpContext.User.Identity.Name;

        public MindMapController(IMapper mapper, IMindMappingService mindMappingService)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.mindMappingService = mindMappingService;
        }

        [HttpGet]
        public async Task<IEnumerable<NodeModel>> GetAsync()
        {
            IEnumerable<NodeDTO> nodeListDTO = await mindMappingService.GetMindMapsAsync(UserName);
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
                    Owner = UserName,
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
                    Owner = UserName
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
                Owner = UserName,
                RootId = request.Path.First(),
                SubPath = request.Path.Skip(1)
            };

            var nodeDTO = await mindMappingService.PutNodeAsync(requestDTO);

            var nodeModel = mapper.Map<NodeModel>(nodeDTO);
            return nodeModel;
        }

        [HttpDelete]
        public async Task DeleteAsync([FromBody] DeleteItemRequestModel request)
        {
            if (request.Path.Count() > 1)
            {
                DeleteNodeRequestDTO requestDTO = new()
                {
                    Owner = UserName,
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
                    Owner = UserName
                };

                await mindMappingService.DeleteMindMapAsync(requestDTO);
            }
        }
    }
}
