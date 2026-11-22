namespace Ahorcado;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        using var fuente = new FuentePixel();
        using var ventana = new VentanaJuego(fuente);
        Application.Run(ventana);
    }
}
