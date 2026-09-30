using GameStore.Interfaces;

namespace GameStore.Services;

public class NotificacaoConsole : INotificacao
{
    public void Enviar(string mensagem)
    {
        Console.WriteLine($"[NOTIFICAÇÃO] {mensagem}");
    }
}
