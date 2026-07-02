# Contributing

Este guia define o fluxo minimo para manter o historico do Cashly organizado e as mudancas faceis de revisar.

## Branches

Use nomes curtos, em minusculo e com kebab-case:

```text
<tipo>/<descricao-curta>
```

Tipos recomendados:

- `feature/` para nova funcionalidade.
- `fix/` para correcao de bug.
- `docs/` para documentacao.
- `refactor/` para refatoracao sem mudanca de comportamento.
- `test/` para testes.
- `chore/` para manutencao, configs e dependencias.
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
<tipo>(escopo opcional): descricao curta
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

Prefira commits pequenos e coesos. Evite misturar feature, refactor e documentacao no mesmo commit quando puder separar.

## Pull Requests

Antes de abrir um PR:

- Atualize sua branch com a base mais recente.
- Descreva objetivamente o que mudou.
- Informe como a mudanca foi testada.
- Cite migrations, alteracoes de contrato de API ou impacto em configuracao.
- Mantenha o PR focado em um objetivo principal.

Checklist recomendado:

```text
- [ ] Build do backend executado quando aplicavel.
- [ ] Testes do backend executados quando aplicavel.
- [ ] Build/testes do frontend executados quando aplicavel.
- [ ] README/docs atualizados quando a mudanca altera uso, setup ou comportamento.
- [ ] Nenhum arquivo local, secreto ou gerado foi incluido por engano.
```

## Validacao Local

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

Use os comandos relevantes para o tipo de mudanca. Uma alteracao apenas em documentacao nao precisa rodar toda a suite.

## Configuracoes e Secrets

- Nao versione `.env`, secrets, arquivos de usuario da IDE, `bin/`, `obj/`, `node_modules/` ou outputs de build.
- Use `.env.example` para documentar variaveis obrigatorias sem valores sensiveis.
- Preferencialmente configure JWT e connection strings via variaveis de ambiente, user secrets ou arquivos locais ignorados.

## Escopo

Mantenha mudancas pequenas e revisaveis. Se uma tarefa revelar outro problema, prefira abrir uma nova tarefa ou PR separado, a menos que o ajuste seja necessario para concluir a mudanca atual.
