using AutoMapper;
using ECommerce.API.DataTransferring.Blogs;
using ECommerce.Application.Services.Blogs.Queries;
using ECommerce.Application.Services.Blogs.Results;
using ECommerce.Application.Services.Objects;
using ECommerce.Application.ViewModels;

namespace ECommerce.API.DataTransferObjectMappers;
public class BlogDtoMap : Profile
{
    public BlogDtoMap()
    {
        CreateMap<GetBlogsQueryDto, GetBlogsQuery>().ReverseMap();
        CreateMap<PagedList<BlogResult>, PagedList<BlogDto>>().ReverseMap();
    }
}

//CreateMap<BankStatementLineDto, BankStatementLine>()
//    .ForPath(
//        command => command.BankStatement,
//        o => o.MapFrom(dto => new BankStatementReference { Id = dto.BankStatementId }))
//    .ForMember(x => x.CreatedActor, opt => opt.Ignore())
//    .ForMember(x => x.UpdatedActor, opt => opt.Ignore())
//    .ForMember(x => x.CreatedDate, opt => opt.Ignore())
//    .ForMember(x => x.UpdatedDate, opt => opt.Ignore())
//    .ForMember(x => x.Status, cnf => cnf.MapFrom(x => x.Status))
//    .ReverseMap();
