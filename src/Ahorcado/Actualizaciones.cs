using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ahorcado;

internal static class Actualizaciones
{
    private const string Repositorio = "davidmotog/Minecraft-Ahorcado-Edition-";
    private static readonly HttpClient Http = CrearCliente();

    internal sealed record Disponible(string Version, Uri Pagina);
    private sealed record Publicacion(
        [property: JsonPropertyName("tag_name")] string Etiqueta,
        [property: JsonPropertyName("draft")] bool Borrador,
        [property: JsonPropertyName("prerelease")] bool Preliminar);

    private static HttpClient CrearCliente()
    {
        var cliente = new HttpClient(new HttpClientHandler { AllowAutoRedirect = false })
        {
            Timeout = TimeSpan.FromSeconds(10),
            MaxResponseContentBufferSize = 1024 * 1024
        };
        cliente.DefaultRequestHeaders.UserAgent.ParseAdd("Ahorcado-Windows/2");
        cliente.DefaultRequestHeaders.Accept.ParseAdd("application/vnd.github+json");
        return cliente;
    }

    public static async Task<Disponible?> Buscar(string actual)
    {
        string json = await Http.GetStringAsync($"https://api.github.com/repos/{Repositorio}/releases/latest")
            .ConfigureAwait(false);
        return Evaluar(actual, json);
    }

    internal static Disponible? Evaluar(string actual, string json)
    {
        var publicacion = JsonSerializer.Deserialize<Publicacion>(json);
        if (publicacion is null || publicacion.Borrador || publicacion.Preliminar) return null;
        if (!LeerVersion(actual, out var instalada) || !LeerVersion(publicacion.Etiqueta, out var publicada))
            throw new InvalidDataException("No se pudo interpretar la versión de Ahorcado.");
        if (publicada.CompareTo(instalada) <= 0) return null;

        return new Disponible(publicacion.Etiqueta.TrimStart('v'),
            new Uri($"https://github.com/{Repositorio}/releases/tag/{publicacion.Etiqueta}"));
    }

    private static bool LeerVersion(string? texto, out (Version Fecha, int Correccion) version)
    {
        version = default;
        var partes = texto?.TrimStart('v').Split('.');
        if (partes is not { Length: 4 or 5 } ||
            partes.Any(p => !int.TryParse(p, NumberStyles.None, CultureInfo.InvariantCulture, out _)) ||
            !Version.TryParse(string.Join('.', partes.Take(4)), out var fecha))
            return false;

        int correccion = partes.Length == 5 ? int.Parse(partes[4], CultureInfo.InvariantCulture) : 0;
        version = (fecha, correccion);
        return true;
    }
}
