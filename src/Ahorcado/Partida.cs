using System.Text;

namespace Ahorcado;

internal enum EstadoPartida { EnCurso, Ganada, Perdida, Revelada }
internal enum ResultadoJugada { Invalida, Repetida, Acierto, Fallo }

internal sealed class Partida
{
    public const int MaxFallos = 6;

    private readonly string solucionNormalizada;
    private readonly HashSet<string> soluciones;
    private readonly HashSet<string> intentos = new(StringComparer.Ordinal);
    private readonly HashSet<char> letras = [];
    private readonly List<char> ordenLetras = [];

    public string Solucion { get; }
    public int TotalIntentos => intentos.Count;
    public int Fallos { get; private set; }
    public EstadoPartida Estado { get; private set; } = EstadoPartida.EnCurso;
    public string LetrasUsadas => string.Join(" ", ordenLetras);

    public Partida(string solucion, IEnumerable<string>? alias = null)
    {
        Solucion = solucion;
        solucionNormalizada = Normalizar(solucion);
        if (!EsValido(solucionNormalizada))
            throw new ArgumentException("La solución debe contener letras.", nameof(solucion));
        soluciones = new(StringComparer.Ordinal) { solucionNormalizada };
        if (alias is not null)
            soluciones.UnionWith(alias.Select(Normalizar).Where(EsValido));
    }

    public string Mascara
    {
        get
        {
            if (Estado != EstadoPartida.EnCurso) return Solucion;
            var texto = new StringBuilder(solucionNormalizada.Length * 2);
            foreach (char letra in solucionNormalizada)
            {
                if (letra == ' ')
                    texto.Append("  ");
                else
                    texto.Append(letras.Contains(letra) ? letra : '_').Append(' ');
            }
            return texto.ToString().TrimEnd();
        }
    }

    public ResultadoJugada Probar(string entrada)
    {
        if (Estado != EstadoPartida.EnCurso) return ResultadoJugada.Invalida;
        string jugada = Normalizar(entrada);
        if (!EsValido(jugada)) return ResultadoJugada.Invalida;
        if (!intentos.Add(jugada)) return ResultadoJugada.Repetida;

        if (jugada.Length == 1)
        {
            char letra = jugada[0];
            letras.Add(letra);
            ordenLetras.Add(letra);
            if (solucionNormalizada.Contains(letra))
            {
                if (solucionNormalizada.All(c => c == ' ' || letras.Contains(c)))
                    Estado = EstadoPartida.Ganada;
                return ResultadoJugada.Acierto;
            }
        }
        else if (soluciones.Contains(jugada))
        {
            Estado = EstadoPartida.Ganada;
            return ResultadoJugada.Acierto;
        }

        Fallos++;
        if (Fallos == MaxFallos) Estado = EstadoPartida.Perdida;
        return ResultadoJugada.Fallo;
    }

    public void Revelar()
    {
        if (Estado == EstadoPartida.EnCurso) Estado = EstadoPartida.Revelada;
    }

    private static string Normalizar(string texto) =>
        string.Join(" ", texto.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries))
            .Normalize(NormalizationForm.FormC).ToUpperInvariant()
            .Replace('Á', 'A').Replace('É', 'E').Replace('Í', 'I')
            .Replace('Ó', 'O').Replace('Ú', 'U').Replace('Ü', 'U');

    private static bool EsValido(string texto) =>
        texto.Length > 0 && texto.All(c => c is (>= 'A' and <= 'Z') or 'Ñ' or ' ');
}
