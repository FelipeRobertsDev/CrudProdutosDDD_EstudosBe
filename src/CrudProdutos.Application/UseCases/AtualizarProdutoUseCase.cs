using CrudProdutos.Application.DTOs;
using CrudProdutos.Domain.Repositories;

namespace CrudProdutos.Application.UseCases;

public sealed class AtualizarProdutoUseCase(IProdutoRepository repository)
{
    public async Task<bool> ExecutarAsync(Guid id, SalvarProdutoRequest request)
    {
        var produto = await repository.ObterPorIdAsync(id);
        if (produto is null) return false; // Não encontrado.

        // A própria entidade valida e aplica a mudança (regra do domínio).
        produto.Atualizar(request.Nome, request.Preco);
        // O repositório em memória mantém a referência ao objeto já alterado.
        // Em banco real, normalmente teríamos um método para persistir a atualização.
        return true;
    }
}
