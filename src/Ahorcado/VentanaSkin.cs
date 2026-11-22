using System.Text.Json;

namespace Ahorcado;

internal sealed class VentanaSkin : Form
{
    private readonly TextBox nombre = new() { Width = 190, MaxLength = 16, AccessibleName = "Nombre de Minecraft Java" };
    private readonly Button buscar = VentanaJuego.Boton("Buscar skin");
    private readonly Button cargar = VentanaJuego.Boton("Abrir PNG");
    private readonly Button restaurar = VentanaJuego.Boton("Usar muñeco original");
    private readonly Button guardar = VentanaJuego.Boton("Aplicar", principal: true);
    private readonly Label estado = VentanaJuego.Etiqueta();
    private readonly DibujoAhorcado vista = new() { Dock = DockStyle.Fill, Fallos = Partida.MaxFallos };
    private readonly CancellationTokenSource cierre = new();
    private SkinJugador? borrador;
    private bool cambio;

    public VentanaSkin(SkinJugador? actual)
    {
        Text = "Personalizar muñeco";
        StartPosition = FormStartPosition.CenterParent;
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(570, 385);
        MinimumSize = new Size(530, 385);
        BackColor = VentanaJuego.Fondo;
        ForeColor = VentanaJuego.Texto;
        Font = new Font("Segoe UI", 11);

        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1, Padding = new Padding(16) };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62));
        var opciones = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, Padding = new Padding(10, 0, 0, 0) };
        opciones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        layout.Controls.Add(vista, 0, 0);
        layout.Controls.Add(opciones, 1, 0);
        Controls.Add(layout);

        opciones.Controls.Add(VentanaJuego.Etiqueta("Nombre de jugador de Minecraft Java"));
        opciones.Controls.Add(nombre);
        opciones.Controls.Add(buscar);
        var delgado = new CheckBox { Text = "Brazos delgados (PNG)", AutoSize = true };
        opciones.Controls.Add(delgado);
        opciones.Controls.Add(cargar);
        opciones.Controls.Add(restaurar);
        estado.Dock = DockStyle.Fill;
        estado.MinimumSize = new Size(0, 60);
        opciones.Controls.Add(estado);
        var cancelar = VentanaJuego.Boton("Cancelar");
        cancelar.DialogResult = DialogResult.Cancel;
        var botones = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = false, Margin = new Padding(0) };
        botones.Controls.AddRange([guardar, cancelar]);
        opciones.Controls.Add(botones);

        vista.UsarSkin(actual);
        estado.Text = actual is null ? "Muñeco original" : $"Skin actual · {actual.Nombre}";
        guardar.Enabled = false;
        AcceptButton = buscar;
        CancelButton = cancelar;
        buscar.Click += async (_, _) => await Buscar();
        cargar.Click += (_, _) =>
        {
            using var dialogo = new OpenFileDialog { Filter = "Skin de Minecraft (*.png)|*.png", Title = "Abrir skin de Minecraft" };
            if (dialogo.ShowDialog(this) != DialogResult.OK) return;
            try { Mostrar(SkinJugador.DesdeArchivo(dialogo.FileName, delgado.Checked)); }
            catch (Exception error) when (error is InvalidDataException or IOException or UnauthorizedAccessException)
            {
                estado.Text = error.Message;
            }
        };
        restaurar.Click += (_, _) => Mostrar(null);
        guardar.Click += (_, _) =>
        {
            try
            {
                SkinJugador.GuardarPreferencia(borrador);
                DialogResult = DialogResult.OK;
            }
            catch (Exception error) when (error is IOException or UnauthorizedAccessException)
            {
                estado.Text = "No se pudo guardar la skin en este equipo.";
            }
        };
    }

    private async Task Buscar()
    {
        buscar.Enabled = cargar.Enabled = restaurar.Enabled = guardar.Enabled = false;
        estado.Text = "Buscando la skin en Mojang…";
        try
        {
            var encontrada = await SkinJugador.Buscar(nombre.Text.Trim(), cierre.Token);
            if (IsDisposed || cierre.IsCancellationRequested) encontrada.Dispose();
            else Mostrar(encontrada);
        }
        catch (OperationCanceledException) when (cierre.IsCancellationRequested) { }
        catch (OperationCanceledException) { estado.Text = "Mojang tardó demasiado en responder. Vuelve a intentarlo."; }
        catch (InvalidDataException error) { estado.Text = error.Message; }
        catch (Exception error) when (error is HttpRequestException or JsonException or FormatException or InvalidOperationException or KeyNotFoundException)
        {
            estado.Text = "No se pudo obtener la skin desde Mojang. También puedes abrir un PNG.";
        }
        finally
        {
            if (!IsDisposed)
            {
                buscar.Enabled = cargar.Enabled = restaurar.Enabled = true;
                guardar.Enabled = cambio;
            }
        }
    }

    private void Mostrar(SkinJugador? nueva)
    {
        vista.UsarSkin(nueva);
        borrador?.Dispose();
        borrador = nueva;
        cambio = true;
        guardar.Enabled = true;
        estado.Text = nueva is null ? "Muñeco original" : $"Vista previa · {nueva.Nombre}";
    }

    public SkinJugador? TomarSkin()
    {
        vista.UsarSkin(null);
        var elegida = borrador;
        borrador = null;
        return elegida;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && !IsDisposed)
        {
            cierre.Cancel();
            cierre.Dispose();
            borrador?.Dispose();
        }
        base.Dispose(disposing);
    }
}
