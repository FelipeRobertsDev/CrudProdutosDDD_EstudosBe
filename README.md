# CRUD de Produtos — ASP.NET Core Web API + DDD + Use Cases

Projeto **intencionalmente simples e comentado**, feito para quem está começando em C#. É um exemplo didático de separação de responsabilidades inspirada em DDD (Domain-Driven Design), **não** uma implementação de todos os padrões avançados de DDD.

## 1. O que vamos construir?

Uma API para cadastrar, listar, consultar, atualizar e excluir produtos (`Id`, `Nome`, `Preco`).

| Ação | Método e rota | Resposta esperada |
| --- | --- | --- |
| Criar | `POST /api/produtos` | `201 Created` |
| Listar | `GET /api/produtos` | `200 OK` |
| Buscar | `GET /api/produtos/{id}` | `200 OK` ou `404 Not Found` |
| Atualizar | `PUT /api/produtos/{id}` | `204 No Content` ou `404 Not Found` |
| Excluir | `DELETE /api/produtos/{id}` | `204 No Content` ou `404 Not Found` |

## 2. Estrutura de pastas

```text
CrudProdutosDDD/
├── CrudProdutosDDD.sln
├── CrudProdutos.http                # Requisições para testar pelo editor
├── README.md
└── src/
    ├── CrudProdutos.Api/
    │   ├── Controllers/ProdutosController.cs  # Recebe HTTP (etapa 1)
    │   └── Program.cs                         # Configura DI (etapa 0)
    ├── CrudProdutos.Application/
    │   ├── DTOs/                              # Objetos de entrada/saída
    │   └── UseCases/                          # Uma ação por classe (etapa 2)
    ├── CrudProdutos.Domain/
    │   ├── Entities/Produto.cs                # Regras do Produto (etapa 3)
    │   └── Repositories/IProdutoRepository.cs # Contrato de persistência
    └── CrudProdutos.Infrastructure/
        └── Repositories/ProdutoRepositoryEmMemoria.cs # Guarda em memória (etapa 4)
```

## 3. Caminho de uma requisição (leia nesta ordem!)

```text
Cliente (Postman / arquivo .http / curl)
             |
             v
[1] API: ProdutosController.Criar()     recebe POST e dados JSON
             |
             v
[2] Application: CriarProdutoUseCase    coordena o caso de uso
             |
             v
[3] Domain: Produto                     valida nome e preço
             |
             v
[4] Interface IProdutoRepository       define como salvar
             |
             v
    Infrastructure: Repository em memória guarda o produto
             |
             v
[5] Application devolve ProdutoDto
             |
             v
    API devolve HTTP 201 e o JSON do produto
```

**Quem depende de quem?** `Api -> Application -> Domain`; `Api -> Infrastructure -> Domain`. O **Domain não depende das outras camadas**. Application usa apenas a interface do repositório (definida no Domain), não a classe em memória. `Program.cs` conecta as duas coisas por **injeção de dependência (DI)**.

## 4. Como executar

Pré-requisito: **.NET SDK 8** (ou SDK compatível capaz de compilar projetos `net8.0`).

Abra um terminal na pasta do repositório e execute:

```bash
dotnet restore
dotnet run --project src/CrudProdutos.Api --launch-profile http
```

A API ficará em `http://localhost:5050`. Para testar, abra `CrudProdutos.http` no Visual Studio ou VS Code com extensão de cliente HTTP, ou use curl.

**Exemplo criando um produto:**

```bash
curl -i -X POST http://localhost:5050/api/produtos \
  -H "Content-Type: application/json" \
  -d '{"nome":"Caderno","preco":19.90}'
```

Você receberá um JSON com `id`, `nome` e `preco`. Copie o `id` para os testes seguintes. Para listar:

```bash
curl http://localhost:5050/api/produtos
```

Para consultar, atualizar e excluir, substitua `SEU-ID` pelo GUID gerado:

```bash
curl http://localhost:5050/api/produtos/SEU-ID
curl -i -X PUT http://localhost:5050/api/produtos/SEU-ID \
  -H "Content-Type: application/json" \
  -d '{"nome":"Caderno grande","preco":24.90}'
curl -i -X DELETE http://localhost:5050/api/produtos/SEU-ID
```

**Tente errar de propósito:** enviar nome vazio (`"nome":""`) ou preço negativo (`"preco":-1`) deve retornar `400 Bad Request`. Procurar um ID que não existe deve retornar `404`.

## 5. O papel de cada camada

- **Domain:** regras e conceitos fundamentais. `Produto.Atualizar` impede preço negativo e nome vazio. Não sabe que existe uma API.
- **Application:** casos de uso (`CriarProdutoUseCase`, etc.). Organiza os passos, consulta o repositório e produz DTOs. Não conhece HTTP.
- **Infrastructure:** implementa os detalhes técnicos, neste exemplo uma coleção em memória. Futuramente poderia usar EF Core e SQL Server.
- **Api:** traduz HTTP/JSON para chamadas aos casos de uso e transforma o resultado em status HTTP.

> **Por que uma interface?** O caso de uso conhece `IProdutoRepository`, não `ProdutoRepositoryEmMemoria`. Se trocarmos a implementação por banco de dados, o caso de uso pode permanecer igual (desde que o contrato seja respeitado).

> **Por que um Use Case por ação?** Para estudar claramente a responsabilidade de cada operação. Em projetos grandes, podem existir outros padrões e abstrações.

## 6. Ordem de estudo sugerida

1. `src/CrudProdutos.Api/Program.cs` — onde tudo é registrado.
2. `src/CrudProdutos.Api/Controllers/ProdutosController.cs` — onde a requisição chega.
3. `src/CrudProdutos.Application/UseCases/CriarProdutoUseCase.cs` — quem coordena.
4. `src/CrudProdutos.Domain/Entities/Produto.cs` — quem contém as regras.
5. `src/CrudProdutos.Domain/Repositories/IProdutoRepository.cs` — o contrato.
6. `src/CrudProdutos.Infrastructure/Repositories/ProdutoRepositoryEmMemoria.cs` — onde os dados ficam.
7. Os outros Use Cases — compare o fluxo de leitura, edição e exclusão.

## 7. Limites intencionais deste exemplo

- **Sem banco de dados:** os dados somem ao parar a API. Isso evita complicar o primeiro contato com DDD.
- **Sem autenticação, migrations, tratamento global de erros ou testes automatizados:** são tópicos para a próxima etapa.
- **Atualização em memória:** o objeto retornado é a mesma referência guardada no dicionário. Um banco real precisaria de código de persistência específico.
- **Assinaturas `async`:** ilustram contratos comuns de acesso a dados; a implementação em memória retorna tarefas concluídas, sem I/O real.

## 8. Pequenos desafios para praticar

1. Adicionar o campo `Descricao` ao produto.
2. Impedir que o preço seja `0` (alterando a regra no Domain).
3. Criar um caso de uso que liste apenas produtos acima de um valor.
4. Trocar o repositório em memória por um banco usando Entity Framework Core.

**Dica:** leia os comentários `ETAPA 0` a `ETAPA 5` nos arquivos; eles acompanham uma requisição de criação ponta a ponta.
