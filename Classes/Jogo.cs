namespace GameStore.Classes;

public class Jogo : Produto
{
    public string Plataforma { get; set; }

    public Jogo(string nome, decimal preco, int estoque, string plataforma)
        : base(nome, preco, estoque)
    {
        Plataforma = plataforma;
    }

    public override string GetDescricao() => $"Jogo - Plataforma: {Plataforma}";
}
