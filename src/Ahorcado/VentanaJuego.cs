using System.Drawing;

namespace Ahorcado;

internal sealed class VentanaJuego : Form
{
    private static readonly Color Fondo = Color.FromArgb(15, 25, 37);
    private static readonly Color Panel = Color.FromArgb(20, 34, 47);
    private static readonly Color Texto = Color.FromArgb(231, 241, 245);
    private static readonly Color Secundario = Color.FromArgb(164, 184, 198);
    private static readonly Color Acento = Color.FromArgb(111, 216, 201);

    private readonly DibujoAhorcado dibujo = new() { Dock = DockStyle.Fill, Margin = new Padding(0) };
    private readonly Label mascara = new() { Dock = DockStyle.Fill, AutoSize = false, TextAlign = ContentAlignment.MiddleLeft, AccessibleName = "Palabra" };
    private readonly Label intentos = Etiqueta();
    private readonly Label letras = Etiqueta();
    private readonly Label estado = Etiqueta();
    private readonly TextBox entrada = new() { Dock = DockStyle.Fill, MaxLength = 80, AccessibleName = "Letra o nombre de la criatura" };
    private readonly Button enviar = Boton("Probar", principal: true);
    private readonly Button nueva = Boton("Comenzar", principal: true);
    private readonly Button revelar = Boton("Mostrar solución");
    private Partida? partida;

    public VentanaJuego()
    {
        Text = "Ahorcado · Edición Minecraft";
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(880, 560);
        MinimumSize = new Size(780, 540);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Fondo;
        ForeColor = Texto;
        Font = new Font("Segoe UI", 11);

        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4, Padding = new Padding(24) };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        Controls.Add(layout);

        var titulo = Etiqueta("Ahorcado");
        titulo.Font = new Font(Font.FontFamily, 26, FontStyle.Bold);
        titulo.Margin = new Padding(0);
        layout.Controls.Add(titulo, 0, 0);
        var subtitulo = Etiqueta("Edición Minecraft");
        subtitulo.ForeColor = Secundario;
        subtitulo.Margin = new Padding(2, 0, 0, 15);
        layout.Controls.Add(subtitulo, 0, 1);

