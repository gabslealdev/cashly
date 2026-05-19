# Subdomínios

## Identity Context - Generic

Responsabilidade: Gerenciar identidade e autenticação dos usuários do sistema.

- Registro de usuário
- Autenticação de usuário
- Gerenciamento de credenciais

### Entidades

#### USER

- `Id`→ Guid
- `Name` → Value Object
- `Email` → Value Object
- `PasswordHash` → Value Object
- `CreatedAt` → DateTimeOffset

Responsabilidades

- Representar um usuário.
- Garantir a unicidade do e-mail
- Garantir que a senha seja armazenada em hash

### Objetos de Valor

`Name` 

- FirstName → string
- LastName → string

**Descrição:** 

Representa o nome completo do usuário dentro do domínio.

**Regras:**

- `FirstName` e `LastName` são obrigatórios.
- Não podem ser nulos, vazios ou conter apenas espaços.
- Devem respeitar um limite mínimo e máximo de caracteres (ex: 2 a 50).
- Devem ser normalizados (ex: trim de espaços, padronização de casing se necessário).

**Comportamentos:**

- Expor o nome completo formatado (ex: `FullName`).
- Garantir validação no momento da criação.

`Email`

- Value → string

**Descrição:**

Representa o e-mail do usuário dentro do domínio, utilizado como identificador para autenticação.

**Regras:**

- É obrigatório.
- Não pode ser nulo, vazio ou conter apenas espaços.
- Deve possuir um formato válido de e-mail.
- Deve ser normalizado (ex: trim de espaços e conversão para lowercase).
- Deve respeitar um limite máximo de caracteres (ex: 254).
**Comportamentos:**
- Expor o valor normalizado.
- Garantir validação no momento da criação.

`PasswordHash`

- Value → string

**Descrição:**

Representa o hash da senha do usuário armazenado de forma segura no domínio.

**Regras:**

- É obrigatório.
- Não pode ser nulo, vazio ou conter apenas espaços.
- Deve sempre estar previamente criptografado (hash).
- Nunca deve representar uma senha em texto puro.
- Deve respeitar o formato esperado pelo algoritmo de hash utilizado.

**Comportamentos:**

- Garantir validação no momento da criação.
- Permitir armazenamento seguro da credencial do usuário.

---

# CASHFLOW CONTEXT

Responsabilidade: Gerenciar o controle financeiro dos usuários, incluindo transações, saldos e histórico. 

- Criação de Cashflows
- Registro de transações (receitas/despesas)
- Cálculo de saldo por período
- Status financeiro (health, closed month)
- Organização por período (mês)

# Entidades

## CASHFLOW

- `Id` → Guid
- `Title` → Value Object
- `CreatedAt` → DateTimeOffset
- `UpdatedAt` → DateTimeOffset

**Responsabilidades**: 

- Armazenar todas as transações do mês atual.
- Armazenar informações sobre meses anteriores
- Armazenar todos os membros do cashflow
- Garantir que as alterações respeitem regras de domínio.
- Coordenar o fechamento do mês.
- Ponto de entrada para operações relacionadas às transações e ao estado financeiro
- Garantir que só haja um único `owner` em cada cashflow

### Objetos de Valor

`Title`

- Value → string

**Descrição:**

Representa o título de um Cashflow, utilizado para identificar e descrever o controle financeiro do usuário.

**Regras:**

- É obrigatório.
- Não pode ser nulo, vazio ou conter apenas espaços.
- Deve respeitar um limite mínimo e máximo de caracteres (ex: 3 a 100).
- Deve ser normalizado (ex: trim de espaços).
- Não deve conter apenas caracteres inválidos ou irrelevantes (ex: somente símbolos).

**Comportamentos:**

- Expor o valor formatado.
- Garantir validação no momento da criação.

### TRANSACTION

- `Id` → Guid
- `CashflowId` → Guid
- `Title` → Value Object
- `Amount` → Value Object
- `Type` → Enum (Income | Expense)
- `Date` → DateTimeOffset
- `Status` → Enum (Scheduled | Completed | Canceled)
- `CreatedAt` → DateTimeOffset
- `UpdatedAt` → DateTimeOffset

Responsabilidades

- Representar uma movimentação financeira (`income` ou `expense`).
- Garantir a consistência dos dados financeiros.
- Garantir que a transação pertença a um único Cashflow.
- Garantir que regras de período (ex: mês fechado) sejam respeitadas.

---

### Objetos de Valor

`Title`

- Value → string

**Descrição:**

Representa o título da transação, utilizado para identificar a movimentação financeira.

**Regras:**

- É obrigatório.
- Não pode ser nulo, vazio ou conter apenas espaços.
- Deve respeitar um limite mínimo e máximo de caracteres (ex: 3 a 100).
- Deve ser normalizado (ex: trim de espaços).

