using AutoMapper;
using Test.Domain.Entity;
using Test.Grpc.Protos;

namespace Test.Grpc.Mapping
{
    public class TestMapper:Profile
    {
        public TestMapper()
        {
            CreateMap<MemberInfo, GetAllResponse>().ReverseMap();
            CreateMap<MemberInfo, MemberModel>().ReverseMap();
            CreateMap<MemberInfo, CreateRequest>().ReverseMap();
            CreateMap<MemberInfo, MemberModel>().ReverseMap();
            CreateMap<UpdateRequest,MemberInfo>().ReverseMap();
            CreateMap<DeleteRequest, MemberInfo>().ReverseMap();
        }
    }
}
