# Contributing

Este guia define o fluxo mínimo para manter o histórico do Cashly organizado e as mudanças fáceis de revisar.

## Branches

Use nomes curtos, em minúsculo e com kebab-case:

```text
<tipo>/<descricao-curta>
```

Tipos recomendados:

- `feature/` para nova funcionalidade.
- `fix/` para correção de bug.
- `docs/` para documentação.
- `refactor/` para refatoração sem mudança de comportamento.
- `test/` para testes.
- `chore/` para manutenção, configs e dependências.
- `infra/` para Docker, CI/CD, deploy e infraestrutura.

Exemplos:

```text
feature/monthly-closing
fix/transaction-status-validation
docs/update-readme
refactor/cashflow-board-handler
test/create-transaction-handler
```

## Commits

Use Conventional Commits:

```text
<tipo>(escopo opcional): descrição curta
```

Exemplos:

```text
feat(cashflow): add monthly board query
fix(transaction): validate invalid status
docs(readme): update project status
refactor(api): rename transaction route
test(domain): cover financial health classifier
chore(gitignore): ignore local editor files
```

Prefira commits pequenos e coesos. Evite misturar feature, refactor e documentação no mesmo commit quando puder separar.

## Pull Requests

Antes de abrir um PR:

- Atualize sua branch com a base mais recente.
- Descreva objetivamente o que mudou.
- Informe como a mudança foi testada.
- Cite migrations, alterações de contrato de API ou impacto em configuração.
- Mantenha o PR focado em um objetivo principal.

Checklist recomendado:

```text
- [ ] Build do backend executado quando aplicável.
- [ ] Testes do backend executados quando aplicável.
- [ ] Build/testes do frontend executados quando aplicável.
- [ ] README/docs atualizados quando a mudança altera uso, setup ou comportamento.
- [ ] Nenhum arquivo local, secreto ou gerado foi incluído por engano.
```

## Validação Local

Backend:

```bash
dotnet build src/backend/Cashly.sln
dotnet test src/backend/Cashly.sln
```

Frontend:

```bash
cd src/frontend/cashly-web
npm install
npm test
npm run build
```

Use os comandos relevantes para o tipo de mudança. Uma alteração apenas em documentação não precisa rodar toda a suite.

## Configurações e Secrets

- Não versione `.env`, secrets, arquivos de usuário da IDE, `bin/`, `obj/`, `node_modules/` ou outputs de build.
- Use `.env.example` para documentar variáveis obrigatórias sem valores sensíveis.
- Preferencialmente configure JWT e connection strings via variáveis de ambiente, user secrets ou arquivos locais ignorados.

## Escopo

Mantenha mudanças pequenas e revisáveis. Se uma tarefa revelar outro problema, prefira abrir uma nova tarefa ou PR separado, a menos que o ajuste seja necessário para concluir a mudança atual.
