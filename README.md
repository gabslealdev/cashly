<p align="center">
  <img src="docs/assets/cashly-logo.svg" alt="Cashly" width="180" />
</p>

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=flat&logo=dotnet&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-9.0-512BD4?style=flat&logo=dotnet&logoColor=white)
![Angular](https://img.shields.io/badge/Angular-20-DD0031?style=flat&logo=angular&logoColor=white)
![TypeScript](https://img.shields.io/badge/TypeScript-5.8-3178C6?style=flat&logo=typescript&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927?style=flat&logo=microsoftsqlserver&logoColor=white)
![Entity Framework Core](https://img.shields.io/badge/Entity%20Framework%20Core-9.0-512BD4?style=flat&logo=dotnet&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-Compose-2496ED?style=flat&logo=docker&logoColor=white)
![xUnit](https://img.shields.io/badge/xUnit-Tests-5E2B97?style=flat)

Cashly e uma aplicacao de controle financeiro pessoal e colaborativo. O objetivo do projeto e permitir que usuarios registrem receitas e despesas, acompanhem saldos mensais e visualizem a evolucao financeira de um cashflow ao longo do tempo.

O projeto esta sendo construido como uma aplicacao full stack, com backend em ASP.NET Core, frontend em Angular e modelagem inspirada em Clean Architecture e DDD tactico.

## Visao Do Produto

Individuos, familias e pequenos grupos frequentemente controlam gastos em planilhas ou conversas dispersas. Isso dificulta colaboracao, historico financeiro e acompanhamento recorrente.

Cashly busca resolver esse problema oferecendo:

- Cadastro e autenticacao de usuarios.
- Criacao de cashflows pessoais ou compartilhados.
- Associacao de membros a um cashflow com papeis de acesso.
- Registro de receitas e despesas.
- Visualizacao mensal de transacoes e saldos.
- Base para fechamento mensal e preservacao de historico financeiro.

## Stack

**Backend**

- .NET 9
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Bearer Authentication
- Argon2 para hash de senha
- Swagger/OpenAPI

**Frontend**

- Angular 20
- TypeScript
- Reactive Forms
- Angular Router
- Http Interceptors
- Signals
- SCSS

**Testes**

- xUnit
- Shouldly
- Moq
- Bogus
- coverlet

**Infraestrutura Local**

- Docker Compose
- SQL Server 2022

## Arquitetura

O backend segue uma arquitetura em camadas inspirada em Clean Architecture:

```text
src/backend/src
├── Cashly.Domain
├── Cashly.Application
├── Cashly.Infrastructure
└── Cashly.Api
```

**Domain**

Contem entidades, value objects, erros e regras centrais do dominio. Exemplos: `User`, `Cashflow`, `CashflowMember`, `Email`, `Name`, `Title`.

**Application**

Contem casos de uso, comandos, queries, handlers, contratos de persistencia, contratos de seguranca e modelos de resposta.

**Infrastructure**

Contem implementacoes externas, como EF Core, DbContext, repositorios, Unit of Work, hashing de senha, JWT e mediator interno.

**Api**

Expoe os casos de uso via controllers HTTP, autenticacao, autorizacao, CORS e Swagger.

## Modulos Do Dominio

### Identity Context

Responsavel por identidade e autenticacao:

- Registro de usuario.
- Login de usuario.
- Email normalizado.
- Senha armazenada como hash.
- Geracao de token JWT.

### Cashflow Context

Responsavel pelo controle financeiro:

- Criacao de cashflows.
- Listagem de cashflows vinculados ao usuario autenticado.
- Registro de transacoes.
- Organizacao mensal.
- Calculo de saldo por periodo.
- Base de dominio para fechamento mensal.

### Collaboration Context

Responsavel pela colaboracao dentro de um cashflow:

- Membros de cashflow.
- Papel do usuario: `Owner`, `Contributor`, `Viewer`.
- Garantia de owner unico por cashflow.

## Funcionalidades

### Implementado

- Cadastro de usuario.
- Login com JWT.
- Hash de senha com Argon2.
- Criacao de cashflow.
- Criador definido automaticamente como `Owner`.
- Listagem de cashflows do usuario autenticado.
- Dashboard inicial com cards de cashflows.
- Selecao de cashflow e abertura do board mensal.
- Registro de transacoes em cashflows.
- Visualizacao mensal com 2 meses anteriores, mes atual e 1 mes futuro.
- Agrupamento de transacoes por mes.
- Calculo de saldo, projecao e status financeiro por periodo no board.
- Interceptor HTTP para envio de token JWT.
- Auth guard no Angular.
- Testes unitarios de dominio.
- Testes unitarios de application para identidade.

### Em Desenvolvimento

- Fechamento mensal no dominio.
- Integracao do fluxo de fechamento mensal na API e no frontend.
- Refinamento do board mensal e do formulario de transacoes.
- Ampliacao da cobertura de testes para cashflow board e transacoes.

### Planejado

- Historico imutavel de meses fechados exposto na aplicacao.
- Colaboracao avancada entre membros.

## Estrutura Do Projeto

```text
.
├── docs
│   ├── 01-Vision.md
│   ├── 02-Glossary.md
│   ├── 03-Requirements.md
│   ├── 04-Model-Domain.md
│   ├── 05-Business-Rules.md
│   ├── 06-Backlog.md
│   ├── 07-Traceability.md
│   └── TEST-COVERAGE-GAPS.md
└── src
    ├── backend
    │   ├── Cashly.sln
    │   ├── compose.YAML
    │   ├── src
    │   │   ├── Cashly.Api
    │   │   ├── Cashly.Application
    │   │   ├── Cashly.Domain
    │   │   └── Cashly.Infrastructure
    │   └── tests
    │       ├── Cashly.Application.UnitTests
    │       └── Cashly.Domain.UnitTests
    └── frontend
        └── cashly-web
```

## Como Rodar Localmente

### Pre-requisitos

- .NET SDK 9
- Node.js compativel com Angular 20
- Angular CLI
- Docker

### Banco De Dados

Na pasta do backend, configure a senha do SQL Server em um arquivo `.env`:

```env
MSSQL_SA_PASSWORD=Your_strong_password123
```

Suba o SQL Server:

```bash
cd src/backend
docker compose -f compose.YAML up -d
```

Configure a connection string e JWT via `appsettings.Development.json`, user secrets ou variaveis de ambiente.

### Backend

```bash
cd src/backend
dotnet restore Cashly.sln
dotnet run --project src/Cashly.Api/Cashly.Api.csproj
```

Por padrao, a API usa:

```text
http://localhost:5066
```

Swagger em ambiente de desenvolvimento:

```text
http://localhost:5066/swagger
```

### Frontend

```bash
cd src/frontend/cashly-web
npm install
npm start
```

Aplicacao Angular:

```text
http://localhost:4200
```

## Testes

Backend:

```bash
dotnet test src/backend/Cashly.sln
```

Frontend:

```bash
cd src/frontend/cashly-web
npm test
```

## Documentacao

A documentacao do projeto esta em `docs/`:

- [Vision](docs/01-Vision.md)
- [Glossary](docs/02-Glossary.md)
- [Requirements](docs/03-Requirements.md)
- [Domain Model](docs/04-Model-Domain.md)
- [Business Rules](docs/05-Business-Rules.md)
- [Backlog](docs/06-Backlog.md)
- [Traceability](docs/07-Traceability.md)
- [Test Coverage Gaps](docs/TEST-COVERAGE-GAPS.md)

## Status Do Projeto

Cashly esta em desenvolvimento ativo. O fluxo principal de autenticacao, criacao de cashflows, selecao de cashflow, visualizacao do board mensal e registro de transacoes ja esta implementado em backend e frontend.

O foco atual e consolidar o fechamento mensal, expor o historico de meses fechados na aplicacao, refinar a experiencia do board e ampliar a cobertura de testes dos contextos de cashflow e transacoes.
