namespace CrudProdutos.Application.DTOs;

// DTO de entrada: o usuário informa somente os campos que pode alterar.
public sealed record SalvarProdutoRequest(string Nome, decimal Preco);
