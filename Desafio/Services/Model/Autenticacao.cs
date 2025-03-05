using System;
using System.Text.Json.Serialization;

namespace Desafio.Services.Model
{
    public class Autenticacao
    {
        [JsonPropertyName("usuario_id")]
        public int UsuarioId { get; set; }
        [JsonPropertyName("usuario_nome")]
        public string UsuarioName { get; set; }
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("expira_em")]
        public DateTime ExpiraEm { get; set; }
        public string Rule { get; set; } = "Bearer";
        [JsonPropertyName("role")]
        public string Role { get; set; }
    }
}
