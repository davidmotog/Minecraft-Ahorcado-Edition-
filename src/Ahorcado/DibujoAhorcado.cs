using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Ahorcado;

internal sealed class DibujoAhorcado : Control
{
    private int fallos;

    public DibujoAhorcado()
    {
        DoubleBuffered = true;
        ResizeRedraw = true;
        BackColor = Color.FromArgb(20, 34, 47);
        AccessibleName = "Dibujo del ahorcado";
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    internal int Fallos
    {
        get => fallos;
        set
        {
            if (fallos == value) return;
            fallos = value;
            AccessibleDescription = $"{fallos} de {Partida.MaxFallos} fallos";
            Invalidate();
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        const int ancho = 230, alto = 224;
        float escala = Math.Min(ClientSize.Width / (float)ancho, ClientSize.Height / (float)alto);
        if (escala <= 0) return;
        g.SmoothingMode = SmoothingMode.None;
        g.TranslateTransform((ClientSize.Width - ancho * escala) / 2, (ClientSize.Height - alto * escala) / 2);
        g.ScaleTransform(escala, escala);

        using var suelo = new SolidBrush(Color.FromArgb(38, 64, 74));
        using var madera = new SolidBrush(Color.FromArgb(189, 125, 75));
        using var sombra = new SolidBrush(Color.FromArgb(112, 67, 52));
        using var cuerda = new SolidBrush(Color.FromArgb(238, 193, 115));
        using var metal = new SolidBrush(Color.FromArgb(111, 216, 201));
        using var metalOscuro = new SolidBrush(Color.FromArgb(57, 133, 138));
        using var visor = new SolidBrush(Color.FromArgb(25, 49, 63));
        using var luz = new SolidBrush(Color.FromArgb(250, 224, 129));

        g.FillRectangle(suelo, 10, 197, 210, 9);
        g.FillRectangle(suelo, 20, 206, 190, 8);
        g.FillRectangle(sombra, 23, 186, 74, 11);
        g.FillRectangle(madera, 31, 181, 59, 8);
        g.FillRectangle(sombra, 47, 27, 17, 155);
        g.FillRectangle(madera, 47, 27, 10, 155);
        g.FillRectangle(sombra, 47, 27, 112, 13);
        g.FillRectangle(madera, 47, 27, 112, 7);
        g.FillRectangle(cuerda, 146, 40, 5, 20);

        if (fallos >= 1)
        {
            g.FillRectangle(metalOscuro, 128, 60, 42, 37);
            g.FillRectangle(metal, 128, 60, 38, 33);
            g.FillRectangle(visor, 133, 69, 28, 14);
            g.FillRectangle(luz, 138, 73, 5, 6);
            g.FillRectangle(luz, 152, 73, 5, 6);
        }
        if (fallos >= 2)
        {
            g.FillRectangle(metalOscuro, 130, 98, 38, 40);
            g.FillRectangle(metal, 131, 98, 33, 36);
            g.FillRectangle(visor, 140, 109, 17, 13);
            g.FillRectangle(luz, 144, 113, 9, 5);
        }
        if (fallos >= 3) g.FillRectangle(metalOscuro, 116, 101, 13, 33);
        if (fallos >= 4) g.FillRectangle(metalOscuro, 169, 101, 13, 33);
        if (fallos >= 5) g.FillRectangle(metalOscuro, 132, 139, 13, 34);
        if (fallos >= 6) g.FillRectangle(metalOscuro, 152, 139, 13, 34);
    }
}
