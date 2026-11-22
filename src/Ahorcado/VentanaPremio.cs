using System.ComponentModel;
using System.Diagnostics;

namespace Ahorcado;

internal sealed class VentanaPremio : Form
{
    private readonly PictureBox imagen = new() { Dock = DockStyle.Fill, SizeMode = PictureBoxSizeMode.Zoom, BackColor = VentanaJuego.Panel };
    private readonly Label pie = VentanaJuego.Etiqueta();
    private readonly Font fuentePremio;

    public VentanaPremio(EntradaCatalogo entrada, FuentePixel fuente)
    {
        Text = "¡Has ganado!";
        ClientSize = new Size(580, 470);
        MinimumSize = new Size(535, 430);
        AutoScaleMode = AutoScaleMode.Dpi;
        StartPosition = FormStartPosition.CenterParent;
        BackColor = VentanaJuego.Fondo;
        ForeColor = VentanaJuego.Texto;
        Font = new Font("Segoe UI", 11);
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 5, Padding = new Padding(18) };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        Controls.Add(layout);

        fuentePremio = fuente.Crear(22);
        var titulo = VentanaJuego.Etiqueta(char.ToUpperInvariant(entrada.Nombre[0]) + entrada.Nombre[1..]);
        titulo.Font = fuentePremio;
        titulo.UseCompatibleTextRendering = true;
        titulo.Dock = DockStyle.Top;
        layout.Controls.Add(titulo, 0, 0);
        var categoria = VentanaJuego.Etiqueta(entrada.Categoria);
        categoria.ForeColor = VentanaJuego.Acento;
        categoria.Margin = new Padding(3, 2, 3, 10);
        layout.Controls.Add(categoria, 0, 1);
        layout.Controls.Add(imagen, 0, 2);
        pie.Margin = new Padding(3, 6, 3, 8);
        pie.ForeColor = VentanaJuego.Secundario;
        layout.Controls.Add(pie, 0, 3);
        var botones = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Margin = new Padding(0) };
        var wiki = VentanaJuego.Boton("Ver imágenes en la wiki");
        wiki.Click += (_, _) =>
        {
            try { Process.Start(new ProcessStartInfo(entrada.Enlace.AbsoluteUri) { UseShellExecute = true }); }
            catch (Exception error) when (error is Win32Exception or InvalidOperationException)
            {
                MessageBox.Show(this, entrada.Enlace.AbsoluteUri, "Abre la ficha en tu navegador");
            }
        };
        var añadir = VentanaJuego.Boton("Añadir foto");
        añadir.Click += (_, _) =>
        {
            using var dialogo = new OpenFileDialog { Filter = "Fotos (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg", Title = "Elegir foto para " + entrada.Nombre };
            if (dialogo.ShowDialog(this) != DialogResult.OK) return;
            try { Mostrar(ImagenPremio.Asociar(entrada, dialogo.FileName), "Foto asociada en este equipo"); }
            catch (Exception error) when (error is IOException or UnauthorizedAccessException or InvalidDataException)
            {
                MessageBox.Show(this, error.Message, "No se pudo añadir la foto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        };
        var cerrar = VentanaJuego.Boton("Continuar", principal: true);
        cerrar.DialogResult = DialogResult.OK;
        AcceptButton = CancelButton = cerrar;
        botones.Controls.AddRange([wiki, añadir, cerrar]);
        layout.Controls.Add(botones, 0, 4);
        var (foto, etiqueta) = ImagenPremio.Obtener(entrada);
        Mostrar(foto, etiqueta);
    }

    private void Mostrar(Bitmap foto, string etiqueta)
    {
        var anterior = imagen.Image;
        imagen.Image = foto;
        imagen.AccessibleName = etiqueta;
        pie.Text = etiqueta;
        anterior?.Dispose();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            imagen.Image?.Dispose();
            imagen.Image = null;
            fuentePremio.Dispose();
        }
        base.Dispose(disposing);
    }
}
