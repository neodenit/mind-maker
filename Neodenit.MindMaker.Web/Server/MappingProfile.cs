using AutoMapper;
using Neodenit.MindMaker.Web.Shared;

namespace Neodenit.MindMaker.Web.Server
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AdviceRequestModel, AdviceRequestDTO>().ForMember(
                dest => dest.Randomness,
                opts => opts.MapFrom(x => x.Creativity / 100.0));

            CreateMap<NodeModel, NodeDTO>().ReverseMap();

            CreateMap<CreateItemRequestModel, CreateMindMapRequestDTO>();
            CreateMap<CreateItemRequestModel, CreateNodeRequestDTO>();
            CreateMap<UpdateNodeRequestModel, UpdateNodeRequestDTO>();
            CreateMap<DeleteItemRequestModel, DeleteMindMapRequestDTO>();
            CreateMap<DeleteItemRequestModel, DeleteNodeRequestDTO>();
        }
    }
}
