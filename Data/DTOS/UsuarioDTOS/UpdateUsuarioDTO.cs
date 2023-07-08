using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APIREST.Data.DTOS.UsuarioDTOS
{
    public class UpdateUsuarioDTO
    {

    [Required(ErrorMessage = "O Campo de Nome é obrigatório")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O Campo de Senha é obrigatório")]
    [StringLength(256, ErrorMessage = "O {0} deve ter pelo menos {2} caracteres de comprimento.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Senha { get; set; }

    public string Cargo { get; set; }

    }
}