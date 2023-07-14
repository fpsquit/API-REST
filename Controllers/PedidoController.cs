using APIREST.Data;
using APIREST.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using APIREST.Helpers;
using Microsoft.EntityFrameworkCore;
using APIREST.Data.DTOS.PedidoDTOS;
using AutoMapper;

namespace APIREST.Controllers
{

    [ApiController]
    [Route("api/pedido")]
    public class PedidoController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PedidoController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CriarPedido([FromBody] CreatePedidoDTO pedidoDTO)
        {
            var cnpj = pedidoDTO.CNPJ;

            var pedidoHelper = new PedidoHelper();

            var cnpjSemPontos = pedidoHelper.RemoverPontosCnpj(cnpj);

            var cnpjValido = pedidoHelper.ValidarCnpj(cnpjSemPontos);

            if (!cnpjValido)
            {
                return BadRequest("CNPJ inválido");
            }


            bool cnpjJaExistente = await _context.Pedido.AnyAsync(pedido => pedido.CNPJ == cnpjSemPontos);
            if (cnpjJaExistente)
            {
                return BadRequest("CNPJ já existente no banco de dados");
            }

            HttpClient httpClient = new HttpClient();

            var response = await httpClient.GetAsync($"https://receitaws.com.br/v1/cnpj/{cnpjSemPontos}");
            if (response.IsSuccessStatusCode)
            {

                var jsonString = await response.Content.ReadAsStringAsync();
                ReceitaWSModel resultadoWS = JsonConvert.DeserializeObject<ReceitaWSModel>(jsonString);
                
                if (resultadoWS != null)
                {
                    var novoPedido = _mapper.Map<Pedido>(pedidoDTO);
                    novoPedido.CNPJ = cnpjSemPontos;
                    novoPedido.Resultado = resultadoWS;

                    _context.Pedido.Add(novoPedido);
                    await _context.SaveChangesAsync();

                    return Ok(novoPedido);
                }

               return StatusCode(500, "Falha ao obter dados da ReceitaWS: resultado nulo");

            }

            return StatusCode(500, $"Falha ao acessar dados da ReceitaWS: {response.StatusCode}");

        }

        [HttpGet]
        public async Task<IActionResult> ObterPedidos(int skip = 0, int take = 25)
        {
            var pedidos = await _context.Pedido.Skip(skip).Take(take).ToListAsync();
            var pedidosDTO = _mapper.Map<List<ReadPedidoDTO>>(pedidos);
            return Ok(pedidosDTO);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPedidosPorId(int id)
        {
            var pedido = await _context.Pedido.FindAsync(id);

            if (pedido == null)
            {
                return NotFound();
            }

            var pedidoDTO = _mapper.Map<ReadPedidoDTO>(pedido);

            return Ok(pedidoDTO.Resultado);

        }

        [HttpGet("cnpj/{cnpj}")]
        public async Task<IActionResult> ObeterPedidosPorCnpj(string cnpj)
        {

            var pedido = await _context.Pedido.FirstOrDefaultAsync(p => p.CNPJ == cnpj);

            if (pedido == null)
            {
                return NotFound("CNPJ Informado não encontrado no banco de dados");
            }

            var pedidoDTO = _mapper.Map<ReadPedidoDTO>(pedido);

            return Ok(pedidoDTO.Resultado);
        }

        [HttpDelete("deletar/{cnpj}")]

        public async Task<IActionResult> DeletarPedidoPorCnpj(string cnpj)
        {
            var pedido = await _context.Pedido.FindAsync(cnpj);

            if (pedido == null)
            {
                return NotFound("CNPJ Informado não encontrado no banco de dados");
            }

            _context.Pedido.Remove(pedido);
            await _context.SaveChangesAsync();

            return Ok();
        }










    }
}