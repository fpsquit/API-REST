using APIREST.Models.DTOS.PedidoDTOS;
using APIREST.Models;
using AutoMapper;

namespace APIREST.Profiles
{
    public class PedidoProfile : Profile
    {
        public PedidoProfile()
        {
            CreateMap<CreatePedidoDTO, Pedido>();
            CreateMap<Pedido, ReadPedidoDTO>();

        }
    }
}