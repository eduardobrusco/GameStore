namespace GameStore.Classes;

public abstract class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public int Estoque { get; set; }

    protected Produto(string nome, decimal preco, int estoque)
    {
        Id = IdGenerator.Proximo();
        Nome = nome;
        Preco = preco;
        Estoque = estoque;
    }

    public virtual string GetDescricao() => "Produto";
}
