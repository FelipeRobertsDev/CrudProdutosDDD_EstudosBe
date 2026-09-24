namespace CrudProdutos.Domain.Entities;

// CAMADA DOMAIN: o Produto é a entidade de negócio. Ela não conhece HTTP, banco nem controller.
public sealed class Produto
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public decimal Preco { get; private set; }

    public Produto(string nome, decimal preco)
    {
        Id = Guid.NewGuid();
        Nome = string.Empty;
        // Regra de negócio centralizada: produtos não podem ter nome vazio nem preço negativo.
        Atualizar(nome, preco);
    }

    public void Atualizar(string nome, decimal preco)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome é obrigatório.");
        if (preco < 0)
            throw new ArgumentException("O preço não pode ser negativo.");
        Nome = nome.Trim();
        Preco = preco;
    }
}
