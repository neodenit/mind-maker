using AutoMapper;
using Neodenit.MindMaker.Web.Shared;

namespace Neodenit.MindMaker.Web.Server
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AdviceRequestModel, AdviceRequestDTO>();

            CreateMap<NodeDTO, NodeModel>();

            CreateMap<CreateItemRequestModel, CreateMindMapRequestDTO>();
            CreateMap<CreateItemRequestModel, CreateNodeRequestDTO>();
            CreateMap<UpdateNodeRequestModel, UpdateNodeRequestDTO>();
            CreateMap<DeleteItemRequestModel, DeleteMindMapRequestDTO>();
            CreateMap<DeleteItemRequestModel, DeleteNodeRequestDTO>();
        }
    }
}
