using CrudProdutos.Application.DTOs;
using CrudProdutos.Domain.Repositories;

namespace CrudProdutos.Application.UseCases;

// Fluxo de leitura: buscar no repositório e transformar entidades em DTOs.
public sealed class ListarProdutosUseCase(IProdutoRepository repository)
{
    public async Task<IReadOnlyList<ProdutoDto>> ExecutarAsync()
    {
        var produtos = await repository.ListarAsync();
        return produtos.Select(p => new ProdutoDto(p.Id, p.Nome, p.Preco)).ToList();
    }
}
