using AutoMapper;
using ProjectTest.Application.Compra.Commands.CreateCompra;
using ProjectTest.Application.Compra.Commands.UpdateCompra;
using ProjectTest.Application.DTO.ItemVenda;
using ProjectTest.Application.DTO.User;
using ProjectTest.Application.DTO.Venda;
using ProjectTest.Domain.Entities;

namespace ProjectTest.Application.AutoMapper
{
    public class AutoMapperSetup : Profile
    {
        public AutoMapperSetup()
        {
            #region DTOtoDomain

            CreateMap<UserDTO, Usuario>();
            CreateMap<UserModifyDTO, Usuario>();
            CreateMap<UserParmersDTO, Usuario>();
            CreateMap<CreateCompraCommand, Venda>();
            CreateMap<CreateItemVendaDTO, Domain.Entities.ItemVenda>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ItemId))
            .ForMember(dest => dest.NomeProduto, opt => opt.MapFrom(src => src.NomeProduto));

            CreateMap<UpdateCompraCommand, Venda>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.VendaId))
                .ForMember(dest => dest.Itens, opt => opt.MapFrom(src => src.Itens));

            CreateMap<CreateItemVendaDTO, Domain.Entities.ItemVenda>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ItemId));
            #endregion

            #region DomaintoDTO

            CreateMap<Usuario, UserDTO>();
            CreateMap<Usuario, UserModifyDTO>();
            CreateMap<Usuario, UserParmersDTO>();
            CreateMap<Venda, VendaDTO>();
            CreateMap<Domain.Entities.ItemVenda, ItemVendaDTO>()
    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
    .ForMember(dest => dest.NomeProduto, opt => opt.MapFrom(src => src.NomeProduto));
            #endregion

        }
    }
}
