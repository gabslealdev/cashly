### BR-01

- **Descrição:** Usuário só se torna owner ao criar um cashflow
- **Contexto:** Cashflow
- **Condição:** Um usuário cria um novo `Cashflow`
- **Ação esperada:** O sistema cria automaticamente um `CashflowMember` com `Role = Owner`
- **Exceções:** Não se aplica
- **Origem:** Sistema
- **Impacto:** Cashflow, CashflowMember

---

### BR-02

- **Descrição:** Deve existir exatamente um owner por cashflow
- **Contexto:** Cashflow
- **Condição:** Um `Cashflow` possui membros
- **Ação esperada:** Garantir que exista apenas um `CashflowMember` com `Role = Owner`
- **Exceções:** Não se aplica
- **Origem:** Sistema
- **Impacto:** Cashflow, CashflowMember

---

### BR-03

- **Descrição:** Não permitir múltiplos owners
- **Contexto:** Collaboration
- **Condição:** Tentativa de adicionar ou alterar membro para `Owner`
- **Ação esperada:** Bloquear a operação caso já exista um owner
- **Exceções:** Transferência de ownership (caso implementado no futuro)
- **Origem:** Sistema
- **Impacto:** CashflowMember

---

### BR-04

- **Descrição:** Owner não pode ser removido
- **Contexto:** Collaboration
- **Condição:** Tentativa de remover um membro com `Role = Owner`
- **Ação esperada:** Bloquear a remoção
- **Exceções:** Transferência prévia de ownership (se implementado)
- **Origem:** Sistema
- **Impacto:** CashflowMember

---

### BR-05

- **Descrição:** Não permitir membros duplicados no mesmo cashflow
- **Contexto:** Collaboration
- **Condição:** Tentativa de adicionar um `User` já existente no mesmo `Cashflow`
- **Ação esperada:** Bloquear a adição
- **Exceções:** Não se aplica
- **Origem:** Sistema
- **Impacto:** CashflowMember

---

### BR-06

- **Descrição:** Não permitir fechar mês com transações pendentes
- **Contexto:** Cashflow / Transaction
- **Condição:** Existência de transações com status `Scheduled` no período
- **Ação esperada:** Bloquear o fechamento do mês
- **Exceções:** Não se aplica
- **Origem:** Sistema
- **Impacto:** ClosedMonth, Transaction

---

### BR-07

- **Descrição:** Não permitir mais de um fechamento por período
- **Contexto:** Cashflow
- **Condição:** Já existe um `ClosedMonth` com o mesmo (`CashflowId`, `Period`)
- **Ação esperada:** Bloquear a criação de um novo `ClosedMonth`
- **Exceções:** Não se aplica
- **Origem:** Sistema
- **Impacto:** ClosedMonth

---

### BR-08

- **Descrição:** Categoria deve existir no catálogo
- **Contexto:** Transaction
- **Condição:** Criação ou atualização de transação com categoria
- **Ação esperada:** Validar existência da categoria no catálogo seed
- **Exceções:** Não se aplica
- **Origem:** Sistema
- **Impacto:** Transaction, Category

---

### BR-09

- **Descrição:** Transição de Scheduled para Completed
- **Contexto:** Transaction
- **Condição:** Transação em status `Scheduled`
- **Ação esperada:** Permitir alteração para `Completed`
- **Exceções:** Mês fechado
- **Origem:** Sistema
- **Impacto:** Transaction

---

### BR-10

- **Descrição:** Transição de Scheduled para Canceled
- **Contexto:** Transaction
- **Condição:** Transação em status `Scheduled`
- **Ação esperada:** Permitir alteração para `Canceled`
- **Exceções:** Mês fechado
- **Origem:** Sistema
- **Impacto:** Transaction

---

### BR-11

- **Descrição:** Transição de Completed para Canceled (opcional)
- **Contexto:** Transaction
- **Condição:** Transação em status `Completed`
- **Ação esperada:** Permitir alteração para `Canceled` (se habilitado)
- **Exceções:** Mês fechado
- **Origem:** Sistema
- **Impacto:** Transaction

---

### BR-12

- **Descrição:** Transação cancelada não pode voltar a estados anteriores
- **Contexto:** Transaction
- **Condição:** Transação em status `Canceled`
- **Ação esperada:** Bloquear alteração para `Scheduled` ou `Completed`
- **Exceções:** Não se aplica
- **Origem:** Sistema
- **Impacto:** Transaction
