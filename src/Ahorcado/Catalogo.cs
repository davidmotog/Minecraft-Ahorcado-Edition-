using System.Text.Json;

namespace Ahorcado;

internal sealed record EntradaCatalogo(string Id, string Categoria, string Nombre, string Wiki, string[] Alias)
{
    public Uri Enlace => new("https://minecraft.wiki/w/" + Uri.EscapeDataString(Wiki.Replace(' ', '_')));
}

internal static class Catalogo
{
    public const string VersionMinecraft = "26.3";
    public static IReadOnlyList<EntradaCatalogo> Entradas { get; } = Leer();

    private static EntradaCatalogo[] Leer()
    {
        using Stream json = typeof(Catalogo).Assembly.GetManifestResourceStream("Ahorcado.catalogo.json")
            ?? throw new InvalidOperationException("Falta el catálogo del juego.");
        return JsonSerializer.Deserialize<EntradaCatalogo[]>(json, JsonSerializerOptions.Web)
            ?? throw new InvalidOperationException("El catálogo está vacío.");
    }

    public static EntradaCatalogo Elegir(string categoria, EntradaCatalogo? anterior)
    {
        var opciones = Entradas.Where(e => (categoria == "Todas" || e.Categoria == categoria) && e != anterior).ToArray();
        return opciones.Length > 0
            ? opciones[Random.Shared.Next(opciones.Length)]
            : throw new ArgumentException("La categoría no contiene respuestas.", nameof(categoria));
    }
}
