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

string palabra = Criaturas.Elegir(null);
for (int i = 0; i < 150; i++)
{
    string siguiente = Criaturas.Elegir(palabra);
    Verificar(siguiente != palabra, "Las partidas consecutivas usan criaturas diferentes.");
    _ = new Partida(siguiente);
    palabra = siguiente;
}

Console.WriteLine("Pruebas del juego correctas.");
