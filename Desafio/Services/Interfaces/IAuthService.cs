using Desafio.Services.Model;

namespace Desafio.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Autenticacao Autenticar(Auth auth);
    }
}
