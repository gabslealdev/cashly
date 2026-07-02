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

Cashly é uma aplicação de controle financeiro pessoal e colaborativo. O objetivo do projeto é permitir que usuários registrem receitas e despesas, acompanhem saldos mensais e visualizem a evolução financeira de um cashflow ao longo do tempo.

O projeto está sendo desenvolvido como um monorepo fullstack, com backend em ASP.NET Core, frontend em Angular e arquitetura inspirada em Clean Architecture para melhor testabilidade.

A modelagem do domínio segue os princípios do Domain-Driven Design (DDD), aplicando conceitos como Entities, Value Objects e Aggregates, bem como encapsulamento das regras de negócio e separação de responsabilidades. Isso mantém o domínio independente de detalhes de infraestrutura, visando ortogonalidade, coesão e manutenibilidade.

## Visão

Indivíduos e famílias frequentemente acompanham e controlam gastos por planilhas ou grupos em WhatsApp. Isso dificulta colaboração, histórico financeiro e acompanhamento recorrente.

Cashly busca resolver esse problema oferecendo:

- Cadastro e autenticação de usuários.
- Criação de cashflows pessoais ou compartilhados.
- Associação de membros a um cashflow com papéis de acesso.
- Registro de receitas e despesas.
- Visualização mensal de transações e saldos.
- Base para fechamento mensal e preservação de histórico financeiro.

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

Contém entidades, value objects, erros e regras centrais do domínio. Exemplos: `User`, `Cashflow`, `CashflowMember`, `Email`, `Name`, `Title`.

**Application**

Contém casos de uso, comandos, queries, handlers, contratos de persistência, contratos de segurança e modelos de resposta.

**Infrastructure**

Contém implementações externas, como EF Core, DbContext, repositórios, Unit of Work, hashing de senha, JWT e mediator interno.

**Api**

Expõe os casos de uso via controllers HTTP, autenticação, autorização, CORS e Swagger.

## Domínio e Agregados

### Identity Context

Responsável por identidade e autenticação:

- Registro de usuário.
- Login de usuário.
- Email normalizado.
- Senha armazenada como hash.
- Geração de token JWT.

### Cashflow Context

Responsável pelo controle financeiro:

- Criação de cashflows.
- Listagem de cashflows vinculados ao usuário autenticado.
- Registro de transações.
- Organização mensal.
- Cálculo de saldo por período.
- Base de domínio para fechamento mensal.

### Collaboration Context

Responsável pela colaboração dentro de um cashflow:

- Membros de cashflow.
- Papel do usuário: `Owner`, `Contributor`, `Viewer`.
- Garantia de owner único por cashflow.

## Funcionalidades

### Implementado

- Cadastro de usuário.
- Login com JWT.
- Hash de senha com Argon2.
- Criação de cashflow.
- Criador definido automaticamente como `Owner`.
- Listagem de cashflows do usuário autenticado.
- Dashboard inicial com cards de cashflows.
- Seleção de cashflow e abertura do board mensal.
- Registro de transações em cashflows.
- Visualização mensal com 2 meses anteriores, mês atual e 1 mês futuro.
- Agrupamento de transações por mês.
- Cálculo de saldo, projeção e status financeiro por período no board.
- Interceptor HTTP para envio de token JWT.
- Auth guard no Angular.
- Testes unitários de domínio.
- Testes unitários de application para identidade.

### Em Desenvolvimento

- Fechamento mensal no domínio.
- Integração do fluxo de fechamento mensal na API e no frontend.
- Refinamento do board mensal e do formulário de transações.
- Ampliação da cobertura de testes para cashflow board e transações.

### Planejado

- Histórico imutável de meses fechados exposto na aplicação.
- Colaboração avançada entre membros.

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

### Pré-requisitos

- .NET SDK 9
- Node.js compatível com Angular 20
- Angular CLI
- Docker

### Banco De Dados

Na pasta do backend, copie o arquivo de exemplo para criar a configuração local:

```bash
cd src/backend
cp .env.example .env
```

O Docker Compose usa o `.env` dessa pasta para configurar o SQL Server. A variável obrigatória para o container é:

```env
MSSQL_SA_PASSWORD=Your_strong_password123
```

Suba o SQL Server ainda na pasta `src/backend`:

```bash
docker compose -f compose.YAML up -d
```

### Configuração Da API

A API precisa de connection string e configurações de JWT. Essas configurações podem ser definidas via `appsettings.Development.json`, user secrets ou variáveis de ambiente.

Variáveis esperadas quando usar ambiente:

```env
ConnectionStrings__DefaultConnection=Server=localhost,1433;Database=Cashly;User Id=sa;Password=Your_strong_password123;TrustServerCertificate=True
Jwt__SecretKey=replace-with-a-long-local-development-secret
Jwt__Issuer=Cashly.Api
Jwt__Audience=Cashly.Web
Jwt__ExpirationInMinutes=180
```

O arquivo `.env` é usado pelo Docker Compose. A API ASP.NET Core não carrega esse arquivo automaticamente; para usar as mesmas chaves na API, exporte as variáveis no shell, use user secrets ou configure o `appsettings.Development.json` localmente.

### Backend

```bash
cd src/backend
dotnet restore Cashly.sln
dotnet run --project src/Cashly.Api/Cashly.Api.csproj
```

Por padrão, a API usa:

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

Aplicação Angular:

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

## Documentação

A documentação do projeto está em `docs/`:

- [Vision](docs/01-Vision.md)
- [Glossary](docs/02-Glossary.md)
- [Requirements](docs/03-Requirements.md)
- [Domain Model](docs/04-Model-Domain.md)
- [Business Rules](docs/05-Business-Rules.md)
- [Backlog](docs/06-Backlog.md)
- [Traceability](docs/07-Traceability.md)
- [Test Coverage Gaps](docs/TEST-COVERAGE-GAPS.md)

## Status Do Projeto

Cashly está em desenvolvimento ativo. O fluxo principal de autenticação, criação de cashflows, seleção de cashflow, visualização do board mensal e registro de transações já está implementado em backend e frontend.

O foco atual é consolidar o fechamento mensal, expor o histórico de meses fechados na aplicação, refinar a experiência do board e ampliar a cobertura de testes dos contextos de cashflow e transações.