        var zona = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1, Margin = new Padding(0) };
        zona.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34));
        zona.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 66));
        zona.Controls.Add(dibujo, 0, 0);
        zona.Controls.Add(CrearPanel(), 1, 0);
        layout.Controls.Add(zona, 0, 2);

        var aviso = Etiqueta("Proyecto de fans independiente de Mojang y Microsoft.");
        aviso.ForeColor = Secundario;
        aviso.Font = new Font(Font.FontFamily, 9);
        aviso.Margin = new Padding(0, 14, 0, 0);
        layout.Controls.Add(aviso, 0, 3);

        nueva.Click += (_, _) => Comenzar();
        enviar.Click += (_, _) => Probar();
        revelar.Click += (_, _) => MostrarSolucion();
        MostrarEstado("Pulsa Comenzar para iniciar una partida.");
    }

    private Control CrearPanel()
    {
        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 7, Padding = new Padding(18), Margin = new Padding(12, 0, 0, 0),
            BackColor = Panel
        };
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        for (int fila = 3; fila < panel.RowCount; fila++) panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        intentos.ForeColor = Acento;
        letras.ForeColor = Secundario;
        mascara.Font = new Font("Consolas", 15, FontStyle.Bold);
        mascara.ForeColor = Texto;
        mascara.Margin = new Padding(3, 4, 3, 4);
        panel.Controls.Add(intentos, 0, 0);
        panel.Controls.Add(letras, 0, 1);
        panel.Controls.Add(mascara, 0, 2);
        var etiquetaEntrada = Etiqueta("Letra o nombre de la criatura");
        etiquetaEntrada.ForeColor = Secundario;
        panel.Controls.Add(etiquetaEntrada, 0, 3);
        var filaEntrada = new TableLayoutPanel { Dock = DockStyle.Top, ColumnCount = 2, AutoSize = true, Margin = new Padding(0) };
        filaEntrada.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        filaEntrada.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        entrada.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        filaEntrada.Controls.Add(entrada, 0, 0);
        filaEntrada.Controls.Add(enviar, 1, 0);
        panel.Controls.Add(filaEntrada, 0, 4);
        estado.MinimumSize = new Size(0, 32);
        estado.Margin = new Padding(3, 8, 3, 3);
        panel.Controls.Add(estado, 0, 5);

        var ayuda = Boton("Ayuda");
        ayuda.Click += (_, _) => MessageBox.Show(this,
            "Adivina la criatura letra a letra o escribe su nombre completo. Dispones de seis fallos. " +
            "Puedes escribir sin tildes. La Ñ y la N son letras distintas. Las jugadas repetidas se ignoran.",
            "Cómo jugar", MessageBoxButtons.OK, MessageBoxIcon.Information);
        var acerca = Boton("Acerca de");
        acerca.Click += (_, _) => MessageBox.Show(this,
            $"Ahorcado · Edición Minecraft · {Application.ProductVersion}\nProyecto final de C# de davidmotog.\n" +
            "Proyecto de fans independiente de Mojang y Microsoft. Código publicado con licencia MIT.",
            "Acerca de Ahorcado", MessageBoxButtons.OK, MessageBoxIcon.Information);
        var acciones = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Margin = new Padding(0), WrapContents = true };
        acciones.Controls.AddRange([nueva, revelar, ayuda, acerca]);
        panel.Controls.Add(acciones, 0, 6);
        return panel;
    }

    private static Label Etiqueta(string texto = "") => new() { Text = texto, AutoSize = true, ForeColor = Texto };

    private static Button Boton(string texto, bool principal = false)
    {
        var boton = new Button
        {
            Text = texto, AutoSize = true, FlatStyle = FlatStyle.Flat, Padding = new Padding(6, 1, 6, 1),
            ForeColor = principal ? Fondo : Texto, BackColor = principal ? Acento : Panel, UseVisualStyleBackColor = false
        };
        boton.FlatAppearance.BorderColor = principal ? Acento : Color.FromArgb(75, 101, 117);
        return boton;
    }

    private void Comenzar()
    {
        if (partida?.Estado == EstadoPartida.EnCurso &&
            MessageBox.Show(this, "¿Reiniciar la partida?", Text, MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;

        partida = new Partida(Criaturas.Elegir(partida?.Solucion));
        entrada.Clear();
        MostrarEstado("Escribe una letra o prueba el nombre completo.");
        entrada.Focus();
    }

    private void Probar()
    {
        if (partida is null || partida.Estado != EstadoPartida.EnCurso) return;
        ResultadoJugada resultado = partida.Probar(entrada.Text);
        string mensaje = partida.Estado switch
        {
            EstadoPartida.Ganada => "¡Has ganado!",
            EstadoPartida.Perdida => "Has agotado los seis fallos.",
            _ => resultado switch
            {
                ResultadoJugada.Acierto => "¡Acierto!",
                ResultadoJugada.Fallo => "Esa respuesta no coincide.",
                ResultadoJugada.Repetida => "Ya probaste esa respuesta.",
                _ => "Escribe una letra o un nombre con letras y espacios."
            }
        };
        if (resultado != ResultadoJugada.Invalida) entrada.Clear();
        MostrarEstado(mensaje);
        if (partida.Estado == EstadoPartida.EnCurso) entrada.Focus();
        else nueva.Focus();
    }

    private void MostrarSolucion()
    {
        if (partida is null) return;
        partida.Revelar();
        MostrarEstado("Solución revelada.");
        nueva.Focus();
    }

    private void MostrarEstado(string mensaje)
    {
        bool jugando = partida?.Estado == EstadoPartida.EnCurso;
        dibujo.Fallos = partida?.Fallos ?? 0;
        mascara.Text = partida?.Mascara ?? "—";
        intentos.Text = $"Fallos {dibujo.Fallos}/{Partida.MaxFallos}    ·    Intentos {partida?.TotalIntentos ?? 0}";
        letras.Text = $"Letras usadas   {(string.IsNullOrEmpty(partida?.LetrasUsadas) ? "—" : partida.LetrasUsadas)}";
        estado.Text = mensaje;
        entrada.Enabled = enviar.Enabled = revelar.Enabled = jugando;
        nueva.Text = partida is null ? "Comenzar" : "Nueva partida";
        AcceptButton = jugando ? enviar : nueva;
    }
}
