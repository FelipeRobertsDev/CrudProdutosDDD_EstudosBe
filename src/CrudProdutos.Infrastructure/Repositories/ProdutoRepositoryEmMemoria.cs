using System.Collections.Concurrent;
using CrudProdutos.Domain.Entities;
using CrudProdutos.Domain.Repositories;

namespace CrudProdutos.Infrastructure.Repositories;

// CAMADA INFRASTRUCTURE: aqui fica o detalhe técnico de onde guardar os dados.
// Usamos memória para ensinar as camadas SEM precisar instalar um banco de dados.
// ATENÇÃO: ao reiniciar a API, os produtos desaparecem.
public sealed class ProdutoRepositoryEmMemoria : IProdutoRepository
{
    private readonly ConcurrentDictionary<Guid, Produto> _produtos = new();

    public Task AdicionarAsync(Produto produto)
    {
        _produtos[produto.Id] = produto;
        return Task.CompletedTask;
    }

    public Task<Produto?> ObterPorIdAsync(Guid id)
    {
        _produtos.TryGetValue(id, out var produto);
        return Task.FromResult(produto);
    }

    public Task<IReadOnlyList<Produto>> ListarAsync()
        => Task.FromResult<IReadOnlyList<Produto>>(_produtos.Values.ToList());

    public Task RemoverAsync(Guid id)
    {
        _produtos.TryRemove(id, out _);
        return Task.CompletedTask;
    }
}
