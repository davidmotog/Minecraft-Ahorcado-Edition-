using System.Buffers.Binary;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Ahorcado;

internal sealed class SkinJugador : IDisposable
{
    private const int LimitePng = 128 * 1024;
    private static readonly string RutaSkin = Path.Combine(DatosLocales.Carpeta, "skin.json");
    private static readonly HttpClient Http = new(new HttpClientHandler { AllowAutoRedirect = false })
    {
        Timeout = TimeSpan.FromSeconds(15),
        MaxResponseContentBufferSize = LimitePng
    };

    private sealed record Preferencia(string Nombre, bool Delgado, byte[] Png);

    private readonly Preferencia preferencia;
    public string Nombre => preferencia.Nombre;
    public bool Delgado => preferencia.Delgado;
    public bool Legado => Textura.Height == 32;
    public Bitmap Textura { get; }

    private SkinJugador(string nombre, bool delgado, byte[] png)
    {
        if (png.Length is < 33 or > LimitePng ||
            !png.AsSpan(0, 8).SequenceEqual(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 }) ||
            !png.AsSpan(12, 4).SequenceEqual("IHDR"u8) ||
            BinaryPrimitives.ReadInt32BigEndian(png.AsSpan(16)) != 64 ||
            BinaryPrimitives.ReadInt32BigEndian(png.AsSpan(20)) is not (32 or 64))
            throw new InvalidDataException("La skin debe ser un PNG de 64 × 64 o 64 × 32 píxeles.");

        try
        {
            using var datos = new MemoryStream(png);
            using var imagen = Image.FromStream(datos, useEmbeddedColorManagement: false, validateImageData: true);
            Textura = new Bitmap(imagen);
        }
        catch (ArgumentException error)
        {
            throw new InvalidDataException("El PNG de la skin está dañado.", error);
        }

        preferencia = new Preferencia(nombre, delgado && !Legado, png);
    }

    public static SkinJugador DesdeArchivo(string ruta, bool delgado)
    {
        using var archivo = File.OpenRead(ruta);
        if (archivo.Length > LimitePng) throw new InvalidDataException("El PNG supera los 128 KB.");
        var png = new byte[(int)archivo.Length];
        archivo.ReadExactly(png);
        return new SkinJugador(Path.GetFileName(ruta), delgado, png);
    }

    public static async Task<SkinJugador> Buscar(string nombre, CancellationToken cancelacion)
    {
        if (!Regex.IsMatch(nombre, @"\A[A-Za-z0-9_]{3,16}\z"))
            throw new InvalidDataException("Escribe un nombre de Minecraft Java de 3 a 16 caracteres.");

        using var jugador = await LeerJson($"https://api.minecraftservices.com/minecraft/profile/lookup/name/{nombre}",
            "Mojang no encuentra ese nombre de jugador.", cancelacion).ConfigureAwait(false);
        if (!jugador.RootElement.TryGetProperty("id", out var id) || !Guid.TryParseExact(id.GetString(), "N", out var uuid))
            throw new InvalidDataException("Mojang devolvió un perfil sin identificador.");
        string nombreConfirmado = jugador.RootElement.GetProperty("name").GetString() ?? nombre;

        using var perfil = await LeerJson($"https://sessionserver.mojang.com/session/minecraft/profile/{uuid:N}",
            "El jugador no tiene un perfil público con skin.", cancelacion).ConfigureAwait(false);
        foreach (var propiedad in perfil.RootElement.GetProperty("properties").EnumerateArray())
        {
            if (propiedad.GetProperty("name").GetString() != "textures") continue;
            using var texturas = JsonDocument.Parse(Convert.FromBase64String(propiedad.GetProperty("value").GetString()!));
            if (!texturas.RootElement.GetProperty("textures").TryGetProperty("SKIN", out var skin)) break;

            string? url = skin.GetProperty("url").GetString();
            if (!Uri.TryCreate(url, UriKind.Absolute, out var origen) ||
                origen.Host != "textures.minecraft.net" || !origen.IsDefaultPort ||
                origen.Scheme is not ("http" or "https") || origen.UserInfo.Length != 0 ||
                origen.Query.Length != 0 || origen.Fragment.Length != 0 ||
                !Regex.IsMatch(origen.AbsolutePath, @"\A/texture/[a-fA-F0-9]{32,64}\z"))
                throw new InvalidDataException("La dirección de la textura de Mojang no es válida.");

            bool delgado = skin.TryGetProperty("metadata", out var meta) &&
                meta.TryGetProperty("model", out var modelo) && modelo.GetString() == "slim";
            using var respuesta = await Http.GetAsync("https://textures.minecraft.net" + origen.AbsolutePath, cancelacion)
                .ConfigureAwait(false);
            Comprobar(respuesta, "La textura del perfil ya no está disponible.");
            byte[] png = await respuesta.Content.ReadAsByteArrayAsync(cancelacion).ConfigureAwait(false);
            return new SkinJugador(nombreConfirmado, delgado, png);
        }
        throw new InvalidDataException("El jugador usa una skin predeterminada. Puedes elegir un PNG.");
    }

    private static async Task<JsonDocument> LeerJson(string url, string ausente, CancellationToken cancelacion)
    {
        using var respuesta = await Http.GetAsync(url, cancelacion).ConfigureAwait(false);
        Comprobar(respuesta, ausente);
        using Stream contenido = await respuesta.Content.ReadAsStreamAsync(cancelacion).ConfigureAwait(false);
        return await JsonDocument.ParseAsync(contenido, cancellationToken: cancelacion).ConfigureAwait(false);
    }

    private static void Comprobar(HttpResponseMessage respuesta, string ausente)
    {
        if (respuesta.StatusCode is HttpStatusCode.NoContent or HttpStatusCode.NotFound)
            throw new InvalidDataException(ausente);
        if (respuesta.StatusCode == HttpStatusCode.TooManyRequests)
            throw new InvalidDataException("Mojang ha limitado las consultas. Espera un momento o usa un PNG.");
        respuesta.EnsureSuccessStatusCode();
    }

    public static SkinJugador? LeerPreferencia()
    {
        try
        {
            using var archivo = File.OpenRead(RutaSkin);
            if (archivo.Length > LimitePng * 2) return null;
            var guardada = JsonSerializer.Deserialize<Preferencia>(archivo);
            return guardada is { Png: not null, Nombre: { Length: > 0 } }
                ? new SkinJugador(guardada.Nombre, guardada.Delgado, guardada.Png)
                : null;
        }
        catch (Exception error) when (error is IOException or UnauthorizedAccessException or JsonException or InvalidDataException)
        {
            return null;
        }
    }

    public static void GuardarPreferencia(SkinJugador? skin)
    {
        if (skin is null)
        {
            if (File.Exists(RutaSkin)) File.Delete(RutaSkin);
            return;
        }
        DatosLocales.Guardar(RutaSkin, archivo => JsonSerializer.Serialize(archivo, skin.preferencia));
    }

    public void Dispose() => Textura.Dispose();
}
