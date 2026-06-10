### Cashflow

Representa o controle financeiro de um usuário ou grupo de usuários.

É o agregado responsável pela identidade do controle financeiro, seus membros e regras de colaboração.

Transações pertencem conceitualmente a um cashflow, mas são modeladas como agregado próprio e associadas por `CashflowId`.

---

### Transaction

Representa uma movimentação financeira dentro de um cashflow.

Pode ser uma receita (entrada) ou despesa (saída), impactando o saldo do período.

É um agregado próprio, associado a um `Cashflow` por identidade (`CashflowId`), sem compor a fronteira do agregado `Cashflow`.

---

### Closed Month

Representa um mês que foi encerrado dentro de um cashflow.

Após o fechamento, os dados do período tornam-se imutáveis, preservando o histórico financeiro.

Armazena o resultado financeiro líquido do período e o status financeiro classificado no momento do fechamento.

---

### Period

Representa um intervalo de tempo baseado em mês e ano (ex: 2026-04).

É utilizado para organizar transações e fechamentos mensais.

---

### Period Financial Result

Representa a apuração financeira de um período.

Consolida `TotalIncome`, `TotalExpense` e `PeriodResult` a partir das transações completas do período.

---

### Period Result

Representa o resultado financeiro líquido de um período.

É calculado como receitas completas menos despesas completas do período.

---

### Health Status

Representa a saúde financeira de um período.

É classificado a partir do resultado financeiro do período e da proporção entre sobra e renda.

---

### Money

Representa um valor monetário dentro do domínio.

É utilizado para cálculos financeiros, como valores de transações e saldos.

---

### Title

Representa um identificador textual de entidades como Cashflow ou Transaction.

É utilizado para descrever e facilitar a identificação no sistema.

---

### Cashflow Member

Representa a participação de um usuário em um cashflow compartilhado.

Define o nível de acesso e permissões dentro do contexto de colaboração.

---

### Role

Representa o papel de um usuário dentro de um cashflow.

Define suas permissões no sistema (ex: Owner, Contributor, Viewer).

---

### Owner

Usuário com controle total sobre o cashflow.

Pode gerenciar membros, transações e configurações.

---

### Contributor

Usuário com permissão para criar e editar transações dentro do cashflow.

Não possui controle administrativo completo.

---

### Viewer

Usuário com acesso apenas de leitura ao cashflow.

Não pode modificar dados.

---

### Scheduled Transaction

Transação que ainda não foi efetivamente realizada.

Está planejada para uma data futura.

---

### Completed Transaction

Transação que já foi realizada e impacta a apuração financeira do período.

---

### Canceled Transaction

Transação que foi invalidada e não deve impactar a apuração financeira.

---

### Month Closing Policy

Regra de domínio que define se um mês pode ser fechado.

Um mês não pode ser fechado enquanto houver transações agendadas no período.
