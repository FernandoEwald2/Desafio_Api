using Desafio.Domain.Usuarios;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Desafio.Domain.Tarefas
{
    [Table("tarefa")]
    public class Tarefa
    {
        [Key, Column("id")]
        public int Id { get; private set; }
        [Column("titulo")]
        public string Titulo { get; private set; }
        [Column("descricao")]
        public string Descricao { get; private set; }
        [Column("concluido")]
        public bool Concluido { get; private set; }
        [ForeignKey("Usuario"), Column("usuario_id")]
        public int UsuarioId { get; private set; }
        public virtual Usuario Usuario { get; }

        public Tarefa() { }
        // Construtor
        public Tarefa(string titulo, string descricao, bool concluido, int usuario_id)
        {
            SetTitulo(titulo);
            SetDescricao(descricao);
            SetConcluido(concluido);
            SetUsuarioId(usuario_id);
        }

        private void SetUsuarioId(int usuario_id)
        {
            UsuarioId = usuario_id;
        }

        private void SetConcluido(bool concluido)
        {
            Concluido = concluido;
        }

        private void SetDescricao(string descricao)
        {
            Descricao = descricao;
        }

        private void SetTitulo(string titulo)
        {
            Titulo = titulo;
        }

        // Método para marcar como concluída
        public void MarcarComoConcluida(bool status)
        {
            SetConcluido(status);
        }

        // Método para atualizar a tarefa
        public void AtualizarTarefa(string titulo, string descricao, bool concluido)
        {
            SetTitulo(titulo);
            SetDescricao(descricao);
            SetConcluido(concluido);
        }
    }
}
