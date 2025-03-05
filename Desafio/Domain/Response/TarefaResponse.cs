namespace Desafio.Api.Domain.Response
{
    public class TarefaResponse
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public bool Concluido { get; set; }
        public int UsuarioId { get; set; }
    }
}
