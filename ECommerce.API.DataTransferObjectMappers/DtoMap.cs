using AutoMapper;

namespace ECommerce.API.DataTransferObjectMappers;

public class DtoMap : Profile
{

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
