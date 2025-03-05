using Desafio.Api.Domain.Request;
using Desafio.Api.Domain.Response;
using Desafio.Api.Util;
using Desafio.Data;
using Desafio.Domain.Usuarios;
using Desafio.Services.Interfaces;
using Desafio.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Desafio.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly DesafioDbContext _context;        

        public UsuarioService(DesafioDbContext context)
        {
            _context = context;            
        }
        public async Task<UsuarioResponse> Atualizar(int id, UsuarioRequest usuarioRequest)
        {
            Usuario usuarioExistente = await _context.Usuarios.FindAsync(id);
            if (usuarioExistente == null) return null;

            if (!string.IsNullOrEmpty(usuarioRequest.Senha))
            {

                if (!Criptografia.VerificarHashSalt(usuarioRequest.Senha, usuarioExistente.SenhaHash, usuarioExistente.SenhaSalt))
                {
                    byte[] hash;
                    byte[] salt;
                    Criptografia.CriarHashSalt(usuarioRequest.Senha, out hash, out salt);
                    usuarioExistente.SetHash(hash);
                    usuarioExistente.SetSalt(salt);
                    usuarioExistente.SetLogin(usuarioRequest.Login);
                    usuarioExistente.SetNome(usuarioRequest.Nome.ToUpper());

                    await _context.SaveChangesAsync();

                    return new UsuarioResponse()
                    {
                        Id = id,
                        Login = usuarioRequest.Login,
                        Nome = usuarioRequest.Nome,
                    };
                }
                else 
                {
                    usuarioExistente.SetLogin(usuarioRequest.Login);
                    usuarioExistente.SetNome(usuarioRequest.Nome.ToUpper());

                    await _context.SaveChangesAsync();

                    return new UsuarioResponse()
                    {
                        Id = id,
                        Login = usuarioRequest.Login,
                        Nome = usuarioRequest.Nome,
                    };
                }
            }
            else
            {
                usuarioExistente.SetLogin(usuarioRequest.Login);
                usuarioExistente.SetNome(usuarioRequest.Nome.ToUpper());

                await _context.SaveChangesAsync();

                return new UsuarioResponse()
                {
                    Id = id,
                    Login = usuarioRequest.Login,
                    Nome = usuarioRequest.Nome,
                };
            }

        }
        
        public async Task<UsuarioResponse> Criar(UsuarioRequest usuarioRequest)
        {
            if (ValidarLoginUsuarioExiste(usuarioRequest.Login))
                throw new Exception("Esse login não esta disponível");
            
            byte[] hash;
            byte[] salt;
            Criptografia.CriarHashSalt(usuarioRequest.Senha, out hash, out salt);            

            Usuario usuario = new Usuario(usuarioRequest.Nome.ToUpper(), usuarioRequest.Login, hash, salt);
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return new UsuarioResponse()
            {
                Id = usuario.Id,
                Login = usuario.Login,
                Nome = usuario.Nome,
            }; ;
        }

        public async Task<bool> Excluir(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return false;

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UsuarioResponse> ObterPorId(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            
            if (usuario == null)
                return null;

            return new UsuarioResponse { Id = usuario.Id,Login = usuario.Login, Nome = usuario.Nome };
        }

        public async Task<Paginacao<UsuarioResponse>> ObterTodos(int pageNumber, int pageSize)
        {
            var totalRecords = await _context.Usuarios.CountAsync();

            var listaUsuarios = await _context.Usuarios
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            var resonse = listaUsuarios.Select(u => new UsuarioResponse
            {
                Id = u.Id,
                Nome = u.Nome,
                Login = u.Login
            }).ToList();

            return new Paginacao<UsuarioResponse>(resonse, totalRecords, pageNumber, pageSize);
        }
        private bool ValidarLoginUsuarioExiste(string login) 
        {
            var usuario = _context.Usuarios.Where(u => u.Login.ToUpper() == login.ToUpper()).FirstOrDefault();
            if(usuario == null)
                return false;
            else
                return true;

        }       

    }
}
