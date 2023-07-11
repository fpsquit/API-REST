using APIREST.Data;
using APIREST.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using APIREST.Helpers;

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
                var pedidoHelper = new PedidoHelper();

                var cnpjSemPontos = pedidoHelper.RemoverPontosCnpj(cnpj);

                var cnpjValido = pedidoHelper.ValidarCnpj(cnpjSemPontos);

                if (!cnpjValido)
                {
                    return BadRequest("CNPJ inválido");
                }


            HttpClient httpClient = new HttpClient();

            var response = await httpClient.GetAsync($"https://receitaws.com.br/v1/cnpj/{cnpjSemPontos}");
            if (response.IsSuccessStatusCode)
            {

                var jsonString = await response.Content.ReadAsStringAsync();
                ReceitaWSModel resultadoWS = JsonConvert.DeserializeObject<ReceitaWSModel>(jsonString);


                var novoPedido = new Pedido()
                {
                      CNPJ = cnpjSemPontos,
                      Resultado = resultadoWS
                };

                _context.Pedido.Add(novoPedido);
                await _context.SaveChangesAsync();
                return Ok(novoPedido);

            }

            return BadRequest($"Falha ao acessar dados da ReceitaWS: {response.StatusCode}");

        }

    }
}