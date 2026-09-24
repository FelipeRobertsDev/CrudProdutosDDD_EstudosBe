using CrudProdutos.Application.DTOs;
using CrudProdutos.Domain.Repositories;

namespace CrudProdutos.Application.UseCases;

public sealed class ObterProdutoUseCase(IProdutoRepository repository)
{
    public async Task<ProdutoDto?> ExecutarAsync(Guid id)
    {
        var produto = await repository.ObterPorIdAsync(id);
        // Se não achou, retorna null; o controller decide responder HTTP 404.
        return produto is null ? null : new ProdutoDto(produto.Id, produto.Nome, produto.Preco);
    }
}
