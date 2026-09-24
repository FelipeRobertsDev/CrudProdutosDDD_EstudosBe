using CrudProdutos.Application.DTOs;
using CrudProdutos.Domain.Entities;
using CrudProdutos.Domain.Repositories;

namespace CrudProdutos.Application.UseCases;

// ETAPA 2: a Application executa a intenção do usuário: criar um produto.
public sealed class CriarProdutoUseCase(IProdutoRepository repository)
{
    public async Task<ProdutoDto> ExecutarAsync(SalvarProdutoRequest request)
    {
        // ETAPA 3: o domínio cria e valida o produto.
        var produto = new Produto(request.Nome, request.Preco);

        // ETAPA 4: salvamos usando a interface, sem conhecer a implementação.
        await repository.AdicionarAsync(produto);

        // ETAPA 5: devolvemos um DTO, não a entidade diretamente.
        return new ProdutoDto(produto.Id, produto.Nome, produto.Preco);
    }
}
