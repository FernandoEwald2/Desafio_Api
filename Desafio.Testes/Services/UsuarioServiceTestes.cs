using Desafio.Api.Domain.Request;
using Desafio.Api.Domain.Response;
using Desafio.Api.Util;
using Desafio.Domain.Usuarios;
using Desafio.Services.Interfaces;
using Desafio.Util;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Desafio.Testes.Services
{
    public class UsuarioServiceTestes
    {
        private readonly Mock<IUsuarioService> _usuaioService;
        private readonly Mock<DbSet<Usuario>> _mockSet;

        public UsuarioServiceTestes() 
        {
            _usuaioService= new Mock<IUsuarioService>();
            _mockSet = new Mock<DbSet<Usuario>>();
        }

        [Fact]
        public async Task Criar_Usuario_Deve_Retornar_UsuarioResponse_Valido() 
        {
            // Arrange (Preparação)
            var request = new UsuarioRequest()
            {
                Login = "login_usuario",
                Nome = "usuario",
                Senha = "1234"
            };

            var respostaEsperada = new UsuarioResponse()
            {
                Id = 1,
                Login = "login_usuario",
                Nome = "usuario"
                
            };

            _usuaioService.Setup(service => service.Criar(It.IsAny<UsuarioRequest>()))
                              .ReturnsAsync(respostaEsperada); // Retorna um Task<UsuarioResponse>

            var UsuarioService = _usuaioService.Object;

            // Act (Ação)
            var retorno = await UsuarioService.Criar(request);

            // Assert (Validação)
            Assert.NotNull(retorno);
            Assert.Equal(respostaEsperada.Id, retorno.Id);
            Assert.Equal(respostaEsperada.Login, retorno.Login);
            Assert.Equal(respostaEsperada.Nome, retorno.Nome);
            // Verifica se o método Criar foi chamado
            _usuaioService.Verify(service => service.Criar(It.IsAny<UsuarioRequest>()), Times.Once);
        }
        [Fact]
        public async Task Excluir_UsuarioExistente_DeveRetornarTrue()
        {
            // Arrange
            var usuarioId = 1;
            byte[] hash;
            byte[] salt;
            Criptografia.CriarHashSalt("12345", out hash, out salt);

            var usuario = new Usuario("usuario", "login", hash, salt );

            // Configura o mock para retornar uma Usuario quando chamada FindAsync
            _mockSet.Setup(m => m.FindAsync(usuarioId)).ReturnsAsync(usuario);

            // Act
            _usuaioService.Setup(service => service.Excluir(usuarioId)).ReturnsAsync(true);
            var result = await _usuaioService.Object.Excluir(usuarioId);

            // Assert
            Assert.True(result);

            // Verifica se o método Excluir foi chamado
            _usuaioService.Verify(service => service.Excluir(usuarioId), Times.Once);
        }
        [Fact]
        public async Task Excluir_UsuarioNaoExistente_DeveRetornarFalse()
        {
            // Arrange
            var usuarioId = 1;
            byte[] hash;
            byte[] salt;
            Criptografia.CriarHashSalt("12345", out hash, out salt);

            var usuario = new Usuario("usuario", "login", hash, salt);

            // Configura o mock para retornar uma Usuario quando chamada FindAsync
            _mockSet.Setup(m => m.FindAsync(usuarioId)).ReturnsAsync(usuario);

            // Act
            _usuaioService.Setup(service => service.Excluir(usuarioId)).ReturnsAsync(false);
            var result = await _usuaioService.Object.Excluir(usuarioId);

            // Assert
            Assert.False(result);

            // Verifica se o método Excluir foi chamado
            _usuaioService.Verify(service => service.Excluir(usuarioId), Times.Once);

        }
        [Fact]
        public async Task Atualizar_UsuarioExistente_DeveRetornarUsuarioAtualizado()
        {
            
            // Arrange (Preparação)
            var request = new UsuarioRequest()
            {
                Login = "login_usuario",
                Nome = "usuario",
                Senha = "1234"
            };

            var respostaEsperada = new UsuarioResponse()
            {
                Id = 1,
                Login = "login_usuario",
                Nome = "usuario"

            };

            // Configura o mock para retornar a usuario atualizado
            _usuaioService.Setup(service => service.Criar(It.IsAny<UsuarioRequest>()))
                              .ReturnsAsync(respostaEsperada); // Retorna um Task<UsuarioResponse>

            var UsuarioService = _usuaioService.Object;

            // Act (Ação)
            var retorno = await UsuarioService.Criar(request);

            // Assert (Validação)
            Assert.NotNull(retorno);
            Assert.Equal(respostaEsperada.Id, retorno.Id);
            Assert.Equal(respostaEsperada.Login, retorno.Login);
            Assert.Equal(respostaEsperada.Nome, retorno.Nome);
            // Verifica se o método Criar foi chamado
            _usuaioService.Verify(service => service.Criar(It.IsAny<UsuarioRequest>()), Times.Once);
        }
        [Fact]
        public async Task Atualizar_UsuarioNaoExistente_DeveRetornarNull()
        {
            // Arrange
            var usuarioId = 1;
            var usuarioRequest = new UsuarioRequest
            { 
                Login = "login atualizado",
                Nome = "nome atualizado",
                Senha = "senha atualizada"
                
            };

            // Configura o mock para retornar null quando a Usuario não for encontrada
            _usuaioService.Setup(service => service.Atualizar(usuarioId, usuarioRequest))
                .ReturnsAsync((UsuarioResponse)null);

            // Act
            var result = await _usuaioService.Object.Atualizar(usuarioId, usuarioRequest);

            // Assert
            Assert.Null(result);  // Verifica se o retorno é null

        }
        [Fact]
        public async Task Obter_UsuarioPeloId_Deve_Retornar_dado_Valido()
        {
            var respostaEsperada = new UsuarioResponse()
            {
                Id = 1,
                Login = "login_usuario",
                Nome = "usuario"

            };
            // Configura o mock para retornar o dado esperado
            _usuaioService.Setup(service => service.ObterPorId(1)).ReturnsAsync(respostaEsperada);

            // Act            
            var retorno = await _usuaioService.Object.ObterPorId(1);

            // Assert (Validação)
            Assert.NotNull(retorno);
            Assert.Equal(respostaEsperada.Id, retorno.Id);
            Assert.Equal(respostaEsperada.Login, retorno.Login);
            Assert.Equal(respostaEsperada.Nome, retorno.Nome);
            // Verifica se o método Criar foi chamado
            _usuaioService.Verify(service => service.ObterPorId(1), Times.Once);
        }
        [Fact]
        public async Task Obter_Lista_Usuarios()
        {
            byte[] hash;
            byte[] salt;
            Criptografia.CriarHashSalt("12345", out hash, out salt);
            List<Usuario> usuarios = new List<Usuario>
                        {
                        new Usuario("usuario 1", "login 1", hash, salt),
                        new Usuario("usuario 2", "login 2", hash, salt),
                        new Usuario("usuario 3", "login 3", hash, salt),
                        new Usuario("usuario 4", "login 4", hash, salt),
                        new Usuario("usuario 5", "login 5", hash, salt)
                    };

            List<UsuarioResponse> listaResponse = usuarios.Select(t => new UsuarioResponse
            {
                Id = t.Id,
                Nome = t.Nome,
                Login = t.Login
            }).ToList();

            var resopnsePaginado = new Paginacao<UsuarioResponse>(listaResponse, 5, 1, 5);

            // Configura o mock para retornar o dado esperado
            _usuaioService.Setup(service => service.ObterTodos(1, 10)).ReturnsAsync(resopnsePaginado);

            var retorno = await _usuaioService.Object.ObterTodos(1, 10);

            Assert.NotNull(retorno);
            Assert.Equal(1, retorno.PaginaAtual);
            Assert.Equal(5, retorno.QuantidadePorPagina);
            Assert.Equal(5, retorno.TotalDeRegistros);
            Assert.Equal(1, retorno.TotalDePaginas);// O total de usuarios deve ser 5
            Assert.Equal(5, retorno.Items.Count); // Como estamos na primeira página e o tamanho da página é 5, esperamos 5 itens

            // Verificar se as usuarios retornadas são as esperadas (usuario 1 e usuario 2)
            Assert.Contains(retorno.Items, t => t.Nome == "usuario 1");
            Assert.Contains(retorno.Items, t => t.Nome == "usuario 2");


        }


    }
}
