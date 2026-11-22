using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO.Compression;

namespace Ahorcado;

internal static class ImagenPremio
{
    private static readonly string Carpeta = Path.Combine(DatosLocales.Carpeta, "Imagenes");
    private const int MaxFoto = 20 * 1024 * 1024;

    private static string Ruta(EntradaCatalogo entrada) => Path.Combine(Carpeta, entrada.Id + ".png");

    public static (Bitmap Imagen, string Pie) Obtener(EntradaCatalogo entrada)
    {
        try
        {
            if (File.Exists(Ruta(entrada)))
            {
                using var archivo = File.OpenRead(Ruta(entrada));
                return (Leer(archivo), "Foto asociada en este equipo");
            }
        }
        catch (Exception error) when (error is IOException or UnauthorizedAccessException or InvalidDataException) { }

        if (entrada.Categoria is "Armadura" or "Herramienta" && TexturaLocal(entrada) is { } textura)
            return (textura, $"Textura de tu instalación de Minecraft Java {Catalogo.VersionMinecraft}");

        return (Ilustrar(entrada.Categoria), "Ilustración de la categoría");
    }

    public static Bitmap Asociar(EntradaCatalogo entrada, string ruta)
    {
        using var archivo = File.OpenRead(ruta);
        if (archivo.Length > MaxFoto) throw new InvalidDataException("La foto supera los 20 MB.");
        using var imagen = Leer(archivo);
        float escala = Math.Min(1, 1200f / Math.Max(imagen.Width, imagen.Height));
        var copia = new Bitmap(Math.Max(1, (int)(imagen.Width * escala)), Math.Max(1, (int)(imagen.Height * escala)));
        try
        {
            using (Graphics g = Graphics.FromImage(copia))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(imagen, 0, 0, copia.Width, copia.Height);
            }
            DatosLocales.Guardar(Ruta(entrada), destino => copia.Save(destino, ImageFormat.Png));
            return copia;
        }
        catch
        {
            copia.Dispose();
            throw;
        }
    }

    private static Bitmap Leer(Stream archivo)
    {
        try
        {
            using var imagen = Image.FromStream(archivo, useEmbeddedColorManagement: false, validateImageData: true);
            if (imagen.Width > 8192 || imagen.Height > 8192 || (long)imagen.Width * imagen.Height > 24_000_000)
                throw new InvalidDataException("Las dimensiones de la imagen son demasiado grandes.");
            return new Bitmap(imagen);
        }
        catch (ArgumentException error)
        {
            throw new InvalidDataException("Windows no pudo abrir esa imagen.", error);
        }
    }

    private static Bitmap? TexturaLocal(EntradaCatalogo entrada)
    {
        string version = Catalogo.VersionMinecraft;
        string ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            ".minecraft", "versions", version, version + ".jar");
        if (!File.Exists(ruta)) return null;

        try
        {
            using var jar = ZipFile.OpenRead(ruta);
            var textura = jar.GetEntry($"assets/minecraft/textures/item/{entrada.Id}.png") ??
                jar.GetEntry($"assets/minecraft/textures/item/{entrada.Id}_inventory.png") ??
                jar.GetEntry($"assets/minecraft/textures/item/{entrada.Id}_standby.png");
            if (textura is null || textura.Length > 128 * 1024) return null;
            using Stream png = textura.Open();
            using var datos = new MemoryStream();
            png.CopyTo(datos);
            datos.Position = 0;
            using var pequeña = Leer(datos);
            var resultado = new Bitmap(256, 256);
            using var g = Graphics.FromImage(resultado);
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = PixelOffsetMode.Half;
            g.DrawImage(pequeña, new Rectangle(8, 8, 240, 240), 0, 0, pequeña.Width, pequeña.Height, GraphicsUnit.Pixel);
            return resultado;
        }
        catch (Exception error) when (error is IOException or UnauthorizedAccessException or InvalidDataException)
        {
            return null;
        }
    }

    private static Bitmap Ilustrar(string categoria)
    {
        string[] píxeles = categoria switch
        {
            "Criatura" => [
                ".....##........##.....", "....#++#......#++#....", "....#++########++#....", "....#++++++++++++#....",
                "....#++..++++..++#....", "....#++..++++..++#....", "....#++++....++++#....", ".....###++++++###.....",
                ".....#++++++++++#.....", ".....#++++++++++#.....", ".....##..####..##.....", "......................"],
            "Armadura" => [
                "....####......####....", "...#++++######++++#...", "..#++++++++++++++++#..", "..#++++++####++++++#..",
                "..#+++++#....#+++++#..", "...#####+....+#####...", "......#++++++++#......", "......#++....++#......",
                "......#++....++#......", "......#++++++++#......", ".......########.......", "......................"],
            "Herramienta" => [
                "............######....", "..........##++++++#...", ".........#+++++++++#..", "..........###++#+++#..",
                ".........##..##..#+#..", ".......##++#.....##...", ".....##+++#...........", "....#++++#............",
                "...#++++#.............", "...#++##..............", "....##................", "......................"],
            _ => [
                ".........####.........", ".......##++++##.......", ".....##++++++++##.....", "...##++++++++++++##...",
                "...################...", "....#++++++++++++#....", "....#++..++++..++#....", "....#++..++++..++#....",
                "....#+++++..+++++#....", "....#+++++..+++++#....", "....##############....", "......................"]
        };
        Color claro = categoria switch
        {
            "Criatura" => Color.FromArgb(111, 216, 201),
            "Armadura" => Color.FromArgb(180, 159, 241),
            "Herramienta" => Color.FromArgb(238, 193, 115),
            _ => Color.FromArgb(223, 145, 112)
        };
        var imagen = new Bitmap(396, 234);
        using var g = Graphics.FromImage(imagen);
        using var luz = new SolidBrush(claro);
        using var borde = new SolidBrush(Color.FromArgb(claro.R / 2, claro.G / 2, claro.B / 2));
        for (int fila = 0; fila < píxeles.Length; fila++)
        for (int columna = 0; columna < píxeles[fila].Length; columna++)
        {
            char píxel = píxeles[fila][columna];
            if (píxel != '.') g.FillRectangle(píxel == '+' ? luz : borde, columna * 18, 9 + fila * 18, 18, 18);
        }
        return imagen;
    }
}
