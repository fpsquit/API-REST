using APIREST.Data.DTOS.UsuarioDTOS;
using APIREST.Models;
using AutoMapper;

namespace APIREST.Profiles
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<CreateUsuarioDTO, Usuario>();
            CreateMap<Usuario, ReadUsuarioDTO>();
            CreateMap<UpdateUsuarioDTO, Usuario>();
        }

    }
}