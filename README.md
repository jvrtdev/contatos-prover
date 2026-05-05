# ContatosService

API REST para cadastro e manutencao de contatos, desenvolvida para prova tecnica .NET com ASP.NET Core Web API, Entity Framework Core, SQL Server e testes unitarios.

## Tecnologias

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server 2022 em Docker
- xUnit, FluentAssertions e Moq
- Swagger com Swashbuckle

## Arquitetura

O projeto usa camadas simples inspiradas em DDD e Clean Architecture:

- `ContatosService.Api`: controllers, viewmodels, mappers HTTP, Swagger e tratamento global de erros.
- `ContatosService.Application`: DTOs, services de aplicacao, interfaces e excecoes de aplicacao.
- `ContatosService.Domain`: entidade rica `Contato`, enums, regras de negocio e contratos de repositorio.
- `ContatosService.Infrastructure`: EF Core, SQL Server, `AppDbContext`, Fluent API, repositories e migrations.
- `ContatosService.UnitTests`: testes unitarios de Domain e Application.

Fluxo principal:

```text
HTTP Request -> ViewModel -> DTO -> Application Service -> Domain Entity -> Repository -> EF Core -> SQL Server
```

## Regras De Negocio

- A idade e calculada em tempo de execucao e nao e persistida no banco.
- O contato deve ser maior de idade.
- A idade nao pode ser igual a 0.
- A data de nascimento nao pode ser maior que a data atual.
- Listagem, visualizacao e edicao consideram apenas contatos ativos.
- O contato pode ser desativado, ativado novamente e excluido.
- A entidade `Contato` protege as regras de atualizacao e transicao de status.

Valores aceitos para `Sexo`:

- `Masculino`
- `Feminino`
- `Outro`

## Banco De Dados

A connection string padrao usa usuario e senha para funcionar em Linux e Windows:

```text
Server=localhost,1433;Database=ContatosDb;User Id=sa;Password=Prover@123456;TrustServerCertificate=True
```

Suba o SQL Server:

```bash
docker compose up -d
```

Restaure a ferramenta local do EF:

```bash
dotnet tool restore
```

A migration inicial ja esta versionada. Para aplicar no banco:

```bash
dotnet ef database update --project src/ContatosService.Infrastructure/ContatosService.Infrastructure.csproj --startup-project src/ContatosService.Api/ContatosService.Api.csproj
```

Para recriar a migration do zero, remova a migration atual e execute:

```bash
dotnet ef migrations add InitialCreate --project src/ContatosService.Infrastructure/ContatosService.Infrastructure.csproj --startup-project src/ContatosService.Api/ContatosService.Api.csproj --output-dir Persistence/Migrations
```

## Como Rodar

Na raiz do repositorio:

```bash
dotnet restore
dotnet build
dotnet run --project src/ContatosService.Api/ContatosService.Api.csproj --launch-profile http
```

Swagger:

```text
http://localhost:5233/swagger
```

## Testes

```bash
dotnet test
```

## Endpoints

Base URL local:

```text
http://localhost:5233
```

| Metodo | Rota | Descricao |
| --- | --- | --- |
| `POST` | `/api/contatos` | Cria um contato |
| `GET` | `/api/contatos` | Lista contatos ativos |
| `GET` | `/api/contatos/{id}` | Visualiza contato ativo por id |
| `PUT` | `/api/contatos/{id}` | Atualiza contato ativo |
| `PATCH` | `/api/contatos/{id}/ativar` | Ativa um contato inativo |
| `PATCH` | `/api/contatos/{id}/desativar` | Desativa um contato ativo |
| `DELETE` | `/api/contatos/{id}` | Exclui um contato |

Exemplo de payload para criacao/atualizacao:

```json
{
  "nome": "Maria Silva",
  "dataNascimento": "1990-03-10",
  "sexo": "Feminino"
}
```

Exemplo de resposta:

```json
{
  "id": "00000000-0000-0000-0000-000000000000",
  "nome": "Maria Silva",
  "dataNascimento": "1990-03-10",
  "sexo": "Feminino",
  "idade": 36,
  "status": "Ativo"
}
```

## Tratamento De Erros

Erros seguem o formato:

```json
{
  "statusCode": 400,
  "message": "Mensagem do erro"
}
```

Mapeamento:

- `DomainException`: `400 Bad Request`
- `NotFoundException`: `404 Not Found`
- `ConflictException`: `409 Conflict`
- Excecoes nao tratadas: `500 Internal Server Error`
