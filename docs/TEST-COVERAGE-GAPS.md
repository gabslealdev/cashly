# Analise de cobertura de testes

Data da analise: 2026-05-22

## Resumo

A cobertura automatizada atual esta concentrada no `IdentityContext` do backend. Existem testes unitarios para alguns value objects e entidade `User`, alem dos handlers de registro e login.

Nao foram encontrados testes automatizados para:

- `CashflowContext`
- `CollaborationContext`
- controllers da API
- repositorios EF Core
- configuracoes/mapeamentos do banco
- mediator
- interceptors, guards, services e componentes Angular

Tambem nao foram encontrados arquivos `.spec.ts` ou `.test.ts` no frontend.

## Testes existentes

### Backend - Domain

Projeto: `src/backend/tests/Cashly.Domain.UnitTests`

Cobertura encontrada:

- `IdentityContext/ValueObjects/EmailUnitTest.cs`
- `IdentityContext/ValueObjects/NameUnitTest.cs`
- `IdentityContext/ValueObjects/PasswordHashUnitTest.cs`
- `IdentityContext/Entities/UserUnitTest.cs`

Esses testes cobrem criacao valida e algumas validacoes de erro para email, nome e password hash, alem de criacao/alteracao basica de `User`.

### Backend - Application

Projeto: `src/backend/tests/Cashly.Application.UnitTests`

Cobertura encontrada:

- `IdentityContext/UseCases/RegisterUser/RegisterUserHandlerTests.cs`
- `IdentityContext/UseCases/LoginUser/LoginUserHandlerTests.cs`

Esses testes cobrem os fluxos principais de registro e login, incluindo email ja existente e usuario inexistente no login.

### Frontend

Projeto: `src/frontend/cashly-web`

Nao ha specs Angular detectadas para services, forms, components, guards ou interceptors.

## Lacunas por prioridade

### Prioridade alta

#### Cashflow domain

Faltam testes para `Cashflow`.

Cenarios recomendados:

- Deve criar cashflow com titulo valido e owner valido.
- Deve adicionar exatamente um owner ao criar cashflow.
- Deve rejeitar `userId` vazio como owner.
- Deve preencher `CreatedAt` e `UpdatedAt`.
- Deve atualizar `UpdatedAt` ao atribuir owner.
- Deve garantir que nao exista mais de um owner no mesmo cashflow.

#### Collaboration domain

Faltam testes para `CashflowMember`.

Cenarios recomendados:

- Deve criar owner com `cashflowId`, `userId` e role `Owner`.
- Deve rejeitar `cashflowId` vazio.
- Deve rejeitar `userId` vazio.
- Deve rejeitar role invalida.
- Deve preencher `JoinedAt`.

Observacao: como os factories de contributor/viewer sao `internal`, os testes podem exigir `InternalsVisibleTo` se quisermos validar esses caminhos diretamente.

#### Title value object

Faltam testes para `CashflowContext/ValueObjects/Title`.

Cenarios recomendados:

- Deve criar title valido.
- Deve normalizar com `Trim`.
- Deve normalizar para lowercase, se essa regra for intencional.
- Deve rejeitar nulo, vazio e whitespace.
- Deve rejeitar titulo curto.
- Deve rejeitar titulo longo.
- Deve validar os limites exatos de tamanho, porque a implementacao atual usa `<= MinLength` e `>= MaxLength`.

#### CreateCashflow use case

Faltam testes para `CreateCashflowHandler` e `CreateCashflowCommandValidator`.

Cenarios recomendados:

- Deve criar cashflow quando usuario existe.
- Deve retornar `UserNotFound` quando usuario nao existe.
- Deve chamar `ICashflowRepository.AddAsync`.
- Deve chamar `IUnitOfWork.CommitAsync` somente em sucesso.
- Deve retornar `CashflowId` e title normalizado.
- Validator deve rejeitar title vazio.
- Validator deve rejeitar `UserId` vazio.
- Validator deveria cobrir minimo/maximo de title ou delegar explicitamente ao dominio.

#### GetCashflowBoard use case

Faltam testes para `GetCashflowBoardHandler`.

Cenarios recomendados:

