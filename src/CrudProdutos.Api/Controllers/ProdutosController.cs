using CrudProdutos.Application.DTOs;
using CrudProdutos.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace CrudProdutos.Api.Controllers;

[ApiController]
[Route("api/produtos")]
public sealed class ProdutosController : ControllerBase
{
    // ETAPA 1: aqui começa o fluxo HTTP. O Controller recebe a requisição e chama o Use Case.
    // Ele NÃO deve conter regra de negócio, nem acessar diretamente o repositório.

    [HttpPost]
    public async Task<ActionResult<ProdutoDto>> Criar(
        [FromBody] SalvarProdutoRequest request, [FromServices] CriarProdutoUseCase useCase)
    {
        try
        {
            var produto = await useCase.ExecutarAsync(request);
            // 201 Created; o header Location aponta para a rota de consulta por ID.
            return CreatedAtAction(nameof(ObterPorId), new { id = produto.Id }, produto);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { erro = ex.Message }); // 400: dados inválidos.
        }
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProdutoDto>>> Listar(
        [FromServices] ListarProdutosUseCase useCase)
        => Ok(await useCase.ExecutarAsync()); // 200: lista de produtos.

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProdutoDto>> ObterPorId(
        Guid id, [FromServices] ObterProdutoUseCase useCase)
    {
        var produto = await useCase.ExecutarAsync(id);
        return produto is null ? NotFound() : Ok(produto); // 404 ou 200.
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Atualizar(
        Guid id, [FromBody] SalvarProdutoRequest request,
        [FromServices] AtualizarProdutoUseCase useCase)
    {
        try
        {
            var encontrado = await useCase.ExecutarAsync(id, request);
            return encontrado ? NoContent() : NotFound(); // 204 ou 404.
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Excluir(
        Guid id, [FromServices] ExcluirProdutoUseCase useCase)
    {
        var encontrado = await useCase.ExecutarAsync(id);
        return encontrado ? NoContent() : NotFound(); // 204 ou 404.
    }
}
