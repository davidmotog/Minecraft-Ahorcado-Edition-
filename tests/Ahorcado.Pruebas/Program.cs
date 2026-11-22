using Ahorcado;

static void Verificar(bool resultado, string caso)
{
    if (!resultado) throw new InvalidOperationException(caso);
}

var frase = new Partida("guardián anciano");
Verificar(frase.Mascara.Contains("   "), "La máscara deja visible la separación entre palabras.");
Verificar(frase.Probar("  GUARDIAN     ANCIANO ") == ResultadoJugada.Acierto &&
          frase.Estado == EstadoPartida.Ganada && frase.Mascara == "guardián anciano",
          "El nombre completo admite espacios, mayúsculas y tildes.");

var letras = new Partida("araña de cueva");
Verificar(letras.Probar("n") == ResultadoJugada.Fallo && letras.Fallos == 1, "La N es distinta de la Ñ.");
Verificar(letras.Probar("a") == ResultadoJugada.Acierto && letras.Mascara.Count(c => c == 'A') == 4,
          "Una letra descubre todas sus apariciones.");
Verificar(letras.Probar(" A ") == ResultadoJugada.Repetida && letras.TotalIntentos == 2,
          "Una jugada repetida conserva el recuento.");
Verificar(letras.Probar("[") == ResultadoJugada.Invalida && letras.Fallos == 1 && letras.TotalIntentos == 2,
          "Un carácter inválido no consume una jugada.");
foreach (char letra in "rñdecuv") letras.Probar(letra.ToString());
Verificar(letras.Estado == EstadoPartida.Ganada, "Se puede completar una frase letra a letra.");

var perdida = new Partida("zombi");
foreach (char letra in "acdefg") perdida.Probar(letra.ToString());
Verificar(perdida.Estado == EstadoPartida.Perdida && perdida.Fallos == Partida.MaxFallos, "El sexto fallo acaba la partida.");
Verificar(perdida.Probar("zombi") == ResultadoJugada.Invalida && perdida.Fallos == Partida.MaxFallos,
          "Una partida terminada no recibe más jugadas.");

var revelada = new Partida("blaze");
revelada.Revelar();
Verificar(revelada.Estado == EstadoPartida.Revelada && revelada.Mascara == "blaze" && revelada.Fallos == 0,
          "Mostrar la solución no gasta fallos.");

Verificar(Catalogo.Entradas.Count == 250 && Catalogo.Entradas.Select(e => e.Id).Distinct().Count() == 250,
    "El catálogo tiene 250 identificadores distintos.");
foreach (var item in Catalogo.Entradas)
{
    var juego = new Partida(item.Nombre, item.Alias);
    Verificar(juego.Probar(item.Nombre) == ResultadoJugada.Acierto && juego.Estado == EstadoPartida.Ganada,
        $"El nombre del catálogo se puede acertar: {item.Nombre}");
    Verificar(item.Enlace.Host == "minecraft.wiki" && item.Enlace.AbsolutePath.StartsWith("/w/"),
        $"La ficha del catálogo apunta a la wiki: {item.Nombre}");
}

EntradaCatalogo? anterior = null;
foreach (string categoria in new[] { "Todas", "Criatura", "Estructura", "Armadura", "Herramienta" })
for (int i = 0; i < 32; i++)
{
    var siguiente = Catalogo.Elegir(categoria, anterior);
    Verificar(siguiente != anterior && (categoria == "Todas" || siguiente.Categoria == categoria),
        $"La selección respeta la categoría {categoria} y evita repetir.");
    anterior = siguiente;
}

var allay = Catalogo.Entradas.Single(e => e.Id == "allay");
var alias = new Partida(allay.Nombre, allay.Alias);
Verificar(alias.Probar("anima") == ResultadoJugada.Acierto && alias.Estado == EstadoPartida.Ganada,
    "Un alias admite tildes omitidas.");

static string Publicacion(string etiqueta, bool preliminar = false) =>
    System.Text.Json.JsonSerializer.Serialize(new { tag_name = etiqueta, prerelease = preliminar });

const string actual = "2.2026.11.22.2";
Verificar(Actualizaciones.Evaluar(actual, Publicacion("v2.2026.11.22")) is null, "La versión del mismo día sin corrección es anterior.");
Verificar(Actualizaciones.Evaluar(actual, Publicacion("v2.2026.11.22.2")) is null, "La misma versión está al día.");
Verificar(Actualizaciones.Evaluar(actual, Publicacion("v2.2026.11.22.10")) is { Version: "2.2026.11.22.10" },
    "El número de corrección se compara como entero.");
Verificar(Actualizaciones.Evaluar(actual, Publicacion("v2.2026.11.23"))?.Pagina.AbsolutePath.EndsWith("/v2.2026.11.23") == true,
    "Una fecha posterior ofrece su página de descarga.");
Verificar(Actualizaciones.Evaluar(actual, Publicacion("alfa2.0", preliminar: true)) is null,
    "Una publicación histórica no se ofrece como actualización.");

Console.WriteLine("Pruebas del juego y de versiones correctas.");
