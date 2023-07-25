namespace APIREST.Models.DTOS.PedidoDTOS
{
    public class ReadPedidoDTO
    {
        public int Id { get; set; }
        public string CNPJ { get; set; }
        public ReceitaWSModel Resultado { get; set; }

    }
}