namespace GameStore.Classes;

public static class IdGenerator
{
    private static int proximoId = 1;

    public static int Proximo()
    {
        return proximoId++;
    }
}
