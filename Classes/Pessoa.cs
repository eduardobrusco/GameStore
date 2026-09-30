namespace GameStore.Classes;

public abstract class Pessoa
{
    public int Id { get; set; }
    public string Nome { get; set; }

    protected Pessoa(string nome)
    {
        Id = IdGenerator.Proximo();
        Nome = nome;
    }

    public virtual string Identificacao()
    {
        return $"ID: {Id} | Nome: {Nome}";
    }
}
