## Objetivo Organizacional

Criar uma aplicação simples e intuitiva para controle financeiro pessoal, permitindo que usuários organizem receitas e despesas por período mensal com visualização clara do saldo.

### Resultado esperado

- Usuários consigam registrar e acompanhar suas finanças sem complexidade
- Redução de desorganização financeira pessoal
- Interface mais simples que apps bancários

### 📊 Indicadores de sucesso

- % de usuários que registram pelo menos 1 transação por semana
- Tempo médio de uso da aplicação
- Número de transações registradas por usuário
- Retenção mensal de usuários

## Requisitos de Stakeholders

### Usuário Final

- **Necessidade:** Controlar suas finanças de forma simples
- **Motivação:** Evitar desorganização financeira e falta de controle melhorar previsibilidade de gastos.

# Requisitos Funcionais

---

## RF-01 — Registro de Usuário

- **Descrição:** O sistema deve permitir o cadastro de novos usuários
- **Entrada:** Nome, email, senha
- **Saída:** Usuário criado
- **Regras:**
    - Email deve ser único
    - Senha deve ser válida
- **Exceções:**
    - Email já cadastrado
- **Prioridade:** Alta

---

## RF-02 — Login de Usuário

- **Descrição:** O sistema deve autenticar usuários
- **Entrada:** Email, senha
- **Saída:** Token JWT
- **Regras:**
    - Credenciais devem ser válidas
- **Exceções:**
    - Usuário não encontrado
    - Senha inválida
- **Prioridade:** Alta

---

## RF-03 — Criar Cashflow

- **Descrição:** O sistema deve permitir a criação de um cashflow
- **Entrada:** Nome do cashflow
- **Saída:** Cashflow criado
- **Regras:**
    - Usuário deve ser owner
- **Exceções:**
    - Nome inválido
- **Prioridade:** Alta

---

## RF-04 — Adicionar Transação

- **Descrição:** Permitir adicionar receita ou despesa
- **Entrada:** Valor, tipo, data, descrição
- **Saída:** Transação criada
- **Regras:**
    - Não permitir transação em mês fechado
- **Exceções:**
    - Mês fechado
- **Prioridade:** Alta

---

## RF-05 — Visualizar Dashboard Mensal

- **Descrição:** Exibir transações por mês em formato de colunas
- **Entrada:** Período
- **Saída:** Lista de transações + resultado financeiro do período
- **Regras:**
    - Agrupar por mês
- **Exceções:**
    - Nenhuma transação
- **Prioridade:** Alta

---

## RF-06 — Alterar Status da Transação

- **Descrição:** Alterar entre Scheduled / Completed
- **Entrada:** ID da transação
- **Saída:** Status atualizado
- **Regras:**
    - Toggle entre estados
- **Exceções:**
    - Transação não encontrada
- **Prioridade:** Média

# 🔹 Requisitos Não Funcionais

---

## 🔐 Segurança

- **Descrição:** Autenticação via JWT
- **Métrica:** Token válido e protegido
- **Impacto arquitetural:** Camada de autenticação + middleware

---

## ⚡ Performance

- **Descrição:** Dashboard deve carregar rapidamente
- **Métrica:** < 2 segundos
- **Impacto:** Queries otimizadas + possível uso de Dapper no futuro

---

## 📦 Escalabilidade

- **Descrição:** Suportar múltiplos usuários e múltiplos cashflows
- **Métrica:** Crescimento linear sem degradação
- **Impacto:** Modelagem com aggregates + separation of concerns

---

## 🧱 Manutenibilidade

- **Descrição:** Código fácil de evoluir
- **Métrica:** Baixo acoplamento
- **Impacto:** Clean Architecture + DDD

---

## 📱 Usabilidade

- **Descrição:** Interface simples e intuitiva
- **Métrica:** Usuário consegue usar sem tutorial
- **Impacto:** UX + Angular organizado por features
