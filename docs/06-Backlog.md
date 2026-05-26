# User Stories

## Identity

### Criar Conta

**[US-IDC-001]**

Como usuário não autenticado, quero criar uma conta para acessar a plataforma.

---

### Registrar Usuário

**[US-IDC-002]**

Como usuário não autenticado, quero registrar meus dados (e-mail e senha) para criar uma conta na plataforma.

**Critérios de aceitação:**

- O sistema deve permitir informar e-mail e senha.
- O sistema deve validar o formato do e-mail.
- O sistema deve garantir que o e-mail seja único.
- O sistema deve armazenar a senha de forma segura (hash).
- O sistema deve retornar confirmação após o registro com sucesso.

---

### Fazer Login

**[US-IDC-003]**

Como usuário, quero fazer login para acessar minha conta.

**Critérios de aceitação:**

- O sistema deve permitir informar e-mail e senha.
- O sistema deve validar as credenciais do usuário.
- O sistema deve retornar erro caso as credenciais sejam inválidas.
- O sistema deve gerar um token de autenticação ao logar com sucesso.
- O sistema deve permitir acesso às funcionalidades autenticadas após login.

---

## CASHFLOW CONTEXT

### Criar Cashflow

**[US-CFC-001]**

Como usuário autenticado quero criar um cashflow para começar a organizar minhas finanças.

**Critérios de aceitação:**

- O sistema deve permitir informar um título para o cashflow.
- O sistema deve validar o título informado.
- O sistema deve criar um novo cashflow associado ao usuário.
- O sistema deve automaticamente definir o usuário como `Owner` do cashflow.
- O sistema deve garantir que o cashflow possua exatamente um owner.
- O sistema deve retornar confirmação após a criação com sucesso.

### Mostrar Cashflow

**[US-CFC-002]**

Como usuário autenticado quero ver meu cashflow no dashboard para inserir minha primeira transação. 

**Critérios de aceitação:**

- O sistema deve mostrar no dashboard os cashflows vinculados ao usuário autenticado.
- Cada cashflow deve exibir
    - Título
    - Role
    - Quantidade de participantes

### Criar Transação

**[US-CFC-003]**

Como usuário autenticado quero adicionar uma receita ou despesa para registrar movimentações financeiras no meu cashflow

**Critérios de aceitação**:

- O sistema deve permitir informar:
    - Valor
    - Tipo (Income / Expense)
    - Data
    - Descrição
- O sistema deve associar a transação a um cashflow
- O sistema deve validar os dados informados
- O sistema deve impedir criação em mês fechado
- O sistema deve persistir a transação
- O sistema deve retornar confirmação de sucesso

**Task**: 

- [ ]  Criar entidade Transaction como Aggregate Root próprio
- [ ]  Associar Transaction ao Cashflow por `CashflowId`
- [ ]  Definir ValueObjects (Amount, Title)
- [ ]  Criar enum TransactionType (Income, Expense)
- [ ]  Implementar regra de mês fechado
- [ ]  Criar comando CreateTransactionCommand
- [ ]  Criar validator
- [ ]  Criar handler
- [ ]  Criar TransactionRepository
- [ ]  Criar endpoint POST /cashflows/{cashflowId}/transactions
- [ ]  Implementar form Angular

### Visualizar Dashboard Mensal

**[US-CFC-004]**

Como usuário autenticado quero visualizar minhas transações organizadas por mês para entender meu saldo e comportamento financeiro

**Critérios de aceitação:** 

- O sistema deve exibir:
    - Colunas por mês
    - Lista de transações por mês
    - Saldo mensal
- O sistema deve agrupar transações por mês
- O sistema deve calcular saldo:
    - Receitas (+)
    - Despesas (-)
- O sistema deve permitir visualizar meses sem transações

**Task**:

- [ ]  Criar GetCashflowDashboardQuery / GetCashflowBoardQuery
- [ ]  Criar ReadModel (MonthlyCashflowView)
- [ ]  Implementar agrupamento por mês
- [ ]  Implementar cálculo de saldo
- [ ]  Derivar Health Status do período aberto a partir das transações
- [ ]  Criar handler da query
- [ ]  Criar endpoint GET /cashflows/{cashflowId}/board
- [ ]  Criar estrutura de colunas no Angular
- [ ]  Renderizar transações por mês

## Atualizar Saldo Dinamicamente

**[US-CFC-005]**

Como usuário autenticado quero ver o saldo atualizado automaticamente ao adicionar ou remover transações para ter feedback imediato das minhas ações

**Critérios de aceitação:**

- O saldo deve ser atualizado após:
    - criação de transação
    - exclusão de transação
- A atualização deve ocorrer sem reload da página
- O dashboard deve refletir o estado atualizado

**Tasks:**

- [ ]  Atualizar estado local após criação
- [ ]  Evitar refetch completo (opcional)
- [ ]  Sincronizar UI com backend
- [ ]  Implementar feedback visual
