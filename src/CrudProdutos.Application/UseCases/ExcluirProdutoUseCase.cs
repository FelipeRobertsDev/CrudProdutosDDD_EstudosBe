using CrudProdutos.Domain.Repositories;

namespace CrudProdutos.Application.UseCases;

public sealed class ExcluirProdutoUseCase(IProdutoRepository repository)
{
    public async Task<bool> ExecutarAsync(Guid id)
    {
        var produto = await repository.ObterPorIdAsync(id);
        if (produto is null) return false;
        await repository.RemoverAsync(id);
        return true;
    }
}
