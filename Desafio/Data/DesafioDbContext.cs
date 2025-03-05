using Desafio.Domain.Tarefas;
using Desafio.Domain.Usuarios;
using Microsoft.EntityFrameworkCore;

namespace Desafio.Data
{
    public class DesafioDbContext : DbContext
    {
        public DesafioDbContext(DbContextOptions<DesafioDbContext> options) : base(options){ }

        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Tarefa> Tarefas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>()
                .HasKey(u => u.Id);
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Nome)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Login)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<Usuario>()
                .Property(u => u.SenhaSalt)
                .IsRequired();
            modelBuilder.Entity<Usuario>()
                .Property(u => u.SenhaHash)
                .IsRequired();

            modelBuilder.Entity<Tarefa>()
                .HasKey(t => t.Id);
            modelBuilder.Entity<Tarefa>()
                .Property(t => t.Titulo)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<Tarefa>()
                .Property(t => t.Descricao)
                .IsRequired()
                .HasMaxLength(500);
            modelBuilder.Entity<Tarefa>()
                .Property(t => t.Concluido)
                .IsRequired();
            modelBuilder.Entity<Tarefa>()
                .HasOne<Usuario>() // Indica que a Tarefa tem relação com Usuario
                .WithMany() // Um usuário pode ter várias tarefas
                .HasForeignKey(t => t.UsuarioId) // Define UsuarioId como chave estrangeira
                .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