- Deve retornar falha quando usuario nao e membro do cashflow.
- Deve retornar falha quando header nao existe.
- Deve retornar sucesso quando usuario e membro e header existe.
- Deve montar cinco colunas mensais.
- Deve montar periodo no formato esperado.
- Deve retornar os meses em ordem cronologica.
- Deve retornar saldo zero, `IsClosed = false` e lista vazia enquanto ainda nao ha transacoes.

Observacao: o handler usa `DateTime.UtcNow` diretamente, o que torna teste de meses menos deterministico. Antes de testar com rigor, vale considerar uma abstracao de relogio.

#### GetUserCashflows use case

Faltam testes para `GetUserCashflowHandler`.

Cenarios recomendados:

- Deve retornar lista de cashflows do usuario.
- Deve retornar lista vazia quando usuario nao possui cashflows.
- Deve chamar `ICashflowReadRepository.GetUserCashflowsAsync` com o `UserId` correto.

### Prioridade media

#### API controllers

Nao ha testes para controllers.

Cenarios recomendados:

- `UsersController` deve retornar `Created` em registro valido.
- `UsersController` deve retornar `BadRequest` em validacao invalida.
- `LoginController` deve retornar sucesso com token em login valido.
- `LoginController` deve retornar erro em credenciais invalidas.
- `CashflowsController.CreateCashflow` deve retornar `Unauthorized` quando claim de usuario esta ausente ou invalida.
- `CashflowsController.CreateCashflow` deve retornar `BadRequest` quando validator falha.
- `CashflowsController.CreateCashflow` deve retornar `Created` quando handler retorna sucesso.
- `CashflowsController.GetUserCashflows` deve retornar `Ok` com lista.
- `CashflowsController.GetUserCashflowBoard` deve retornar `NotFound` quando handler falha.

#### Infrastructure repositories

Nao ha testes para repositorios EF Core.

Cenarios recomendados:

- `UserRepository.ExistByEmailAsync` deve encontrar usuario existente.
- `UserRepository.GetByEmailAsync` deve retornar usuario esperado.
- `CashflowRepository.AddAsync` deve persistir cashflow com member owner.
- `CashflowReadRepository.GetUserCashflowsAsync` deve retornar somente cashflows onde o usuario e membro.
- `CashflowReadRepository.GetUserCashflowsAsync` deve projetar role e quantidade de membros corretamente.
- `CashflowReadRepository.GetCashflowBoardHeaderAsync` deve retornar null quando usuario nao e membro.
- `CashflowMemberReadRepository.HasMemberAsync` deve retornar true/false corretamente.

Esses testes podem ser de integracao com SQLite in-memory ou SQL Server test container. Evitar EF InMemory para queries relacionais mais importantes.

#### EF mappings

Nao ha testes ou validacao automatizada de mappings.

Cenarios recomendados:

- Verificar se owned value objects como `Title` sao persistidos e lidos corretamente.
- Verificar nomes de tabelas/colunas esperados.
- Verificar relacionamento `Cashflow` -> `CashflowMembers`.
- Verificar constraints obrigatorias.

#### Mediator

Nao ha testes dedicados para `Mediator`.

Cenarios recomendados:

- Deve resolver command handler registrado.
- Deve resolver query handler registrado.
- Deve propagar cancellation token via `WaitAsync`.
- Deve falhar claramente quando handler nao esta registrado.
- Deve falhar quando handler nao retorna `Task<TResponse>`.

### Prioridade media para frontend

#### Auth guard

Faltam testes para `auth-guard.ts`.

Cenarios recomendados:

- Deve permitir acesso quando usuario esta autenticado.
- Deve redirecionar/bloquear quando usuario nao esta autenticado.

#### Auth interceptor

Faltam testes para `auth-interceptor.ts`.

Cenarios recomendados:

- Deve adicionar header `Authorization` quando ha token.
- Nao deve adicionar header quando nao ha token.
- Deve preservar request original alem do header.

#### Identity forms

Faltam testes para `RegisterUserForm` e `LoginUserForm`.

Cenarios recomendados:

- Deve marcar campos como touched quando submit e invalido.
- Deve chamar service com payload correto quando formulario e valido.
- Deve lidar com sucesso emitindo/navegando conforme comportamento atual.
- Deve lidar com erro sem quebrar o componente.

