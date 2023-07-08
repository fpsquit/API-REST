using APIREST.Data;
using APIREST.Data.DTOS.UsuarioDTOS;
using APIREST.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIREST.Controllers
{
    [ApiController]
    [Route("api/usuario")]

    public class UsuarioController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UsuarioController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("registrar")]
        public IActionResult RegistrarUsuario([FromBody] CreateUsuarioDTO usuarioDTO)
        {
            Usuario usuario = _mapper.Map<Usuario>(usuarioDTO);
            _context.Usuario.Add(usuario);
            _context.SaveChanges();
            return CreatedAtAction(nameof(RecuperarUsuarioPorId),
                new { id = usuario.Id }, usuario);
        }

        [HttpGet]
        public IEnumerable<Usuario> RecuperarUsuario()
        {
            return _context.Usuario;
        }

        [HttpGet("{id}")]
        public IActionResult RecuperarUsuarioPorId(int id)
        {
            var usuario = _context.Usuario
                .FirstOrDefault(usuario => usuario.Id == id);
            if (usuario != null)
            {
                ReadUsuarioDTO usuarioDTO = _mapper.Map<ReadUsuarioDTO>(usuario);
                return Ok(usuarioDTO);
            } 
            return NotFound();

        }






    }

}