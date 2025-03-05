using Desafio.Api.Domain.Request;
using Desafio.Api.Domain.Response;
using Desafio.Api.Services.Interfaces;
using Desafio.Api.Util;
using Desafio.Domain.Tarefas;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Desafio.Testes.Services
{
    public class TarefaServiceTestes
    {
        private readonly Mock<ITarefaService> _tarefaServiceMock;
        private readonly Mock<DbSet<Tarefa>> _mockSet;       

        public TarefaServiceTestes()
        {
            _mockSet = new Mock<DbSet<Tarefa>>();           
            _tarefaServiceMock = new Mock<ITarefaService>();
        }
        
        [Fact]
        public async Task Criar_Tarefa_Deve_Retornar_TarefaResponse_Valida()
        {
            // Arrange (Preparação)
            var request = new TarefaRequest()
            {
                Concluido = false,
                Descricao = "descrição",
                Titulo = "Titulo",
                UsuarioId = 111
            };

            var respostaEsperada = new TarefaResponse()
            {
                Id = 1,
                Concluido = false,
                Descricao = "descrição",
                Titulo = "Titulo",
                UsuarioId = 111
            };

            _tarefaServiceMock.Setup(service => service.Criar(It.IsAny<TarefaRequest>()))
                              .ReturnsAsync(respostaEsperada); // Retorna um Task<TarefaResponse>

            var tarefaService = _tarefaServiceMock.Object;

            // Act (Ação)
            var retorno = await tarefaService.Criar(request);

            // Assert (Validação)
            Assert.NotNull(retorno);
            Assert.Equal(respostaEsperada.Id, retorno.Id);
            Assert.Equal(respostaEsperada.Concluido, retorno.Concluido);
            Assert.Equal(respostaEsperada.Descricao, retorno.Descricao);
            Assert.Equal(respostaEsperada.Titulo, retorno.Titulo);
            Assert.Equal(respostaEsperada.UsuarioId, retorno.UsuarioId);
            // Verifica se o método Criar foi chamado
            _tarefaServiceMock.Verify(service => service.Criar(It.IsAny<TarefaRequest>()), Times.Once);
        }

        [Fact]
        public async Task Excluir_TarefaExistente_DeveRetornarTrue()
        {
            // Arrange
            var tarefaId = 1;
            var tarefa = new Tarefa("Tarefa 1", "Descrição da Tarefa", false, 111);

            // Configura o mock para retornar uma tarefa quando chamada FindAsync
            _mockSet.Setup(m => m.FindAsync(tarefaId)).ReturnsAsync(tarefa);

            // Act
            _tarefaServiceMock.Setup(service => service.Excluir(tarefaId)).ReturnsAsync(true);
            var result = await _tarefaServiceMock.Object.Excluir(tarefaId);

            // Assert
            Assert.True(result);

            // Verifica se o método Excluir foi chamado
            _tarefaServiceMock.Verify(service => service.Excluir(tarefaId), Times.Once);

        }

        [Fact]
        public async Task Excluir_TarefaNaoExistente_DeveRetornarFalse()
        {
            // Arrange
            var tarefaId = 1;
            var tarefa = new Tarefa("Tarefa 1", "Descrição da Tarefa", false, 111);

            // Configura o mock para retornar uma tarefa quando chamada FindAsync
            _mockSet.Setup(m => m.FindAsync(tarefaId)).ReturnsAsync(tarefa);

            // Act
            _tarefaServiceMock.Setup(service => service.Excluir(tarefaId)).ReturnsAsync(false);
            var result = await _tarefaServiceMock.Object.Excluir(tarefaId);

            // Assert
            Assert.False(result);

            // Verifica se o método Excluir foi chamado
            _tarefaServiceMock.Verify(service => service.Excluir(tarefaId), Times.Once);

        }

        [Fact]
        public async Task Atualizar_TarefaExistente_DeveRetornarTarefaAtualizada()
        {
            // Arrange
            var tarefaId = 1;
            var tarefaRequest = new TarefaRequest { Titulo = "Tarefa Atualizada", Descricao = "Descrição Atualizada" };

            var tarefaResponse = new TarefaResponse
            {
                Id = tarefaId,
                Titulo = "Tarefa Atualizada",
                Descricao = "Descrição Atualizada"
            };

            // Configura o mock para retornar a tarefa atualizada
            _tarefaServiceMock.Setup(service => service.Atualizar(tarefaId, tarefaRequest))
                .ReturnsAsync(tarefaResponse);

            // Act
            var result = await _tarefaServiceMock.Object.Atualizar(tarefaId, tarefaRequest);

            // Assert
            Assert.NotNull(result);  // Verifica se o retorno não é nulo
            Assert.Equal(tarefaId, result.Id);  // Verifica se o ID da tarefa é o mesmo
            Assert.Equal(tarefaRequest.Titulo, result.Titulo);  // Verifica se o título foi atualizado
            Assert.Equal(tarefaRequest.Descricao, result.Descricao);  // Verifica se a descrição foi atualizada

            // Verifica se o método Atualizar foi chamado
            _tarefaServiceMock.Verify(service => service.Atualizar(tarefaId, tarefaRequest), Times.Once);
        }

        [Fact]
        public async Task Atualizar_TarefaNaoExistente_DeveRetornarNull()
        {
            // Arrange
            var tarefaId = 1;
            var tarefaRequest = new TarefaRequest { Titulo = "Tarefa Atualizada", Descricao = "Descrição Atualizada" };

            // Configura o mock para retornar null quando a tarefa não for encontrada
            _tarefaServiceMock.Setup(service => service.Atualizar(tarefaId, tarefaRequest))
                .ReturnsAsync((TarefaResponse)null);

            // Act
            var result = await _tarefaServiceMock.Object.Atualizar(tarefaId, tarefaRequest);

            // Assert
            Assert.Null(result);  // Verifica se o retorno é null

        }
        
        [Fact]
        public async Task ConcluirTarefas_DeveAtualizarTarefasTrue()
        {

            int[] ids = { 1, 2, 3, 4 };

            // Configura o mock para retornar o dado esperado
            _tarefaServiceMock.Setup(service => service.ConcluirTarefas(ids, false)).ReturnsAsync(true);

            // Act            
            var result = await _tarefaServiceMock.Object.ConcluirTarefas(ids, false);

            // Assert
            Assert.True(result);

            // Verifica se o método Excluir foi chamado
            _tarefaServiceMock.Verify(service => service.ConcluirTarefas(ids, false), Times.Once);
        }
        [Fact]
        public async Task ConcluirTarefas_DeveAtualizarTarefasFalse()
        {

            int[] ids = { 1, 2, 3, 4 };

            // Configura o mock para retornar o dado esperado
            _tarefaServiceMock.Setup(service => service.ConcluirTarefas(ids, false)).ReturnsAsync(false);

            // Act            
            var result = await _tarefaServiceMock.Object.ConcluirTarefas(ids, false);

            // Assert
            Assert.False(result);

            // Verifica se o método Excluir foi chamado
            _tarefaServiceMock.Verify(service => service.ConcluirTarefas(ids, false), Times.Once);
        }
        [Fact]
        public async Task Obter_TarefasPeloId_Deve_Retornar_dado_Valido()
        {
            var respostaEsperada = new TarefaResponse()
            {
                Id = 1,
                Concluido = false,
                Descricao = "descrição",
                Titulo = "Titulo",
                UsuarioId = 111
            };
            // Configura o mock para retornar o dado esperado
            _tarefaServiceMock.Setup(service => service.ObterPorId(1)).ReturnsAsync(respostaEsperada);

            // Act            
            var retorno = await _tarefaServiceMock.Object.ObterPorId(1);

            // Assert (Validação)
            Assert.NotNull(retorno);
            Assert.Equal(respostaEsperada.Id, retorno.Id);
            Assert.Equal(respostaEsperada.Concluido, retorno.Concluido);
            Assert.Equal(respostaEsperada.Descricao, retorno.Descricao);
            Assert.Equal(respostaEsperada.Titulo, retorno.Titulo);
            Assert.Equal(respostaEsperada.UsuarioId, retorno.UsuarioId);
            // Verifica se o método Criar foi chamado
            _tarefaServiceMock.Verify(service => service.ObterPorId(1), Times.Once);
        }
        [Fact]
        public async Task Obter_Lista_Tarefas_PeloId_Usuario()
        {
            List<Tarefa> tarefas = new List<Tarefa>
                        {
                        new Tarefa("Tarefa 1", "Descrição 1", false, 111),
                        new Tarefa("Tarefa 2", "Descrição 2", false, 111),
                        new Tarefa("Tarefa 3", "Descrição 3", false, 111),
                        new Tarefa("Tarefa 4", "Descrição 4", false, 111),
                        new Tarefa("Tarefa 5", "Descrição 5", false, 111)
                    };

            List<TarefaResponse> listaResponse = tarefas.Select(t => new TarefaResponse
            {
                Id = t.Id,
                Concluido = t.Concluido,
                Descricao = t.Descricao,
                Titulo = t.Titulo,
                UsuarioId = t.UsuarioId
            }).ToList();

            var resopnsePaginado = new Paginacao<TarefaResponse>(listaResponse, 5, 1, 5);

            // Configura o mock para retornar o dado esperado
            _tarefaServiceMock.Setup(service => service.ObterTodas(1,10, 111)).ReturnsAsync(resopnsePaginado);

            var retorno = await _tarefaServiceMock.Object.ObterTodas(1, 10, 111);

            Assert.NotNull(retorno);
            Assert.Equal(1, retorno.PaginaAtual);
            Assert.Equal(5, retorno.QuantidadePorPagina);
            Assert.Equal(5, retorno.TotalDeRegistros);
            Assert.Equal(1, retorno.TotalDePaginas);// O total de tarefas deve ser 5
            Assert.Equal(5, retorno.Items.Count); // Como estamos na primeira página e o tamanho da página é 5, esperamos 5 itens

            // Verificar se as tarefas retornadas são as esperadas (Tarefa 1 e Tarefa 2)
            Assert.Contains(retorno.Items, t => t.Titulo == "Tarefa 1");
            Assert.Contains(retorno.Items, t => t.Titulo == "Tarefa 2");


        }

    }
}
