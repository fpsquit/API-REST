using System.ComponentModel.DataAnnotations;

namespace APIREST.Models.DTOS.PedidoDTOS
{
    public class UpdateUsuarioDTO
    {

        public string? Nome { get; set; }
        [StringLength(256, ErrorMessage = "O {0} deve ter pelo menos {2} caracteres de comprimento.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string? Senha { get; set; }
        public string? Cargo { get; set; }

    }
}