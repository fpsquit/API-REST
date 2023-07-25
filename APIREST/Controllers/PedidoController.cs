using APIREST.Data;
using APIREST.Models;
using Microsoft.AspNetCore.Mvc;
using APIREST.Helpers;
using Microsoft.EntityFrameworkCore;
using APIREST.Models.DTOS.PedidoDTOS;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using APIREST.Services;

namespace APIREST.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/pedido")]
    public class PedidoController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IReceitaWSService _receitaWSService;


        public PedidoController(AppDbContext context, IMapper mapper, IReceitaWSService receitaws)
        {
            _context = context;
            _mapper = mapper;
            _receitaWSService = receitaws;
        }


        [HttpPost]
        public async Task<IActionResult> CriarPedido([FromBody] CreatePedidoDTO pedidoDTO)
        {
            var cnpj = pedidoDTO.CNPJ;

            var cnpjSemPontos = PedidoHelper.RemoverPontosCnpj(cnpj);

            var cnpjValido = PedidoHelper.ValidarCnpj(cnpjSemPontos);

            if (!cnpjValido)
            {
                return BadRequest("CNPJ inválido");
            }

            bool cnpjJaExistente = await _context.Pedido.AnyAsync(pedido => pedido.CNPJ == cnpjSemPontos);
            if (cnpjJaExistente)
            {
                return BadRequest("CNPJ já existente no banco de dados");
            }

            try
            {
                var resultadoWS = await _receitaWSService.ObterDadosReceitaWS(cnpjSemPontos);
                if (resultadoWS.Cnpj != null)
                {
                    var novoPedido = _mapper.Map<Pedido>(pedidoDTO);
                    novoPedido.CNPJ = cnpjSemPontos;
                    novoPedido.Resultado = resultadoWS;

                    _context.Pedido.Add(novoPedido);
                    await _context.SaveChangesAsync();

                    return Ok(novoPedido);
                }

                return StatusCode(500, "Falha ao obter dados da ReceitaWS: CNPJ não encontrado");

            }
            catch (HttpRequestException ex)
            {
                // Tratamento para exceção HttpRequestException (erro HTTP)
                return StatusCode(500, ex.Message);
            }

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

            return Ok(pedidoDTO);

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

            return Ok(pedidoDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarPedidoPorId(int id)
        {
            var pedido = await _context.Pedido.FindAsync(id);
            if (pedido == null)
            {
                return NotFound("Pedido não encontrado");
            }

            _context.Pedido.Remove(pedido);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("cnpj/{cnpj}")]

        public async Task<IActionResult> DeletarPedidoPorCnpj(string cnpj)
        {
            var pedido = await _context.Pedido.FirstOrDefaultAsync(p => p.CNPJ == cnpj);

            if (pedido == null)
            {
                return NotFound("CNPJ Informado não encontrado no banco de dados");
            }

            _context.Pedido.Remove(pedido);
            await _context.SaveChangesAsync();

            return NoContent();
        }










    }
}