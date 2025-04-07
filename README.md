# 🧩 Desafio API
 
 Projeto de uma **API RESTful** desenvolvida em **C#** com foco em boas práticas de arquitetura, segurança e organização por camadas. A aplicação consiste em um **sistema de cadastro de tarefas**, com autenticação via JWT, rotas protegidas e paginação nos resultados.
 
 ## 🔐 Autenticação
 
 A API implementa autenticação com **JWT Bearer Token**, garantindo que apenas usuários autenticados possam acessar as rotas protegidas.
 
 ## ✏️ Funcionalidades
 
 - Login com geração de Token JWT
 - CRUD completo de tarefas:
   - Criar tarefa
   - Editar tarefa
   - Excluir tarefa
   - Obter tarefa por ID
   - Listar todas as tarefas (com paginação)
 
 ## 🏗️ Arquitetura em Camadas
 
 O projeto é separado por camadas para garantir **organização**, **manutenção** e **escalabilidade**:
 
 - **Controllers**  
   Definem e configuram as rotas das APIs. Chamam os serviços por meio de interfaces.
 
 - **Services**  
   Contém as regras de negócio e interfaces públicas para isolar a camada de dados.
 
 - **Data**  
   Responsável pela comunicação com o banco de dados. Contém o `DbContext` e as configurações do EF Core.
 
 - **Domain**  
   Inclui:
   - Classes relacionadas às tabelas (Entidades)
   - Objetos de requisição (Request) e resposta (Response)
   - Regras de domínio
 
 - **Utils**  
   Contém utilitários para:
   - Criptografia
   - Gerador e validação de JWT
   - Paginação
 
 ## 🛢️ Banco de Dados
 
 - Utiliza **SQL Server**
 - A conexão é feita via Entity Framework Core
 - As **credenciais de acesso** (usuário, senha) e a **chave secreta JWT** são armazenadas em **variáveis de ambiente**, evitando exposição no `appsettings.json`
 
 ## 🧪 Tecnologias e Bibliotecas
 
 - **C# / .NET Core 3.1**
 - **Entity Framework Core 3.1**
   - `Microsoft.EntityFrameworkCore`
   - `Microsoft.EntityFrameworkCore.SqlServer`
   - `Microsoft.EntityFrameworkCore.Design`
 - **Autenticação JWT**
   - `Microsoft.AspNetCore.Authentication.JwtBearer`
 - **Swagger para documentação da API**
   - `Swashbuckle.AspNetCore`
 - **System.Linq.Dynamic.Core**
   - Para construção dinâmica de consultas LINQ
 
 ## 🚀 Como Rodar o Projeto
 
 > Pré-requisitos: .NET Core SDK 3.1, SQL Server
 
 1. Clone o repositório:
    ```bash
    git clone https://github.com/FernandoEwald2/Desafio_Api.git
 
 2. Configure as variáveis de ambiente com os mesmos nomes que estão na sctring de conexão:
  DB_USER_DESAFIO, DB_PASSWORD_DESAFIO e WT_SECRET com os valores correspondentes.
 
 3. Crie as tabelas usuario e tarefa com as mesmas propriedades das classes ou execute o comenaod Migrations
   - dotnet ef database update
 
 **Pronto basta rodar o comendo dotnet run e o projeto Api deve carregar em:** https://localhost:5001
 
 🛡️ Segurança
 Autenticação via JWT
 Variáveis de ambiente para proteger dados sensíveis
 Boa separação de responsabilidades entre as camadas
 
 📌 Autor
 
 [Fernando Ewald](https://www.linkedin.com/in/fernando-ewald)
