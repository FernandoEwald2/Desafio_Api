using System;
using System.Text.Json.Serialization;

namespace Desafio.Services.Model
{
    public class Auth
    {
        [JsonPropertyName("login")]
        public string Login { get; set; }

        [JsonPropertyName("senha")]
        public string Senha { get; set; }

        public bool Validar(){

            if (String.IsNullOrEmpty(this.Login))
                throw new ArgumentNullException("Informe o login");

            if (String.IsNullOrEmpty(this.Senha))
                throw new ArgumentNullException("Informe o login");

            return true;
        }

    }
}
