namespace Ahorcado;

internal static class Criaturas
{
    private static readonly string[] palabras =
    [
        "ajolote", "murciélago", "abeja", "blaze", "gato",
        "araña de cueva", "pollo", "bacalao", "vaca", "creeper",
        "delfin", "burro", "ahogado", "guardián anciano", "enderman",
        "endermite", "invocador", "zorro", "ghast", "calamar luminoso",
        "cabra", "guardián", "hoglin", "caballo", "zombi momificado",
        "llama", "cubo de magma", "champiñaca", "mula", "ocelote",
        "panda", "loro", "fantasma", "cerdo", "piglin",
        "piglin bruto", "saqueador", "oso polar", "pez globo", "conejo",
        "devastador", "salmón", "oveja", "shulker", "lepisma",
        "esqueleto", "caballo esqueleto", "slime", "araña", "calamar",
        "esqueleto glacial", "lavagante", "llama ambulante", "pez tropical", "tortuga",
        "ánima", "aldeano", "vindicador", "vendedor ambulante", "bruja",
        "esqueleto wither", "lobo", "zoglin", "zombi", "caballo zombi",
        "aldeano zombi", "piglin zombificado",
    ];

    public static string Elegir(string? anterior)
    {
        int indiceAnterior = Array.IndexOf(palabras, anterior);
        if (indiceAnterior < 0) return palabras[Random.Shared.Next(palabras.Length)];

        int indice = Random.Shared.Next(palabras.Length - 1);
        return palabras[indice < indiceAnterior ? indice : indice + 1];
    }
}