#### Cashflow frontend

Faltam testes para `CashflowService`, `CreateCashflowForm`, `DashboardPage` e `CashflowBoard`.

Cenarios recomendados:

- `CashflowService.createCashflow` deve chamar `POST /api/cashflows`.
- `CashflowService.getUserCashflow` deve chamar `GET /api/cashflows`.
- `CashflowService.getCashflowBoard` deve chamar `GET /api/cashflows/{id}/board`.
- `CreateCashflowForm` deve validar titulo obrigatorio, minimo e maximo.
- `CreateCashflowForm` deve emitir `cashflowCreated` e `closeModal` apos sucesso.
- `DashboardPage` deve carregar cashflows no `ngOnInit`.
- `DashboardPage` deve selecionar cashflow ao receber evento do header card.
- `CashflowBoard` deve carregar board quando `cashflowId` muda.
- `CashflowBoard` deve exibir estado de erro quando service falha.

#### Componentes UI/shared

Faltam testes para componentes simples.

Cenarios recomendados:

- `Modal` deve emitir `closed`.
- `MenuButton` deve emitir `clicked`.
- `CashflowHeaderCard` deve emitir `selected` com o cashflow recebido.
- `TransactionCard` ainda e placeholder; testar somente depois de definir contrato visual.

## Lacunas relacionadas a Transaction

`Transaction` deve ser tratada como Aggregate Root proprio, associada a `Cashflow` por `CashflowId`.

O agregado `Cashflow` nao deve manter uma lista de transacoes no dominio. O relacionamento `Cashflow 1:N Transaction` deve ser garantido por FK no banco, mapeamento de persistencia e queries/read models.

Antes de implementar a US de criar transacao, os testes esperados deveriam cobrir:

- Criacao de transaction com cashflow valido.
- Rejeicao de `cashflowId` vazio.
- Rejeicao de title invalido.
- Rejeicao de amount menor ou igual a zero, quando `Amount` existir.
- Aceite apenas de `Income` e `Expense`.
- Definicao inicial de status.
- Definicao de `CreatedAt` e `UpdatedAt`.
- Associacao ao cashflow correto por `CashflowId`.
- Persistencia via `TransactionRepository`.
- Mapeamento EF da FK `transactions.cashflow_id -> cashflows.id`.
- Bloqueio de criacao em mes fechado, quando `ClosedMonth` existir.
- Endpoint de criacao retornando sucesso e erros de validacao.
- Frontend enviando payload correto e atualizando/recarregando o board apos sucesso.

## Gaps de qualidade dos testes existentes

- Os testes de application usam `Mediator` real para testar handlers. Isso valida a integracao com DI, mas mistura responsabilidade do handler com mediator. Para handlers, testes diretos tendem a ser mais simples; o mediator pode ter uma suite propria.
- `LoginUserHandlerTests` cobre usuario inexistente, mas falta senha invalida.
- `RegisterUserHandlerTests` cobre email duplicado, mas falta erro/validacao de campos invalidos.
- Nao ha testes dos validators de Identity.
- Nao ha testes de API para garantir status codes e formato de resposta.
- Nao ha cobertura automatizada para autenticao/autorizacao em endpoints protegidos.

## Ordem sugerida

1. Adicionar testes de dominio para `Title`, `Cashflow` e `CashflowMember`.
2. Adicionar testes de application para `CreateCashflowHandler`, `GetUserCashflowsHandler` e `GetCashflowBoardHandler`.
3. Adicionar testes de controller para os endpoints de cashflow e identity.
4. Adicionar testes de repositorio/read models com banco relacional de teste.
5. Adicionar specs Angular para services, guard, interceptor e formularios principais.
6. Ao iniciar `Transaction`, escrever primeiro os testes de dominio e handler para fixar contrato antes do frontend.
7. Incluir testes de mapping/repositorio para garantir a associacao entre `Transaction` e `Cashflow` por identidade, sem exigir `Cashflow.Transactions` no dominio.

## Observacao operacional

Esta analise foi feita por inspecao dos arquivos de teste e producao. Os comandos de teste (`dotnet test` e `npm test`) nao foram executados nesta etapa.
