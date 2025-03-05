using Desafio.Api.Domain.Request;
using Desafio.Api.Domain.Response;
using Desafio.Api.Services.Interfaces;
using Desafio.Api.Util;
using Desafio.Services.Interfaces;
using Desafio.Services.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Desafio.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IAuthService _authService;


        public UsuarioController(IUsuarioService usuarioService, IAuthService authService)
        {
            _usuarioService = usuarioService;
            _authService = authService;
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] Auth auth) 
        {
            var response =  _authService.Autenticar(auth);
            if(response == null)
                return Unauthorized(new { message = "Usuário ou senha inválidos" });

            return Ok(response);            

        }

       
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<Paginacao<UsuarioResponse>>> ObterTodos([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var usuarios = await _usuarioService.ObterTodos(pageNumber, pageSize);
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UsuarioResponse>> ObterPorId(int id)
        {
            var usuario = await _usuarioService.ObterPorId(id);
            if (usuario == null)
                return NotFound();

            return Ok(usuario);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<UsuarioResponse>> Criar(UsuarioRequest usuario)
        {
            var novoUsuario = await _usuarioService.Criar(usuario);
            return CreatedAtAction(nameof(Criar), new { id = novoUsuario.Id }, novoUsuario);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Atualizar(int id, UsuarioRequest usuario)
        {
            var atualizado = await _usuarioService.Atualizar(id, usuario);
            if (atualizado == null)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Excluir(int id)
        {
            var removido = await _usuarioService.Excluir(id);
            if (!removido)
                return NotFound();

            return NoContent();
        }
    }
}
