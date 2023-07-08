using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIREST.Data.DTOS.UsuarioDTOS
{
    public class ReadUsuarioDTO
    {
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Cargo { get; set; }

    }
}