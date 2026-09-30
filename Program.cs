using GameStore.Classes;
using GameStore.Interfaces;
using GameStore.Services;
using System.Globalization;

namespace GameStore;

class Program
{
    static List<Produto> produtos = new();
    static List<Cliente> clientes = new();
    static List<Pedido> pedidos = new();
    static readonly JsonService jsonService = new();
    static readonly INotificacao notificacao = new NotificacaoConsole();

    static void Main()
    {
        CarregarDados();
        int opcao;

        do
        {
            Console.Clear();
            Console.WriteLine("====================================");
            Console.WriteLine("          GAMESTORE");
            Console.WriteLine("   Sistema de Gestão de Loja");
            Console.WriteLine("====================================");
            Console.WriteLine("1 - Cadastrar cliente");
            Console.WriteLine("2 - Cadastrar produto");
            Console.WriteLine("3 - Listar clientes");
            Console.WriteLine("4 - Listar produtos");
            Console.WriteLine("5 - Realizar pedido");
            Console.WriteLine("6 - Listar pedidos");
            Console.WriteLine("7 - Consultar cliente");
            Console.WriteLine("8 - Consultar produto");
            Console.WriteLine("9 - Controle de estoque mínimo");
            Console.WriteLine("10 - Salvar dados");
            Console.WriteLine("11 - Carregar dados");
            Console.WriteLine("12 - Relatório de vendas");
            Console.WriteLine("0 - Sair");
            Console.Write("\nDigite uma opção: ");

            if (!int.TryParse(Console.ReadLine(), out opcao))
            {
                Console.WriteLine("Opção inválida.");
                Pausar();
                continue;
            }

            try
            {
                switch (opcao)
                {
                    case 1: CadastrarCliente(); break;
                    case 2: CadastrarProduto(); break;
                    case 3: ListarClientes(); break;
                    case 4: ListarProdutos(); break;
                    case 5: RealizarPedido(); break;
                    case 6: ListarPedidos(); break;
                    case 7: ConsultarCliente(); break;
                    case 8: ConsultarProduto(); break;
                    case 9: EstoqueMinimo(); break;
                    case 10: SalvarDados(); break;
                    case 11: CarregarDados(); break;
                    case 12: RelatorioVendas(); break;
                    case 0: SalvarDados(); Console.WriteLine("Sistema encerrado."); break;
                    default: Console.WriteLine("Opção inexistente."); Pausar(); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro: {ex.Message}");
                Pausar();
            }
        } while (opcao != 0);
    }

    static void CadastrarCliente()
    {
        Console.Clear();
        Console.WriteLine("=== CADASTRO DE CLIENTE ===");
        Console.Write("Nome: ");
        string nome = Console.ReadLine() ?? "";
        Console.Write("CPF: ");
        string cpf = Console.ReadLine() ?? "";
        if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(cpf))
            throw new ArgumentException("Nome e CPF são obrigatórios.");

        clientes.Add(new Cliente(nome, cpf));
        Console.WriteLine("Cliente cadastrado com sucesso.");
        Pausar();
    }

    static void CadastrarProduto()
    {
        Console.Clear();
        Console.WriteLine("=== CADASTRO DE PRODUTO ===");
        Console.Write("Nome: ");
        string nome = Console.ReadLine() ?? "";
        Console.Write("Preço: ");
        decimal preco = LerDecimal();
        Console.Write("Estoque: ");
        int estoque = LerInteiro();

        if (preco <= 0 || estoque < 0)
            throw new ArgumentException("Preço deve ser maior que zero e estoque não pode ser negativo.");

        Console.WriteLine("Tipo: 1-Jogo | 2-Console");
        int tipo = LerInteiro();

        Produto produto;
        if (tipo == 1)
        {
            Console.Write("Plataforma: ");
            string plataforma = Console.ReadLine() ?? "";
            produto = new Jogo(nome, preco, estoque, plataforma);
        }
        else if (tipo == 2)
        {
            Console.Write("Fabricante: ");
            string fabricante = Console.ReadLine() ?? "";
            produto = new ConsoleGame(nome, preco, estoque, fabricante);
        }
        else
        {
            throw new ArgumentException("Tipo de produto inválido.");
        }

        produtos.Add(produto);
        Console.WriteLine("Produto cadastrado com sucesso.");
        Pausar();
    }

    static void ListarClientes()
    {
        Console.Clear();
        Console.WriteLine("=== CLIENTES ===");
        if (!clientes.Any()) Console.WriteLine("Nenhum cliente cadastrado.");
        foreach (var c in clientes) Console.WriteLine(c);
        Pausar();
    }

    static void ListarProdutos()
    {
        Console.Clear();
        Console.WriteLine("=== PRODUTOS ===");
        if (!produtos.Any()) Console.WriteLine("Nenhum produto cadastrado.");
        foreach (var p in produtos)
            Console.WriteLine($"{p.Id} - {p.Nome} | {p.GetDescricao()} | R$ {p.Preco:F2} | Estoque: {p.Estoque}");
        Pausar();
    }

