namespace GameStore.Classes;

public class ItemPedido
{
    public Produto Produto { get; set; }
    public int Quantidade { get; set; }

    public ItemPedido(Produto produto, int quantidade)
    {
        Produto = produto;
        Quantidade = quantidade;
    }

    public decimal Subtotal() => Produto.Preco * Quantidade;
}
