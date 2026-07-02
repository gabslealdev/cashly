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

- [x]  Criar entidade Transaction como Aggregate Root próprio
- [x]  Associar Transaction ao Cashflow por `CashflowId`
- [x]  Definir ValueObjects (Amount, Title)
- [x]  Criar enum TransactionType (Income, Expense)
- [x]  Implementar regra de mês fechado
- [x]  Criar comando CreateTransactionCommand
- [x]  Criar validator
- [x]  Criar handler
- [x]  Criar TransactionRepository
- [ ]  Criar endpoint POST /cashflows/{cashflowId}/transactions
- [x]  Implementar form Angular

### Visualizar Dashboard Mensal

**[US-CFC-004]**

Como usuário autenticado quero visualizar minhas transações organizadas por mês para entender meu saldo e comportamento financeiro

**Critérios de aceitação:** 

- O sistema deve exibir:
    - Colunas por mês
    - Lista de transações por mês
    - Resultado financeiro mensal
- O sistema deve agrupar transações por mês
- O sistema deve calcular resultado financeiro:
    - Receitas (+)
    - Despesas (-)
- O sistema deve permitir visualizar meses sem transações

**Task**:

- [x]  Criar GetCashflowDashboardQuery / GetCashflowBoardQuery
- [x]  Criar ReadModel (MonthlyCashflowView)
- [x]  Implementar agrupamento por mês
- [x]  Implementar cálculo de resultado financeiro do período
- [x]  Derivar Health Status do período aberto a partir das transações
- [x]  Criar handler da query
- [x]  Criar endpoint GET /cashflows/{cashflowId}/board
- [x]  Criar estrutura de colunas no Angular
- [x]  Renderizar transações por mês

## Atualizar Saldo Dinamicamente

**[US-CFC-005]**

Como usuário autenticado quero ver o resultado financeiro atualizado automaticamente ao adicionar ou remover transações para ter feedback imediato das minhas ações

**Critérios de aceitação:**

- O resultado financeiro deve ser atualizado após:
    - criação de transação
    - exclusão de transação
- A atualização deve ocorrer sem reload da página
- O dashboard deve refletir o estado atualizado

**Tasks:**

- [x]  Atualizar estado local após criação
- [ ]  Evitar refetch completo (opcional)
- [x]  Sincronizar UI com backend
- [x]  Implementar feedback visual
