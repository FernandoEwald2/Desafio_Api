using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Desafio.Domain.Usuarios
{
    [Table("usuario")]
    public class Usuario
    {
        [Key, Column("id")]
        public int Id { get; private set; }
        [Column("nome")]
        public string Nome { get; private set; }
        [Column("login")]
        public string Login { get; private set; }
        [Column("senha_hash")]
        public byte[] SenhaHash { get; private set; }
        [Column("senha_salt")]
        public byte[] SenhaSalt { get; private set; }

        public Usuario() { }
        // Construtor
        public Usuario( string nome, string login, byte[] senhaHash, byte[] senhaSalt)
        {
            
            SetNome(nome);
            SetLogin(login);
            SetHash(senhaHash);
            SetSalt(senhaSalt);
        }

        public void SetSalt(byte[] senhaSalt)
        {
            SenhaSalt = senhaSalt;
        }

        public void SetHash(byte[] senhaHash)
        {
            SenhaHash = senhaHash;
        }

        public void SetLogin(string login)
        {
            Login = login;
        }

        public void SetNome(string nome)
        {
            Nome = nome;
        }        

        public void AtualizarUsuario(Usuario usuario) 
        {
            SetNome(usuario.Nome);
            SetLogin(usuario.Login);
            SetHash(usuario.SenhaHash);
            SetSalt(usuario.SenhaSalt);

        }
       
    }
}
