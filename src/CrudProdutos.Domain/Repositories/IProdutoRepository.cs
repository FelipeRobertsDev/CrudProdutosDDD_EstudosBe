using CrudProdutos.Domain.Entities;

namespace CrudProdutos.Domain.Repositories;

// CONTRATO: descreve o que precisamos guardar/consultar, sem decidir COMO guardar.
// Infrastructure implementará essa interface. Isso evita acoplar negócio a banco de dados.
public interface IProdutoRepository
{
    Task AdicionarAsync(Produto produto);
    Task<Produto?> ObterPorIdAsync(Guid id);
    Task<IReadOnlyList<Produto>> ListarAsync();
    Task RemoverAsync(Guid id);
}
