using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace APIREST.Models
{
    public class Pedido
    {
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 20)]
        public int Id { get; set; }
        [Required(ErrorMessage = "O CNPJ é obrigatório.")]
        [StringLength(14, ErrorMessage = "O CNPJ deve ter 14 caracteres.")]
        public string CNPJ { get; set;}

        [Column(TypeName = "jsonb")]
        public ReceitaWSModel Resultado { get; set;}
        
    }
}