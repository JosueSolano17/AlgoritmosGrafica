using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinAPPLineas.Clases;
using static WinAPPLineas.Clases.CRecortarPoligono;

namespace WinAPPLineas.Formularios
{
    public partial class WeilerAthertonForm : Form
    {
        private PictureBox picCanvas;
        private Panel pnlControls;
        private GroupBox grpSubjectPolygon;
        private GroupBox grpClipPolygon;
        private GroupBox grpAnimation;
        private GroupBox grpVisualization;
        private GroupBox grpActions;
        private Label lblInstructions;
        private Label lblStats;
        private Button btnAddSubjectVertex;
        private Button btnClearSubject;
        private Button btnAddClipVertex;
        private Button btnClearClip;
        private Button btnClip;
        private Button btnClear;
        private Button btnStop;
        private Button btnLoadPreset;
        private CheckBox chkAnimate;
        private CheckBox chkShowIntersections;
        private CheckBox chkShowVertexNumbers;
        private CheckBox chkShowGrid;
        private TrackBar trkSpeed;
        private Label lblSpeed;
        private Label lblSpeedValue;
        private ListBox lstSteps;
        private ListBox lstSubjectVertices;
        private ListBox lstClipVertices;

        private Bitmap canvas;
        private Bitmap displayBitmap;
        private Graphics gfx;
        private Graphics displayGfx;

        private Color subjectColor = Color.FromArgb(0, 188, 212); // Cian
        private Color clipColor = Color.FromArgb(255, 193, 7); // Amarillo
        private Color resultColor = Color.FromArgb(139, 195, 74); // Verde lima
        private Color intersectionColor = Color.FromArgb(244, 67, 54); // Rojo
        private Color gridColor = Color.FromArgb(200, 200, 200);
        private Color bgColor = Color.FromArgb(250, 250, 250);
        private Color vertexColor = Color.FromArgb(103, 58, 183); // Púrpura

        private bool isAnimating = false;
        private CancellationTokenSource cancellationTokenSource;

        private List<PointD> subjectPolygon = new List<PointD>();
        private List<PointD> clipPolygon = new List<PointD>();
        private List<PointD> resultPolygon = new List<PointD>();
        private List<PointD> intersectionPoints = new List<PointD>();

        private const int CANVAS_WIDTH = 600;
        private const int CANVAS_HEIGHT = 600;
        private const int GRID_SIZE = 20;

        private bool isAddingSubject = false;
        private bool isAddingClip = false;

        public WeilerAthertonForm()
        {
            InitializeComponent();
            InitializeCustomComponents();
            SetupCanvas();
        }

        private void InitializeCustomComponents()
        {
            this.Text = "Weiler-Atherton - Recorte de Polígonos";
            this.Size = new Size(1050, 850);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 245, 245);

            // Panel de controles
            pnlControls = new Panel
            {
                Dock = DockStyle.Right,
                Width = 350,
                BackColor = Color.FromArgb(33, 33, 33),
                Padding = new Padding(10),
                AutoScroll = true
            };

            // GroupBox de polígono sujeto
            grpSubjectPolygon = new GroupBox
            {
                Text = "Polígono Sujeto (Cian)",
                Location = new Point(10, 10),
                Size = new Size(330, 180),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(224, 224, 224)
            };

            lstSubjectVertices = new ListBox
            {
                Location = new Point(10, 25),
                Size = new Size(310, 100),
                Font = new Font("Consolas", 8F),
                BackColor = Color.FromArgb(48, 48, 48),
                ForeColor = Color.FromArgb(224, 224, 224)
            };

