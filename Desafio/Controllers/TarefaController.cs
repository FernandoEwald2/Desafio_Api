using Desafio.Api.Domain.Request;
using Desafio.Api.Domain.Response;
using Desafio.Api.Services.Interfaces;
using Desafio.Api.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Desafio.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TarefaController : ControllerBase
    {
        private readonly ITarefaService _tarefaService;

        public TarefaController(ITarefaService tarefaService)
        {
            _tarefaService = tarefaService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<Paginacao<TarefaResponse>>> ObterTodas(int usuario_id, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
           
            var tarefas = await _tarefaService.ObterTodas(pageNumber, pageSize, usuario_id);
            return Ok(tarefas);
        }

        // Obter uma tarefa por ID
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<TarefaResponse>> ObterPorId(int id)
        {
            var tarefa = await _tarefaService.ObterPorId(id);
            if (tarefa == null)
                return NotFound("Tarefa não encontrada");

            return Ok(tarefa);
        }

        // Criar uma nova tarefa
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<TarefaResponse>> Criar([FromBody] TarefaRequest novaTarefa)
        {
            var tarefaCriada = await _tarefaService.Criar(novaTarefa);
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefaCriada.Id }, tarefaCriada);
        }

        // Atualizar uma tarefa existente
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> Atualizar(int id, [FromBody] TarefaRequest tarefaAtualizada)
        {
            var atualizado = await _tarefaService.Atualizar(id, tarefaAtualizada);
            if (atualizado == null)
                return NotFound();

            return NoContent();
        }

        [HttpPatch]
        [Authorize]
        public async Task<ActionResult> ConcluirTarefas([FromBody] int[] ids, bool status) 
        {  
            return Ok(await _tarefaService.ConcluirTarefas(ids, status));
        }


        // Excluir uma tarefa
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Excluir(int id)
        {
            var deletado = await _tarefaService.Excluir(id);
            if (!deletado)
                return NotFound("Tarefa não encontrada");

            return NoContent();
        }
    }
}
