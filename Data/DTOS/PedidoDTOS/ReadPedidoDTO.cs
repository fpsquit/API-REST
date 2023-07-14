using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIREST.Models;

namespace APIREST.Data.DTOS.PedidoDTOS
{
    public class ReadPedidoDTO
    {
        public int Id { get; set; }
        public string CNPJ { get; set; }
        public ReceitaWSModel Resultado { get; set; }

    }
}