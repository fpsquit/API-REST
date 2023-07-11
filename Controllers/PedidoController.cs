using APIREST.Data;
using APIREST.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APIREST.Controllers
{

    [ApiController]
    [Route("api/pedido")]
    public class PedidoController : ControllerBase
    {

        private readonly AppDbContext _context;

        public PedidoController(AppDbContext context)
        {

            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CriarPedido([FromBody] string cnpj)
        {


            HttpClient httpClient = new HttpClient();

            var response = await httpClient.GetAsync($"https://receitaws.com.br/v1/cnpj/{cnpj}");
            if (response.IsSuccessStatusCode)
            {

                var jsonString = await response.Content.ReadAsStringAsync();
                ReceitaWSModel resultadoWS = JsonConvert.DeserializeObject<ReceitaWSModel>(jsonString);


                var novoPedido = new Pedido()
                {
                      CNPJ = cnpj,
                      Resultado = resultadoWS
                };
                var cnpjValido = novoPedido.ValidarCnpj(cnpj);
                if (!cnpjValido)
                {
                    throw new Exception("CNPJ INVALIDO");
                }

                _context.Pedido.Add(novoPedido);
                await _context.SaveChangesAsync();
                return Ok(novoPedido);
            }

            throw new Exception($"Falha ao acessar dados receitaWS {response.StatusCode}");

        }

    }
}