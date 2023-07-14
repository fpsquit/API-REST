using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIREST.Data;
using APIREST.Data.DTOS.UsuarioDTOS;
using APIREST.Models;
using APIREST.Services;
using Microsoft.AspNetCore.Mvc;
using BCryptNet = BCrypt.Net.BCrypt;

namespace APIREST.Controllers
{

    [ApiController]
    [Route("api/login")]
    public class AutenticacaoController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly AppDbContext _context;

        public AutenticacaoController(TokenService tokenService, AppDbContext context)
        {
            _tokenService = tokenService;
            _context = context;
        }

        [HttpPost]
        public IActionResult Autenticar([FromBody] LoginUsuarioDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuarioAutenticado = _context.Usuario.FirstOrDefault(u => u.Nome == loginDTO.Nome);
            if (usuarioAutenticado != null && BCryptNet.Verify(loginDTO.Senha, usuarioAutenticado.Senha))
            {
                var token = _tokenService.GeradorToken(usuarioAutenticado);
                
                return Ok(new { Token = token });
            }

            return Unauthorized("Credenciais inválidas.");
        }
    }
}