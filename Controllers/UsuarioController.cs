using APIREST.Data;
using APIREST.Models;
using Microsoft.AspNetCore.Mvc;

namespace APIREST.Controllers
{

    [ApiController]
[Route ("api/usuario")]
 
 public class UsuarioController : ControllerBase 
 {

      private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

     [HttpPost]
        public IActionResult RegistrarUsuario([FromBody] Usuario usuario)
        {
            _context.Usuario.Add(usuario);
            _context.SaveChanges();
            return CreatedAtAction(nameof(RecuperarUsuarioPorId),
                new { id = usuario.Id },
                usuario);
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
            if (usuario == null) return NotFound();
            return Ok(usuario);

        }
     
   




 }

}