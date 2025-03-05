using Desafio.Api.Domain.Request;
using Desafio.Api.Domain.Response;
using Desafio.Api.Util;
using System.Threading.Tasks;

namespace Desafio.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<Paginacao<UsuarioResponse>> ObterTodos(int pageNumber, int pageSize);
        Task<UsuarioResponse> ObterPorId(int id);
        Task<UsuarioResponse> Criar(UsuarioRequest usuario);
        Task<UsuarioResponse> Atualizar(int id, UsuarioRequest usuario);
        Task<bool> Excluir(int id);        
    }
}