    static void RealizarPedido()
    {
        Console.Clear();
        Console.WriteLine("=== NOVO PEDIDO ===");
        if (!clientes.Any() || !produtos.Any())
            throw new InvalidOperationException("Cadastre pelo menos um cliente e um produto antes de realizar um pedido.");

        ListarClientesSemPausa();
        Console.Write("ID do cliente: ");
        int clienteId = LerInteiro();
        Cliente cliente = clientes.FirstOrDefault(c => c.Id == clienteId)
            ?? throw new ArgumentException("Cliente não encontrado.");

        var pedido = new Pedido(cliente);

        while (true)
        {
            ListarProdutosSemPausa();
            Console.Write("ID do produto (0 para finalizar): ");
            int produtoId = LerInteiro();
            if (produtoId == 0) break;

            Produto produto = produtos.FirstOrDefault(p => p.Id == produtoId)
                ?? throw new ArgumentException("Produto não encontrado.");

            Console.Write("Quantidade: ");
            int quantidade = LerInteiro();
            if (quantidade <= 0 || quantidade > produto.Estoque)
                throw new ArgumentException("Quantidade inválida ou superior ao estoque.");

            pedido.AdicionarItem(produto, quantidade);
            Console.WriteLine("Item adicionado.");
        }

        if (!pedido.Itens.Any())
            throw new InvalidOperationException("O pedido deve possuir pelo menos um item.");

        pedidos.Add(pedido);
        notificacao.Enviar($"Pedido #{pedido.Id} criado. Total: R$ {pedido.CalcularTotal():F2}");
        Pausar();
    }

    static void ListarPedidos()
    {
        Console.Clear();
        Console.WriteLine("=== PEDIDOS ===");
        if (!pedidos.Any()) Console.WriteLine("Nenhum pedido cadastrado.");
        foreach (var p in pedidos)
        {
            Console.WriteLine($"Pedido #{p.Id} | Cliente: {p.Cliente.Nome} | Status: {p.Status} | Total: R$ {p.CalcularTotal():F2}");
            foreach (var item in p.Itens)
                Console.WriteLine($"  - {item.Produto.Nome} x{item.Quantidade} = R$ {item.Subtotal():F2}");
        }
        Pausar();
    }

    static void ConsultarCliente()
    {
        Console.Clear();
        Console.Write("Digite o nome do cliente: ");
        string busca = Console.ReadLine() ?? "";
        var encontrados = clientes.Where(c => c.Nome.Contains(busca, StringComparison.OrdinalIgnoreCase)).ToList();
        if (!encontrados.Any()) Console.WriteLine("Cliente não encontrado.");
        foreach (var c in encontrados) Console.WriteLine(c);
        Pausar();
    }

    static void ConsultarProduto()
    {
        Console.Clear();
        Console.Write("Digite o nome do produto: ");
        string busca = Console.ReadLine() ?? "";
        var encontrados = produtos.Where(p => p.Nome.Contains(busca, StringComparison.OrdinalIgnoreCase)).ToList();
        if (!encontrados.Any()) Console.WriteLine("Produto não encontrado.");
        foreach (var p in encontrados)
            Console.WriteLine($"{p.Id} - {p.Nome} | {p.GetDescricao()} | R$ {p.Preco:F2} | Estoque: {p.Estoque}");
        Pausar();
    }

    static void EstoqueMinimo()
    {
        Console.Clear();
        Console.WriteLine("=== ESTOQUE MÍNIMO ===");
        var baixos = produtos.Where(p => p.Estoque <= 3).ToList();
        if (!baixos.Any()) Console.WriteLine("Nenhum produto está no estoque mínimo.");
        foreach (var p in baixos)
            Console.WriteLine($"ATENÇÃO: {p.Nome} possui apenas {p.Estoque} unidade(s).");
        Pausar();
    }

    static void RelatorioVendas()
    {
        Console.Clear();
        Console.WriteLine("=== RELATÓRIO DE VENDAS ===");
        decimal total = pedidos.Sum(p => p.CalcularTotal());
        int quantidade = pedidos.Sum(p => p.Itens.Sum(i => i.Quantidade));
        Console.WriteLine($"Pedidos realizados: {pedidos.Count}");
        Console.WriteLine($"Itens vendidos: {quantidade}");
        Console.WriteLine($"Faturamento: R$ {total:F2}");
        Pausar();
    }

    static void SalvarDados()
    {
        jsonService.Salvar("Dados/clientes.json", clientes);
        jsonService.Salvar("Dados/produtos.json", produtos);
        jsonService.Salvar("Dados/pedidos.json", pedidos);
        Console.WriteLine("Dados salvos em JSON.");
        Pausar();
    }

    static void CarregarDados()
    {
        clientes = jsonService.Carregar<List<Cliente>>("Dados/clientes.json") ?? new();
        produtos = jsonService.Carregar<List<Produto>>("Dados/produtos.json") ?? new();
        pedidos = jsonService.Carregar<List<Pedido>>("Dados/pedidos.json") ?? new();
    }

    static int LerInteiro()
    {
        if (!int.TryParse(Console.ReadLine(), out int valor))
            throw new ArgumentException("Digite um número inteiro válido.");
        return valor;
    }

    static decimal LerDecimal()
    {
        string entrada = Console.ReadLine() ?? "";
        if (!decimal.TryParse(entrada.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal valor))
            throw new ArgumentException("Digite um valor decimal válido.");
        return valor;
    }

    static void ListarClientesSemPausa()
    {
        foreach (var c in clientes) Console.WriteLine($"{c.Id} - {c.Nome}");
    }

    static void ListarProdutosSemPausa()
    {
        foreach (var p in produtos) Console.WriteLine($"{p.Id} - {p.Nome} | R$ {p.Preco:F2} | Estoque: {p.Estoque}");
    }

    static void Pausar()
    {
        Console.WriteLine("\nPressione ENTER para continuar...");
        Console.ReadLine();
    }
}
