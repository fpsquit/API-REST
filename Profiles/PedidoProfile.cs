using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIREST.Data.DTOS.PedidoDTOS;
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