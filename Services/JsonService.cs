using System.Text.Json;

namespace GameStore.Services;

public class JsonService
{
    private readonly JsonSerializerOptions options = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public void Salvar<T>(string caminho, T dados)
    {
        string? pasta = Path.GetDirectoryName(caminho);
        if (!string.IsNullOrEmpty(pasta)) Directory.CreateDirectory(pasta);
        string json = JsonSerializer.Serialize(dados, options);
        File.WriteAllText(caminho, json);
    }

    public T? Carregar<T>(string caminho)
    {
        if (!File.Exists(caminho)) return default;
        string json = File.ReadAllText(caminho);
        return JsonSerializer.Deserialize<T>(json, options);
    }
}
