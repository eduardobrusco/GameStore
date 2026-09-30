namespace GameStore.Classes;

public class Pedido
{
    public int Id { get; set; }
    public Cliente Cliente { get; set; }
    public List<ItemPedido> Itens { get; set; } = new();
    public string Status { get; set; } = "Aberto";

    public Pedido(Cliente cliente)
    {
        Id = IdGenerator.Proximo();
        Cliente = cliente;
    }

    public void AdicionarItem(Produto produto, int quantidade)
    {
        if (quantidade <= 0) throw new ArgumentException("Quantidade deve ser maior que zero.");
        if (produto.Estoque < quantidade) throw new InvalidOperationException("Estoque insuficiente.");

        produto.Estoque -= quantidade;
        Itens.Add(new ItemPedido(produto, quantidade));
    }

    public decimal CalcularTotal() => Itens.Sum(i => i.Subtotal());
}
