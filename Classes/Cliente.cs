namespace GameStore.Classes;

public class Cliente : Pessoa
{
    public string CPF { get; set; }

    public Cliente(string nome, string cpf) : base(nome)
    {
        CPF = cpf;
    }

    public override string Identificacao()
    {
        return $"{base.Identificacao()} | CPF: {CPF}";
    }

    public override string ToString() => Identificacao();
}
