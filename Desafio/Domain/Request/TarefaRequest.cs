namespace Desafio.Api.Domain.Request
{
    public class TarefaRequest
    {        
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public bool Concluido { get; set; }
        public int UsuarioId { get; set; }
    }
}
