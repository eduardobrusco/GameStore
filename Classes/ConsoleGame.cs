namespace GameStore.Classes;

public class ConsoleGame : Produto
{
    public string Fabricante { get; set; }

    public ConsoleGame(string nome, decimal preco, int estoque, string fabricante)
        : base(nome, preco, estoque)
    {
        Fabricante = fabricante;
    }

    public override string GetDescricao() => $"Console - Fabricante: {Fabricante}";
}
