using Desafio.Api.Domain.Request;
using Desafio.Api.Domain.Response;
using Desafio.Api.Util;
using System.Threading.Tasks;

namespace Desafio.Api.Services.Interfaces
{
    public interface ITarefaService
    {
        Task<Paginacao<TarefaResponse>> ObterTodas(int pageNumber, int pageSize, int usuario_id);
        Task<TarefaResponse> ObterPorId(int id);
        Task<TarefaResponse> Criar(TarefaRequest tarefa);
        Task<TarefaResponse> Atualizar(int id, TarefaRequest tarefa);
        Task<bool> Excluir(int id);
        Task<bool> ConcluirTarefas(int[] ids, bool status);
    }
}
