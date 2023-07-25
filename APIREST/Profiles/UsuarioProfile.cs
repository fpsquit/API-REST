using APIREST.Models.DTOS.PedidoDTOS;
using APIREST.Models;
using AutoMapper;
using BCryptNet = BCrypt.Net.BCrypt;

namespace APIREST.Profiles
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<CreateUsuarioDTO, Usuario>();
            CreateMap<Usuario, ReadUsuarioDTO>();
            CreateMap<UpdateUsuarioDTO, Usuario>()

            .ForMember(dest => dest.Nome, opt => opt.MapFrom((src, dest) => src.Nome != null ? src.Nome : dest.Nome))
            .ForMember(dest => dest.Cargo, opt => opt.MapFrom((src, dest) => src.Cargo != null ? src.Cargo : dest.Cargo))
            .ForMember(dest => dest.Senha, opt => opt.MapFrom((src, dest) => src.Senha != null ? BCryptNet.HashPassword(src.Senha) : dest.Senha));

            CreateMap<LoginUsuarioDTO, Usuario>();
        }

    }
}