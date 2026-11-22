namespace Ahorcado;

internal static class DatosLocales
{
    public static string Carpeta { get; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Ahorcado");

    public static void Guardar(string ruta, Action<Stream> escribir)
    {
        string directorio = Path.GetDirectoryName(ruta)!;
        Directory.CreateDirectory(directorio);
        string temporal = Path.Combine(directorio, Guid.NewGuid().ToString("N") + ".tmp");
        try
        {
            using (var archivo = File.Create(temporal)) escribir(archivo);
            File.Move(temporal, ruta, overwrite: true);
        }
        finally
        {
            File.Delete(temporal);
        }
    }
}