**Comportamentos:**

- Expor o valor formatado.
- Garantir validação no momento da criação.

---

`Amount`

- Value → decimal

**Descrição:**

Representa o valor monetário da transação.

**Regras:**

- É obrigatório.
- Deve ser maior que zero.
- Deve respeitar a precisão monetária (ex: 2 casas decimais).
- Não pode ser negativo (o tipo da transação define se é entrada ou saída).

**Comportamentos:**

- Expor o valor monetário.
- Garantir validação no momento da criação.

### CLOSEDMONTH

- `Id` → Guid
- `CashflowId` → Guid
- `Period` → Value Object
- `OpeningBalance` → Value Object
- `ClosingBalance` → Value Object
- `Status` → Enum (Critical, Warning, Attention, Healthy, Excellent)
- `ClosedAt` → DateTimeOffset

Responsabilidades

- Representar o fechamento financeiro de um mês.
- Armazenar o saldo inicial e final do período fechado.
- Registrar o status financeiro do mês no momento do fechamento.
- Garantir que um mês fechado se torne imutável.
- Preservar o histórico financeiro do Cashflow.

**Regras principais:**

- Um `ClosedMonth` deve estar sempre associado a um único `Cashflow`.
- Um mesmo `Cashflow` não pode possuir dois fechamentos para o mesmo período.
- O fechamento só pode existir para um período válido.
- Após fechado, o mês não deve permitir alterações em seus dados.
- O `ClosingBalance` deve refletir o resultado consolidado do período no momento do fechamento.
- O `Status` do mês deve ser derivado das regras do domínio, e não definido livremente de forma manual.

---

### Objetos de Valor

`Period`

- `Year` → int
- `Month` → int

**Descrição:**

Representa o período de referência de um fechamento mensal dentro do domínio.

**Regras:**

- `Year` e `Month` são obrigatórios.
- `Month` deve estar entre 1 e 12.
- O período deve representar um mês válido do calendário.
- Deve ser tratado de forma padronizada para comparação e ordenação.

**Comportamentos:**

- Expor o período formatado (ex: `2026-04`).
- Garantir validação no momento da criação.

---

`Money`

- `Value` → decimal

**Descrição:**

Representa um valor monetário dentro do domínio.

**Regras:**

- É obrigatório.
- Deve respeitar a precisão monetária definida pelo domínio (ex: 2 casas decimais).
- Pode assumir valor positivo, zero ou negativo, conforme o contexto.
- Deve ser armazenado em formato apropriado para cálculos financeiros.

**Comportamentos:**

- Expor o valor monetário.
- Garantir validação no momento da criação.
- Permitir operações de comparação.
- Permitir operações aritméticas do domínio, quando aplicável.

## Collaboration Context - Supporting

**Responsabilidade:** Gerenciar a colaboração entre usuários em um cashflow compartilhado, controlando membros, papéis e permissões de acesso.

### Capacidades principais

- Adicionar membros a um cashflow
- Remover membros de um cashflow
- Definir papéis de acesso
- Controlar permissões dentro do cashflow compartilhado
- Garantir que a colaboração respeite as regras do domínio

---

### Entidades

### CASHFLOWMEMBER

- `Id` → Guid
- `CashflowId` → Guid
- `UserId` → Guid
- `Role` → Enum (Owner | Contributor | Viewer)
- `CreatedAt` → DateTimeOffset
- `UpdatedAt` → DateTimeOffset

Responsabilidades

- Representar a participação de um usuário em um cashflow.
- Definir o nível de acesso do usuário dentro do cashflow.
- Garantir que cada membro pertença a um único cashflow.
- Garantir que as regras de colaboração sejam respeitadas.

**Regras principais:**

- Um `CashflowMember` deve estar sempre associado a um único `Cashflow`.
- Um `CashflowMember` deve estar sempre associado a um único `User`.
- Um mesmo usuário não pode ser adicionado mais de uma vez ao mesmo cashflow.
- Todo cashflow deve possuir pelo menos um membro com papel `Owner`.
- O membro com papel `Owner` não pode ser removido sem uma regra explícita de transferência de ownership.
- As permissões do membro devem ser derivadas do papel atribuído.

---

### Objetos de Valor

`Role`

- `Value` → string

**Descrição:**

Representa o papel do usuário dentro de um cashflow compartilhado, definindo seu nível de permissão.

**Regras:**

- É obrigatório.
- Deve corresponder a um papel válido do domínio.
- Deve respeitar os valores permitidos pelo sistema (ex: `Owner`, `Contributor`, `Viewer`).
- Não pode assumir valores arbitrários fora das opções definidas.

**Comportamentos:**

- Expor o papel atribuído ao membro.
- Garantir validação no momento da criação.
- Permitir a identificação das permissões associadas ao papel.
