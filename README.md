
# Tech Challenge 4 - Sistema de Reservas
API para um sistema de reservas onde é possível criar usuário, e após logado fazer o cadastro de reservas.

# Tecnologias implementadas
- .NET8
- ASP.NET WebApi
- Entity Framework Core
- JWT Bearer Authentication
- SQL Server
- LOGS com Microsoft.Extensions.Logging 
- Injeção de dependência
- Docker

# Integrante
- Marcondes Amaral de Oliveira - RM352481

# Execução da solution
- Clean Solution
- Build Solution
- No Package Manage Console digite os comandos:
    - Add-Migration InitialCreate
    - Update-Database


Obs: Utilizei o Docker desktop com uma imagem do banco de dados SQL Server.

Segue o passo a passo para Execução completa do projeto:

- docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=<S3Nh4F0rT3>" -p 1433:1433 --name sqlserve-tech4 -h sqlserve-tech4 -d mcr.microsoft.com/mssql/server:2019-latest

- conectar o Gerenciador de banco de dados como:
server name: 127.0.0.1
Authentication: SQL Server Authentication
Login: sa
Password: <S3Nh4F0rT3>
(Ou pode conectar pelo próprio visual studio para acessar e consultar o banco e visualizar os dados no banco)

- Add-Migration firstMigration -Project src\SistemaDeReservas.Infrastructure -StartupProject src\SistemaDeReservas.Infrastructure

- Update-Database -Project src\SistemaDeReservas.Infrastructure -StartupProject src\SistemaDeReservas.Infrastructure -Connection "Server=localhost,1433\\sqlserve-tech4;Database=Tech4;User Id=sa;Password=<S3Nh4F0rT3>;TrustServerCertificate=True"

- no appsettings.json setar
"EmailFromAddress": "email@gmail.com",
"SmtpPassword": "xxx xxx xxxx xxxx"

Nesse caso configurei como sender o gmail e o mesmo pede "senha de app" para poder ser utilizado por aplicativo externo. Outro provedor pode ser que não seja necessário e funcione simplesmente com o email e senha normalmente.  nesse caso a senha de app é gerada especificamente para o aplicativo.

Seguindo esses passos o sistema estará pronto para execução.

Link para o video no Youtube: https://youtu.be/7qQhIfdQSKg

