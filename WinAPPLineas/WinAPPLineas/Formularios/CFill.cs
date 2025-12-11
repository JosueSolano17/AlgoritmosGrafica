using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinAPPLineas
{
    public partial class CFill : Form
    {
        // Mueva enum a internal para poder referenciarlo desde wrappers si lo prefieres; aquí lo mantenemos y usamos índice.
        private enum FillVariant { FloodFillPila, BoundaryFill, ScanlineFill }

        // Añadidos: referencias a controles para poder ocultar selector en wrappers
        private ComboBox _cmbVariant;
        private Button _btnDemo;
        private Button _btnClear;
        private PictureBox _pic;

        private Bitmap _canvas;
        private Graphics _gfx;
        private Color _fillColor = Color.CornflowerBlue;
        private Color _boundaryColor = Color.Black;
        private FillVariant _variant = FillVariant.FloodFillPila;
        private List<Point> _polygon = new List<Point>(); // Para Scanline
        private bool _drawingPolygon;

        public CFill()
        {
            InitializeComponent();
            Text = "Algoritmos de Relleno";
            Width = 900; Height = 650;

            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 40 };
            _cmbVariant = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 220 };
            _cmbVariant.Items.AddRange(new object[] { "Flood Fill (Pila)", "Boundary Fill", "Scanline Fill (Polígono)" });
            _cmbVariant.SelectedIndex = 0;
            _cmbVariant.SelectedIndexChanged += (s, e) =>
            {
                _variant = (FillVariant)_cmbVariant.SelectedIndex;
                ResetCanvas();
            };

            _btnDemo = new Button { Text = "Demostración", Left = 230, Width = 120 };
            _btnDemo.Click += (s, e) => Demo();

            _btnClear = new Button { Text = "Limpiar", Left = 360, Width = 90 };
            _btnClear.Click += (s, e) => ResetCanvas();

            var lblInfo = new Label { Text = "Click sobre el lienzo para rellenar o definir polígonos.", AutoSize = true, Left = 460, Top = 10 };

            pnlTop.Controls.Add(_cmbVariant);
            pnlTop.Controls.Add(_btnDemo);
            pnlTop.Controls.Add(_btnClear);
            pnlTop.Controls.Add(lblInfo);

            _pic = new PictureBox { Dock = DockStyle.Fill, BackColor = Color.White };
            _pic.MouseClick += PictureMouseClick;
            Controls.Add(_pic);
            Controls.Add(pnlTop);

            _canvas = new Bitmap(800, 550);
            _pic.Image = _canvas;
            _gfx = Graphics.FromImage(_canvas);
            _gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

            ResetCanvas();
        }

        private void ResetCanvas()
        {
            _gfx.Clear(Color.White);
            _polygon.Clear();
            _drawingPolygon = false;
            // Dibuja una forma por defecto para Flood/Boundary
            using (var pen = new Pen(_boundaryColor, 1))
            {
                _gfx.DrawRectangle(pen, 100, 100, 300, 250);
                _gfx.DrawEllipse(pen, 450, 120, 200, 200);
            }
            Refresh();
        }

        private void PictureMouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (_variant == FillVariant.ScanlineFill)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        // Definir vértices
                        _polygon.Add(e.Location);
                        if (_polygon.Count > 1)
                        {
                            using (var pen = new Pen(_boundaryColor, 1))
                                _gfx.DrawLine(pen, _polygon[_polygon.Count - 2], _polygon[_polygon.Count - 1]);
                        }
                        _drawingPolygon = true;
                        Refresh();
                    }
                    else if (e.Button == MouseButtons.Right && _polygon.Count >= 3)
                    {
                        // Cerrar polígono
                        using (var pen = new Pen(_boundaryColor, 1))
                            _gfx.DrawLine(pen, _polygon.Last(), _polygon.First());
                        _drawingPolygon = false;
                        // Validación de polígono simple
                        if (!IsPolygonValid(_polygon))
                        {
                            MessageBox.Show("Polígono inválido. Verifica que no tenga autointersecciones.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        ScanlineFill(_polygon, _fillColor);
                        Refresh();
                    }
                }
                else
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        if (_variant == FillVariant.FloodFillPila)
                            FloodFillStack(e.Location, _fillColor);
                        else
                            BoundaryFillStack(e.Location, _fillColor, _boundaryColor);
                        Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error procesando clic: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Demo()
        {
            try
            {
                ResetCanvas();
                if (_variant == FillVariant.FloodFillPila)
                {
                    FloodFillStack(new Point(150, 150), _fillColor);
                    FloodFillStack(new Point(520, 180), Color.LightGreen);
                }
                else if (_variant == FillVariant.BoundaryFill)
                {
                    BoundaryFillStack(new Point(150, 150), _fillColor, _boundaryColor);
                    BoundaryFillStack(new Point(520, 180), Color.LightSalmon, _boundaryColor);
                }
                else
                {
                    // Polígono de demostración
                    var poly = new List<Point>
                    {
                        new Point(200,200), new Point(400,220), new Point(380,320),
                        new Point(260,360), new Point(180,280)
                    };
                    using (var pen = new Pen(_boundaryColor, 1))
                    {
                        for (int i = 0; i < poly.Count; i++)
                            _gfx.DrawLine(pen, poly[i], poly[(i + 1) % poly.Count]);
                    }
                    ScanlineFill(poly, _fillColor);
                }
                Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en demostración: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Flood Fill (iterativo con pila) sobre Bitmap
        private void FloodFillStack(Point seed, Color fillColor)
        {
            if (!Inside(seed)) return;
            var targetColor = _canvas.GetPixel(seed.X, seed.Y);
            if (targetColor.ToArgb() == fillColor.ToArgb()) return;

            var stack = new Stack<Point>();
            stack.Push(seed);
            while (stack.Count > 0)
            {
                var p = stack.Pop();
                if (!Inside(p)) continue;
                var c = _canvas.GetPixel(p.X, p.Y);
                if (c.ToArgb() != targetColor.ToArgb()) continue;

                _canvas.SetPixel(p.X, p.Y, fillColor);
                stack.Push(new Point(p.X + 1, p.Y));
                stack.Push(new Point(p.X - 1, p.Y));
                stack.Push(new Point(p.X, p.Y + 1));
                stack.Push(new Point(p.X, p.Y - 1));
            }
        }

        // Boundary Fill (iterativo con pila) parando en boundaryColor
        private void BoundaryFillStack(Point seed, Color fillColor, Color boundaryColor)
        {
            if (!Inside(seed)) return;
            var c0 = _canvas.GetPixel(seed.X, seed.Y);
            if (c0.ToArgb() == boundaryColor.ToArgb() || c0.ToArgb() == fillColor.ToArgb()) return;

            var stack = new Stack<Point>();
            stack.Push(seed);
            while (stack.Count > 0)
            {
                var p = stack.Pop();
                if (!Inside(p)) continue;

                var c = _canvas.GetPixel(p.X, p.Y);
                if (c.ToArgb() == boundaryColor.ToArgb() || c.ToArgb() == fillColor.ToArgb()) continue;

                _canvas.SetPixel(p.X, p.Y, fillColor);

                stack.Push(new Point(p.X + 1, p.Y));
                stack.Push(new Point(p.X - 1, p.Y));
                stack.Push(new Point(p.X, p.Y + 1));
                stack.Push(new Point(p.X, p.Y - 1));
            }
        }

        // Scanline Fill (polígono simple, no autointersecciones)
        private void ScanlineFill(List<Point> polygon, Color fill)
        {
            if (polygon == null || polygon.Count < 3) return;
            var minY = polygon.Min(p => p.Y);
            var maxY = polygon.Max(p => p.Y);

            for (int y = minY; y <= maxY; y++)
            {
                var intersections = new List<int>();
                for (int i = 0; i < polygon.Count; i++)
                {
                    var p1 = polygon[i];
                    var p2 = polygon[(i + 1) % polygon.Count];

                    if (p1.Y == p2.Y) continue; // ignorar horizontales

                    var ymin = Math.Min(p1.Y, p2.Y);
                    var ymax = Math.Max(p1.Y, p2.Y);
                    if (y < ymin || y >= ymax) continue;

                    // Intersección con la línea horizontal y
                    double x = p1.X + (double)(y - p1.Y) * (p2.X - p1.X) / (p2.Y - p1.Y);
                    intersections.Add((int)Math.Round(x));
                }

                intersections.Sort();
                for (int k = 0; k + 1 < intersections.Count; k += 2)
                {
                    int x1 = intersections[k];
                    int x2 = intersections[k + 1];
                    for (int x = Math.Min(x1, x2); x <= Math.Max(x1, x2); x++)
                    {
                        var p = new Point(x, y);
                        if (Inside(p)) _canvas.SetPixel(x, y, fill);
                    }
                }
            }
        }

        private bool Inside(Point p) => p.X >= 0 && p.Y >= 0 && p.X < _canvas.Width && p.Y < _canvas.Height;

        // Validación simple de polígono: sin autointersecciones
        private bool IsPolygonValid(List<Point> pts)
        {
            int n = pts.Count;
            if (n < 3) return false;
            Func<Point, Point, Point, Point, bool> segIntersect = (a, b, c, d) =>
            {
                bool Intersect1D(int a1, int a2, int b1, int b2) =>
                    Math.Max(Math.Min(a1, a2), Math.Min(b1, b2)) <= Math.Min(Math.Max(a1, a2), Math.Max(b1, b2));

                int Cross(Point u, Point v) => (u.X * v.Y - u.Y * v.X);
                Point Sub(Point u, Point v) => new Point(u.X - v.X, u.Y - v.Y);

                if (!Intersect1D(a.X, b.X, c.X, d.X) || !Intersect1D(a.Y, b.Y, c.Y, d.Y)) return false;
                var ab = Sub(b, a); var ac = Sub(c, a); var ad = Sub(d, a);
                var cd = Sub(d, c); var ca = Sub(a, c); var cb = Sub(b, c);
                int s1 = Math.Sign(Cross(ab, ac)); int s2 = Math.Sign(Cross(ab, ad));
                int s3 = Math.Sign(Cross(cd, ca)); int s4 = Math.Sign(Cross(cd, cb));
                return s1 * s2 <= 0 && s3 * s4 <= 0;
            };

            for (int i = 0; i < n; i++)
            {
                var a = pts[i];
                var b = pts[(i + 1) % n];
                for (int j = i + 1; j < n; j++)
                {
                    // Evitar adyacentes
                    if (j == i || j == (i + 1) % n || (j + 1) % n == i) continue;
                    var c = pts[j];
                    var d = pts[(j + 1) % n];
                    if (segIntersect(a, b, c, d)) return false;
                }
            }
            return true;
        }

        // Agrega este método parcial para evitar el error CS0103.
        // Si usas Windows Forms Designer, este método es generado automáticamente.
        // Si no usas el diseñador, puedes dejarlo vacío para evitar el error.
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CFill
            // 
            this.ClientSize = new System.Drawing.Size(928, 485);
            this.Name = "CFill";
            this.Load += new System.EventHandler(this.CFill_Load);
            this.ResumeLayout(false);

        }

        // Método público para fijar variante y ocultar selector (usado por wrappers)
        public void SetVariant(int index, bool hideSelector = true)
        {
            if (index < 0 || index > 2) throw new ArgumentOutOfRangeException(nameof(index));
            _variant = (FillVariant)index;
            if (_cmbVariant != null)
            {
                _cmbVariant.SelectedIndexChanged -= (s, e) => { }; // evita repetición si se reasigna
                _cmbVariant.SelectedIndex = index;
                if (hideSelector) _cmbVariant.Visible = false;
            }
            ResetCanvas();
        }

        // Exponer Demo para que el wrapper pueda invocarla
        public void RunDemoSafe()
        {
            Demo();
        }

        private void CFill_Load(object sender, EventArgs e)
        {

        }
    }
}