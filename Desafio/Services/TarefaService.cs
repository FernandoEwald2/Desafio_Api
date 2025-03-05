using Desafio.Api.Domain.Request;
using Desafio.Api.Domain.Response;
using Desafio.Api.Services.Interfaces;
using Desafio.Api.Util;
using Desafio.Data;
using Desafio.Domain.Tarefas;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Desafio.Api.Services
{
    public class TarefaService : ITarefaService
    {
        private readonly DesafioDbContext _context;

        public TarefaService(DesafioDbContext context)
        {
            _context = context;
        }
        public async Task<Paginacao<TarefaResponse>> ObterTodas(int pageNumber, int pageSize, int usuario_id)
        {
            var totalRecords = await _context.Tarefas.CountAsync();
            List<Tarefa> listaTarefas = await _context.Tarefas
                .Where(t => t.UsuarioId == usuario_id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var response = listaTarefas.Select(t => new TarefaResponse
            {
                Id = t.Id,
                Concluido = t.Concluido,
                Descricao = t.Descricao,
                Titulo = t.Titulo,
                UsuarioId = t.UsuarioId
            }).ToList();

            return new Paginacao<TarefaResponse>(response, totalRecords, pageNumber, pageSize);
        }

        public async Task<TarefaResponse> ObterPorId(int id)
        {
            Tarefa tarefa = await _context.Tarefas.FindAsync(id);

            return new TarefaResponse
            {
                Id = tarefa.Id,
                Concluido = tarefa.Concluido,
                Descricao = tarefa.Descricao,
                Titulo = tarefa.Titulo
            };
        }

        public async Task<TarefaResponse> Criar(TarefaRequest tarefaRequest)
        {

            Tarefa tarefa = new Tarefa(tarefaRequest.Titulo.ToUpper(), tarefaRequest.Descricao.ToUpper(), tarefaRequest.Concluido, tarefaRequest.UsuarioId);
            _context.Tarefas.Add(tarefa);
            await _context.SaveChangesAsync();
            return new TarefaResponse
            {
                Id = tarefa.Id,
                Concluido = tarefa.Concluido,
                Descricao = tarefa.Descricao,
                Titulo = tarefa.Titulo,
                UsuarioId = tarefa.UsuarioId
            };
        }

        public async Task<TarefaResponse> Atualizar(int id, TarefaRequest tarefa)
        {
            var tarefaExistente = await _context.Tarefas.FindAsync(id);
            if (tarefaExistente == null) return null;

            tarefaExistente.AtualizarTarefa(tarefa.Titulo.ToUpper(), tarefa.Descricao.ToUpper(), tarefa.Concluido);

            await _context.SaveChangesAsync();
            return new TarefaResponse
            {
                Id = tarefaExistente.Id,
                Concluido = tarefaExistente.Concluido,
                Descricao = tarefaExistente.Descricao,
                Titulo = tarefaExistente.Titulo
            };
        }
        public async Task<bool> ConcluirTarefas(int[] ids, bool status)
        {
            List<Tarefa> tarefas = await _context.Tarefas.Where(x => ids.Contains(x.Id)).ToListAsync();
            if (!tarefas.Any())
                return true;

            foreach (var tarefa in tarefas)
            {
                tarefa.MarcarComoConcluida(status);
            }            

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Excluir(int id)
        {
            var tarefa = await _context.Tarefas.FindAsync(id);
            if (tarefa == null) return false;

            _context.Tarefas.Remove(tarefa);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
