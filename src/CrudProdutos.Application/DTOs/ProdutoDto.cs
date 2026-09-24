namespace CrudProdutos.Application.DTOs;

// DTO: dados que saem da Application e são enviados ao cliente da API.
public sealed record ProdutoDto(Guid Id, string Nome, decimal Preco);