            btnAddSubjectVertex = new Button
            {
                Text = "➕ Click en Canvas para Agregar",
                Location = new Point(10, 130),
                Size = new Size(310, 20),
                BackColor = Color.FromArgb(0, 188, 212),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAddSubjectVertex.FlatAppearance.BorderSize = 0;
            btnAddSubjectVertex.Click += BtnAddSubjectVertex_Click;

            btnClearSubject = new Button
            {
                Text = "🗑 Limpiar Sujeto",
                Location = new Point(10, 153),
                Size = new Size(310, 20),
                BackColor = Color.FromArgb(120, 120, 120),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnClearSubject.FlatAppearance.BorderSize = 0;
            btnClearSubject.Click += (s, e) => { subjectPolygon.Clear(); UpdateVertexLists(); RedrawDisplay(); };

            grpSubjectPolygon.Controls.AddRange(new Control[] { lstSubjectVertices, btnAddSubjectVertex, btnClearSubject });

            // GroupBox de polígono de recorte
            grpClipPolygon = new GroupBox
            {
                Text = "Polígono de Recorte (Amarillo)",
                Location = new Point(10, 200),
                Size = new Size(330, 180),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(224, 224, 224)
            };

            lstClipVertices = new ListBox
            {
                Location = new Point(10, 25),
                Size = new Size(310, 100),
                Font = new Font("Consolas", 8F),
                BackColor = Color.FromArgb(48, 48, 48),
                ForeColor = Color.FromArgb(224, 224, 224)
            };

            btnAddClipVertex = new Button
            {
                Text = "➕ Click en Canvas para Agregar",
                Location = new Point(10, 130),
                Size = new Size(310, 20),
                BackColor = Color.FromArgb(255, 193, 7),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAddClipVertex.FlatAppearance.BorderSize = 0;
            btnAddClipVertex.Click += BtnAddClipVertex_Click;

            btnClearClip = new Button
            {
                Text = "🗑 Limpiar Recorte",
                Location = new Point(10, 153),
                Size = new Size(310, 20),
                BackColor = Color.FromArgb(120, 120, 120),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnClearClip.FlatAppearance.BorderSize = 0;
            btnClearClip.Click += (s, e) => { clipPolygon.Clear(); UpdateVertexLists(); RedrawDisplay(); };

            grpClipPolygon.Controls.AddRange(new Control[] { lstClipVertices, btnAddClipVertex, btnClearClip });

            // GroupBox de animación
            grpAnimation = new GroupBox
            {
                Text = "Animación",
                Location = new Point(10, 390),
                Size = new Size(330, 100),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(224, 224, 224)
            };

            chkAnimate = new CheckBox
            {
                Text = "Activar animación paso a paso",
                Location = new Point(10, 25),
                Size = new Size(310, 20),
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(224, 224, 224),
                Checked = true
            };

            lblSpeed = new Label
            {
                Text = "Velocidad:",
                Location = new Point(10, 50),
                Size = new Size(80, 20),
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(224, 224, 224)
            };

            lblSpeedValue = new Label
            {
                Text = "Media",
                Location = new Point(250, 50),
                Size = new Size(70, 20),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(139, 195, 74),
                TextAlign = ContentAlignment.MiddleRight
            };

            trkSpeed = new TrackBar
            {
                Location = new Point(10, 70),
                Size = new Size(310, 25),
                Minimum = 0,
                Maximum = 100,
                Value = 50,
                TickFrequency = 25
            };
            trkSpeed.ValueChanged += TrkSpeed_ValueChanged;

            grpAnimation.Controls.AddRange(new Control[] { chkAnimate, lblSpeed, lblSpeedValue, trkSpeed });

            // GroupBox de visualización
            grpVisualization = new GroupBox
            {
                Text = "Opciones de Visualización",
                Location = new Point(10, 500),
                Size = new Size(330, 110),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(224, 224, 224)
            };

            chkShowIntersections = new CheckBox
            {
                Text = "Mostrar puntos de intersección",
                Location = new Point(10, 25),
                Size = new Size(310, 20),
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(224, 224, 224),
                Checked = true
            };

            chkShowVertexNumbers = new CheckBox
            {
                Text = "Mostrar números de vértices",
                Location = new Point(10, 50),
                Size = new Size(310, 20),
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(224, 224, 224),
                Checked = true
            };

            chkShowGrid = new CheckBox
            {
                Text = "Mostrar cuadrícula",
                Location = new Point(10, 75),
                Size = new Size(310, 20),
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(224, 224, 224),
                Checked = true
            };
            chkShowGrid.CheckedChanged += (s, e) => RedrawDisplay();

            grpVisualization.Controls.AddRange(new Control[] { chkShowIntersections, chkShowVertexNumbers, chkShowGrid });

            // GroupBox de acciones
            grpActions = new GroupBox
            {
                Text = "Acciones",
                Location = new Point(10, 620),
                Size = new Size(330, 100),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(224, 224, 224)
            };

            btnLoadPreset = new Button
            {
                Text = "📋 Cargar Ejemplo",
                Location = new Point(10, 25),
                Size = new Size(310, 30),
                BackColor = Color.FromArgb(63, 81, 181),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnLoadPreset.FlatAppearance.BorderSize = 0;
            btnLoadPreset.Click += BtnLoadPreset_Click;

            btnClip = new Button
            {
                Text = "✂ Recortar Polígono",
                Location = new Point(10, 60),
                Size = new Size(310, 30),
                BackColor = Color.FromArgb(139, 195, 74),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnClip.FlatAppearance.BorderSize = 0;
            btnClip.Click += BtnClip_Click;

            grpActions.Controls.AddRange(new Control[] { btnLoadPreset, btnClip });

            // Lista de pasos
            Label lblStepsTitle = new Label
            {
                Text = "Pasos del Algoritmo:",
                Location = new Point(10, 730),
                Size = new Size(330, 20),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(224, 224, 224)
            };

            lstSteps = new ListBox
            {
                Location = new Point(10, 755),
                Size = new Size(330, 100),
                Font = new Font("Consolas", 7.5F),
                BackColor = Color.FromArgb(48, 48, 48),
                ForeColor = Color.FromArgb(224, 224, 224),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Label de estadísticas
            lblStats = new Label
            {
                Text = "Estado: Listo\nAgrega vértices haciendo click",
                Location = new Point(10, 865),
                Size = new Size(330, 40),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(224, 224, 224),
                BackColor = Color.FromArgb(48, 48, 48),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(5),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Botones principales
            btnClear = new Button
            {
                Text = "🗑 Limpiar Todo",
                Location = new Point(10, 915),
                Size = new Size(160, 35),
                BackColor = Color.FromArgb(120, 120, 120),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnClear.FlatAppearance.BorderSize = 0;
            btnClear.Click += BtnClear_Click;

            btnStop = new Button
            {
                Text = "⏹ Detener",
                Location = new Point(180, 915),
                Size = new Size(160, 35),
                BackColor = Color.FromArgb(244, 67, 54),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Enabled = false
            };
            btnStop.FlatAppearance.BorderSize = 0;
            btnStop.Click += BtnStop_Click;

            // Instrucciones
            lblInstructions = new Label
            {
                Text = "📋 WEILER-ATHERTON:\n\n" +
                       "Algoritmo avanzado para\n" +
                       "recorte de polígonos.\n\n" +
                       "CARACTERÍSTICAS:\n" +
                       "• Funciona con polígonos\n" +
                       "  cóncavos y convexos\n" +
                       "• Puede producir múltiples\n" +
                       "  polígonos resultantes\n" +
                       "• Maneja casos complejos\n\n" +
                       "PASOS:\n" +
                       "1. Encuentra intersecciones\n" +
                       "2. Construye lista enlazada\n" +
                       "3. Marca entrada/salida\n" +
                       "4. Recorre para obtener\n" +
                       "   resultado\n\n" +
                       "Nota: Implementación\n" +
                       "simplificada con fallback.",
                Location = new Point(10, 960),
                Size = new Size(330, 340),
                Font = new Font("Segoe UI", 7.5F),
                ForeColor = Color.FromArgb(224, 224, 224),
                BackColor = Color.FromArgb(48, 48, 48),
                Padding = new Padding(8),
                BorderStyle = BorderStyle.FixedSingle
            };

            pnlControls.Controls.AddRange(new Control[] {
                grpSubjectPolygon, grpClipPolygon, grpAnimation, grpVisualization, grpActions,
                lblStepsTitle, lstSteps, lblStats, btnClear, btnStop, lblInstructions
            });

            // PictureBox
            picCanvas = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = bgColor,
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.CenterImage,
                Cursor = Cursors.Cross
            };
            picCanvas.MouseClick += PicCanvas_MouseClick;

            this.Controls.Add(picCanvas);
            this.Controls.Add(pnlControls);
        }

        private void SetupCanvas()
        {
            canvas = new Bitmap(CANVAS_WIDTH, CANVAS_HEIGHT);
            displayBitmap = new Bitmap(CANVAS_WIDTH, CANVAS_HEIGHT);

            gfx = Graphics.FromImage(canvas);
            displayGfx = Graphics.FromImage(displayBitmap);

            gfx.SmoothingMode = SmoothingMode.AntiAlias;
            displayGfx.SmoothingMode = SmoothingMode.AntiAlias;

            RedrawDisplay();
        }

        private void RedrawDisplay()
        {
            displayGfx.Clear(bgColor);

            if (chkShowGrid.Checked)
            {
                DrawGrid();
            }

            DrawAxes();

            // Dibujar polígonos
            if (clipPolygon.Count >= 3)
            {
                DrawPolygon(clipPolygon, clipColor, "Clip", chkShowVertexNumbers.Checked);
            }

            if (subjectPolygon.Count >= 3)
            {
                DrawPolygon(subjectPolygon, subjectColor, "Subject", chkShowVertexNumbers.Checked);
            }

            if (resultPolygon.Count >= 3)
            {
                DrawPolygon(resultPolygon, resultColor, "Result", false, true);
            }

            // Dibujar intersecciones
            if (chkShowIntersections.Checked && intersectionPoints.Count > 0)
            {
                foreach (var pt in intersectionPoints)
                {
                    DrawIntersectionPoint(pt);
                }
            }

            picCanvas.Image = displayBitmap;
            picCanvas.Refresh();
        }

        private void DrawGrid()
        {
            using (Pen gridPen = new Pen(gridColor, 1))
            {
                gridPen.DashStyle = DashStyle.Dot;

                for (int x = 0; x < CANVAS_WIDTH; x += GRID_SIZE)
                {
                    displayGfx.DrawLine(gridPen, x, 0, x, CANVAS_HEIGHT);
                }

                for (int y = 0; y < CANVAS_HEIGHT; y += GRID_SIZE)
                {
                    displayGfx.DrawLine(gridPen, 0, y, CANVAS_WIDTH, y);
                }
            }
        }

        private void DrawAxes()
        {
            int centerX = CANVAS_WIDTH / 2;
            int centerY = CANVAS_HEIGHT / 2;

            using (Pen axisPen = new Pen(Color.Black, 2))
            {
                displayGfx.DrawLine(axisPen, 0, centerY, CANVAS_WIDTH, centerY);
                displayGfx.DrawLine(axisPen, centerX, 0, centerX, CANVAS_HEIGHT);
            }

            using (Font font = new Font("Arial", 8))
            using (SolidBrush brush = new SolidBrush(Color.Black))
            {
                displayGfx.DrawString("X", font, brush, CANVAS_WIDTH - 15, centerY + 5);
                displayGfx.DrawString("Y", font, brush, centerX + 5, 5);
                displayGfx.DrawString("0", font, brush, centerX + 5, centerY + 5);
            }
        }

        private void DrawPolygon(List<PointD> polygon, Color color, string label, bool showNumbers, bool filled = false)
        {
            if (polygon.Count < 2) return;

            var screenPoints = polygon.Select(p => WorldToScreen(p)).ToArray();

            // Rellenar si es el resultado
            if (filled && polygon.Count >= 3)
            {
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(100, color)))
                {
                    displayGfx.FillPolygon(brush, screenPoints);
                }
            }

            // Dibujar bordes
            using (Pen pen = new Pen(color, filled ? 4 : 2))
            {
                if (polygon.Count >= 3)
                {
                    displayGfx.DrawPolygon(pen, screenPoints);
                }
                else
                {
                    for (int i = 0; i < polygon.Count - 1; i++)
                    {
                        displayGfx.DrawLine(pen, screenPoints[i], screenPoints[i + 1]);
                    }
                }
            }

            // Dibujar vértices
            for (int i = 0; i < polygon.Count; i++)
            {
                var p = screenPoints[i];
                using (SolidBrush brush = new SolidBrush(color))
                {
                    displayGfx.FillEllipse(brush, p.X - 4, p.Y - 4, 8, 8);
                }

                using (Pen pen = new Pen(Color.White, 2))
                {
                    displayGfx.DrawEllipse(pen, p.X - 4, p.Y - 4, 8, 8);
                }

                if (showNumbers)
                {
                    using (Font font = new Font("Arial", 8, FontStyle.Bold))
                    using (SolidBrush brush = new SolidBrush(Color.White))
                    using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(200, color)))
                    {
                        string text = i.ToString();
                        var size = displayGfx.MeasureString(text, font);
                        displayGfx.FillEllipse(bgBrush, p.X + 8, p.Y - 8, size.Width + 4, size.Height + 2);
                        displayGfx.DrawString(text, font, brush, p.X + 10, p.Y - 7);
                    }
                }
            }
        }

        private void DrawIntersectionPoint(PointD pt)
        {
            var p = WorldToScreen(pt);

            // Dibujar cruz
            using (Pen pen = new Pen(intersectionColor, 3))
            {
                displayGfx.DrawLine(pen, p.X - 6, p.Y - 6, p.X + 6, p.Y + 6);
                displayGfx.DrawLine(pen, p.X - 6, p.Y + 6, p.X + 6, p.Y - 6);
            }

            // Círculo alrededor
            using (Pen pen = new Pen(intersectionColor, 2))
            {
                displayGfx.DrawEllipse(pen, p.X - 8, p.Y - 8, 16, 16);
            }
        }

        private PointF WorldToScreen(PointD p)
        {
            int centerX = CANVAS_WIDTH / 2;
            int centerY = CANVAS_HEIGHT / 2;

            float screenX = centerX + (float)p.X;
            float screenY = centerY - (float)p.Y;

            return new PointF(screenX, screenY);
        }

        private PointD ScreenToWorld(Point p)
        {
            int centerX = CANVAS_WIDTH / 2;
            int centerY = CANVAS_HEIGHT / 2;

            double worldX = p.X - centerX;
            double worldY = centerY - p.Y;

            return new PointD(worldX, worldY);
        }

        private void PicCanvas_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var worldPoint = ScreenToWorld(e.Location);

            if (isAddingSubject)
            {
                subjectPolygon.Add(worldPoint);
                UpdateVertexLists();
                RedrawDisplay();
                lblStats.Text = $"Vértice sujeto agregado: {worldPoint}\nTotal: {subjectPolygon.Count}";
            }
            else if (isAddingClip)
            {
                clipPolygon.Add(worldPoint);
                UpdateVertexLists();
                RedrawDisplay();
                lblStats.Text = $"Vértice recorte agregado: {worldPoint}\nTotal: {clipPolygon.Count}";
            }
        }

        private void BtnAddSubjectVertex_Click(object sender, EventArgs e)
        {
            isAddingSubject = !isAddingSubject;
            isAddingClip = false;

            btnAddSubjectVertex.BackColor = isAddingSubject ? Color.FromArgb(0, 200, 83) : Color.FromArgb(0, 188, 212);
            btnAddSubjectVertex.Text = isAddingSubject ? "✓ Agregando... (Click para Detener)" : "➕ Click en Canvas para Agregar";
            btnAddClipVertex.BackColor = Color.FromArgb(255, 193, 7);
            btnAddClipVertex.Text = "➕ Click en Canvas para Agregar";

            lblStats.Text = isAddingSubject ? "Modo: Agregando vértices al SUJETO" : "Modo agregación desactivado";
        }

        private void BtnAddClipVertex_Click(object sender, EventArgs e)
        {
            isAddingClip = !isAddingClip;
            isAddingSubject = false;

            btnAddClipVertex.BackColor = isAddingClip ? Color.FromArgb(0, 200, 83) : Color.FromArgb(255, 193, 7);
            btnAddClipVertex.ForeColor = isAddingClip ? Color.White : Color.Black;
            btnAddClipVertex.Text = isAddingClip ? "✓ Agregando... (Click para Detener)" : "➕ Click en Canvas para Agregar";
            btnAddSubjectVertex.BackColor = Color.FromArgb(0, 188, 212);
            btnAddSubjectVertex.Text = "➕ Click en Canvas para Agregar";

            lblStats.Text = isAddingClip ? "Modo: Agregando vértices al RECORTE" : "Modo agregación desactivado";
        }

        private void UpdateVertexLists()
        {
            lstSubjectVertices.Items.Clear();
            for (int i = 0; i < subjectPolygon.Count; i++)
            {
                lstSubjectVertices.Items.Add($"{i}: {subjectPolygon[i]}");
            }

            lstClipVertices.Items.Clear();
            for (int i = 0; i < clipPolygon.Count; i++)
            {
                lstClipVertices.Items.Add($"{i}: {clipPolygon[i]}");
            }
        }

        private void BtnLoadPreset_Click(object sender, EventArgs e)
        {
            subjectPolygon.Clear();
            clipPolygon.Clear();

            // Polígono sujeto: Hexágono
            double angleStep = 2 * Math.PI / 6;
            for (int i = 0; i < 6; i++)
            {
                double angle = i * angleStep;
                subjectPolygon.Add(new PointD(
                    100 * Math.Cos(angle),
                    100 * Math.Sin(angle)
                ));
            }

            // Polígono de recorte: Cuadrado rotado
            clipPolygon.Add(new PointD(-80, -80));
            clipPolygon.Add(new PointD(80, -80));
            clipPolygon.Add(new PointD(80, 80));
            clipPolygon.Add(new PointD(-80, 80));

            UpdateVertexLists();
            RedrawDisplay();
            lblStats.Text = "Ejemplo cargado:\nSujeto: Hexágono\nRecorte: Cuadrado";
        }

        private async void BtnClip_Click(object sender, EventArgs e)
        {
            if (subjectPolygon.Count < 3)
            {
                MessageBox.Show("El polígono sujeto necesita al menos 3 vértices.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (clipPolygon.Count < 3)
            {
                MessageBox.Show("El polígono de recorte necesita al menos 3 vértices.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnClip.Enabled = false;
            btnStop.Enabled = true;
            isAddingSubject = false;
            isAddingClip = false;

            cancellationTokenSource = new CancellationTokenSource();
            isAnimating = true;

            lstSteps.Items.Clear();
            resultPolygon.Clear();
            intersectionPoints.Clear();

            try
            {
                if (chkAnimate.Checked)
                {
                    await AnimateWeilerAtherton(cancellationTokenSource.Token);
                }
                else
                {
                    PerformClipping();
                }
            }
            catch (OperationCanceledException)
            {
                lblStats.Text = "Animación cancelada";
            }
            finally
            {
                isAnimating = false;
                btnClip.Enabled = true;
                btnStop.Enabled = false;
                cancellationTokenSource?.Dispose();
                cancellationTokenSource = null;
            }
        }

        private async Task AnimateWeilerAtherton(CancellationToken token)
        {
            int delay = GetDelayFromTrackbar();

            var steps = CRecortarPoligono.TraceWeilerAtherton(subjectPolygon, clipPolygon);

            foreach (var step in steps)
            {
                token.ThrowIfCancellationRequested();

                string message = "";

                switch (step.Action)
                {
                    case WeilerAthertonStep.StepAction.Start:
                        message = step.Message;
                        break;

                    case WeilerAthertonStep.StepAction.FindIntersections:
                        message = step.Message;

                        // Extraer punto de intersección del mensaje si es posible
                        if (chkShowIntersections.Checked)
                        {
                            RedrawDisplay();
                        }
                        break;

                    case WeilerAthertonStep.StepAction.InsertIntersections:
                        message = step.Message;
                        break;

                    case WeilerAthertonStep.StepAction.MarkEntryExit:
                        message = step.Message;
                        break;

                    case WeilerAthertonStep.StepAction.Traverse:
                        message = step.Message;
                        break;

                    case WeilerAthertonStep.StepAction.FinalResult:
                        message = step.Message;
                        if (step.ResultSoFar != null && step.ResultSoFar.Count > 0)
                        {
                            resultPolygon = new List<PointD>(step.ResultSoFar);
                            RedrawDisplay();
                        }
                        break;
                }

                lstSteps.Items.Add(message);
                if (lstSteps.Items.Count > 0)
                    lstSteps.SelectedIndex = lstSteps.Items.Count - 1;

                lblStats.Text = message;

                await Task.Delay(delay, token);
            }
        }

        private void PerformClipping()
        {
            lstSteps.Items.Clear();

            var steps = CRecortarPoligono.TraceWeilerAtherton(subjectPolygon, clipPolygon);

            foreach (var step in steps)
            {
                lstSteps.Items.Add(step.Message);

                if (step.Action == WeilerAthertonStep.StepAction.FinalResult && step.ResultSoFar != null)
                {
                    resultPolygon = new List<PointD>(step.ResultSoFar);
                    lblStats.Text = $"✓ Recorte completo: {resultPolygon.Count} vértices";
                }
            }

            RedrawDisplay();
        }

        private int GetDelayFromTrackbar()
        {
            int value = trkSpeed.Value;
            if (value <= 10) return 2000;
            if (value <= 25) return 1500;
            if (value <= 50) return 1000;
            if (value <= 75) return 500;
            if (value <= 90) return 250;
            return 100;
        }

        private void TrkSpeed_ValueChanged(object sender, EventArgs e)
        {
            int value = trkSpeed.Value;
            if (value <= 10)
                lblSpeedValue.Text = "Muy Lenta";
            else if (value <= 25)
                lblSpeedValue.Text = "Lenta";
            else if (value <= 50)
                lblSpeedValue.Text = "Media";
            else if (value <= 75)
                lblSpeedValue.Text = "Rápida";
            else if (value <= 90)
                lblSpeedValue.Text = "Muy Rápida";
            else
                lblSpeedValue.Text = "Máxima";
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            subjectPolygon.Clear();
            clipPolygon.Clear();
            resultPolygon.Clear();
            intersectionPoints.Clear();
            UpdateVertexLists();
            RedrawDisplay();
            lstSteps.Items.Clear();
            lblStats.Text = "Todo limpiado. Listo para comenzar.";
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            cancellationTokenSource?.Cancel();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            cancellationTokenSource?.Cancel();
            gfx?.Dispose();
            displayGfx?.Dispose();
            canvas?.Dispose();
            displayBitmap?.Dispose();
            base.OnFormClosing(e);
        }

        private void WeilerAthertonForm_Load(object sender, EventArgs e)
        {

        }
    }
}