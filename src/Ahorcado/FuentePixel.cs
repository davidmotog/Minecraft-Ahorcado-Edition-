using System.Drawing.Text;
using System.Runtime.InteropServices;

namespace Ahorcado;

internal sealed class FuentePixel : IDisposable
{
    private readonly PrivateFontCollection fuentes = new();
    private readonly nint memoria;

    public FuentePixel()
    {
        using Stream recurso = typeof(FuentePixel).Assembly.GetManifestResourceStream("Ahorcado.PixelifySans-Regular.ttf")
            ?? throw new InvalidOperationException("Falta la tipografía del juego.");
        using var contenido = new MemoryStream();
        recurso.CopyTo(contenido);
        byte[] bytes = contenido.ToArray();
        memoria = Marshal.AllocCoTaskMem(bytes.Length);
        try
        {
            Marshal.Copy(bytes, 0, memoria, bytes.Length);
            fuentes.AddMemoryFont(memoria, bytes.Length);
        }
        catch
        {
            Dispose();
            throw;
        }
    }

    public Font Crear(float tamaño)
    {
        using FontFamily familia = fuentes.Families.Single();
        return new Font(familia, tamaño);
    }

    public void Dispose()
    {
        fuentes.Dispose();
        Marshal.FreeCoTaskMem(memoria);
    }
}
