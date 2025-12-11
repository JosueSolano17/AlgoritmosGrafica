using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinAPPLineas.Clases;


namespace WinAPPLineas.Formularios
{
    public partial class LiangBarskyForm : Form
    {
        private PictureBox picCanvas;
        private Panel pnlControls;
        private GroupBox grpLine;
        private GroupBox grpClipWindow;
        private GroupBox grpAnimation;
        private GroupBox grpVisualization;
        private NumericUpDown numX0, numY0, numX1, numY1;
        private NumericUpDown numXMin, numYMin, numXMax, numYMax;
        private Label lblX0, lblY0, lblX1, lblY1;
        private Label lblXMin, lblYMin, lblXMax, lblYMax;
        private Label lblInstructions;
        private Label lblStats;
        private Button btnDrawLine;
        private Button btnClip;
        private Button btnClear;
        private Button btnStop;
        private CheckBox chkAnimate;
        private CheckBox chkShowParameters;
        private CheckBox chkShowTests;
        private CheckBox chkShowGrid;
        private TrackBar trkSpeed;
        private Label lblSpeed;
        private Label lblSpeedValue;
        private ListBox lstSteps;

        private Bitmap canvas;
        private Bitmap displayBitmap;
        private Graphics gfx;
        private Graphics displayGfx;

        private Color lineColor = Color.FromArgb(3, 169, 244); // Azul claro
        private Color clippedLineColor = Color.FromArgb(76, 175, 80); // Verde
        private Color rejectedLineColor = Color.FromArgb(244, 67, 54); // Rojo
        private Color windowColor = Color.FromArgb(233, 30, 99); // Rosa
        private Color u0Color = Color.FromArgb(0, 150, 136); // Teal
        private Color u1Color = Color.FromArgb(255, 152, 0); // Naranja
        private Color gridColor = Color.FromArgb(200, 200, 200);
        private Color bgColor = Color.FromArgb(250, 250, 250);
        private Color testColor = Color.FromArgb(103, 58, 183); // Púrpura

        private bool isAnimating = false;
        private CancellationTokenSource cancellationTokenSource;

        private double lineX0, lineY0, lineX1, lineY1;
        private double clipXMin, clipYMin, clipXMax, clipYMax;

        private const int CANVAS_WIDTH = 600;
        private const int CANVAS_HEIGHT = 600;
        private const int GRID_SIZE = 20;

        public LiangBarskyForm()
        {
            InitializeComponent();
            InitializeCustomComponents();
            SetupCanvas();
        }

        private void InitializeCustomComponents()
        {
            this.Text = "Liang-Barsky - Recorte de Líneas";
            this.Size = new Size(1000, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 245, 245);

            // Panel de controles
            pnlControls = new Panel
            {
                Dock = DockStyle.Right,
                Width = 320,
                BackColor = Color.FromArgb(33, 33, 33),
                Padding = new Padding(10),
                AutoScroll = true
            };

            // GroupBox de línea
            grpLine = new GroupBox
            {
                Text = "Coordenadas de la Línea",
                Location = new Point(10, 10),
                Size = new Size(300, 120),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(224, 224, 224)
            };

            lblX0 = CreateLabel("X0:", 15, 25);
            numX0 = CreateNumericUpDown(80, 23, -300, 300, 50);

            lblY0 = CreateLabel("Y0:", 160, 25);
            numY0 = CreateNumericUpDown(190, 23, -300, 300, 50);

            lblX1 = CreateLabel("X1:", 15, 60);
            numX1 = CreateNumericUpDown(80, 58, -300, 300, 250);

            lblY1 = CreateLabel("Y1:", 160, 60);
            numY1 = CreateNumericUpDown(190, 58, -300, 300, 250);

            btnDrawLine = new Button
            {
                Text = "📏 Dibujar Línea",
                Location = new Point(15, 90),
                Size = new Size(270, 25),
                BackColor = Color.FromArgb(3, 169, 244),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnDrawLine.FlatAppearance.BorderSize = 0;
            btnDrawLine.Click += BtnDrawLine_Click;

            grpLine.Controls.AddRange(new Control[] { lblX0, numX0, lblY0, numY0, lblX1, numX1, lblY1, numY1, btnDrawLine });

            // GroupBox de ventana de recorte
            grpClipWindow = new GroupBox
            {
                Text = "Ventana de Recorte",
                Location = new Point(10, 140),
                Size = new Size(300, 120),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(224, 224, 224)
            };

            lblXMin = CreateLabel("XMin:", 15, 25);
            numXMin = CreateNumericUpDown(80, 23, -300, 300, 100);

            lblYMin = CreateLabel("YMin:", 160, 25);
            numYMin = CreateNumericUpDown(225, 23, -300, 300, 100);

            lblXMax = CreateLabel("XMax:", 15, 60);
            numXMax = CreateNumericUpDown(80, 58, -300, 300, 200);

            lblYMax = CreateLabel("YMax:", 160, 60);
            numYMax = CreateNumericUpDown(225, 58, -300, 300, 200);

            Button btnSetWindow = new Button
            {
                Text = "🔲 Establecer Ventana",
                Location = new Point(15, 90),
                Size = new Size(270, 25),
                BackColor = Color.FromArgb(233, 30, 99),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSetWindow.FlatAppearance.BorderSize = 0;
            btnSetWindow.Click += (s, e) => RedrawDisplay();

            grpClipWindow.Controls.AddRange(new Control[] { lblXMin, numXMin, lblYMin, numYMin, lblXMax, numXMax, lblYMax, numYMax, btnSetWindow });

            // GroupBox de animación
            grpAnimation = new GroupBox
            {
                Text = "Animación",
                Location = new Point(10, 270),
                Size = new Size(300, 110),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(224, 224, 224)
            };

            chkAnimate = new CheckBox
            {
                Text = "Activar animación paso a paso",
                Location = new Point(15, 25),
                Size = new Size(270, 25),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(224, 224, 224),
                Checked = true
            };

            lblSpeed = new Label
            {
                Text = "Velocidad:",
                Location = new Point(15, 55),
                Size = new Size(80, 20),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(224, 224, 224)
            };

            lblSpeedValue = new Label
            {
                Text = "Media",
                Location = new Point(220, 55),
                Size = new Size(65, 20),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(3, 169, 244),
                TextAlign = ContentAlignment.MiddleRight
            };

            trkSpeed = new TrackBar
            {
                Location = new Point(15, 75),
                Size = new Size(270, 30),
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
                Location = new Point(10, 390),
                Size = new Size(300, 120),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(224, 224, 224)
            };

            chkShowParameters = new CheckBox
            {
                Text = "Mostrar parámetros u0 y u1",
                Location = new Point(15, 25),
                Size = new Size(270, 25),
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(224, 224, 224),
                Checked = true
            };

            chkShowTests = new CheckBox
            {
                Text = "Mostrar pruebas p y q",
                Location = new Point(15, 55),
                Size = new Size(270, 25),
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(224, 224, 224),
                Checked = true
            };

            chkShowGrid = new CheckBox
            {
                Text = "Mostrar cuadrícula",
                Location = new Point(15, 85),
                Size = new Size(270, 25),
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(224, 224, 224),
                Checked = true
            };
            chkShowGrid.CheckedChanged += (s, e) => RedrawDisplay();

            grpVisualization.Controls.AddRange(new Control[] { chkShowParameters, chkShowTests, chkShowGrid });

            // Lista de pasos
            Label lblStepsTitle = new Label
            {
                Text = "Pasos del Algoritmo:",
                Location = new Point(10, 520),
                Size = new Size(300, 20),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(224, 224, 224)
            };

            lstSteps = new ListBox
            {
                Location = new Point(10, 545),
                Size = new Size(300, 120),
                Font = new Font("Consolas", 8F),
                BackColor = Color.FromArgb(48, 48, 48),
                ForeColor = Color.FromArgb(224, 224, 224),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Label de estadísticas
            lblStats = new Label
            {
                Text = "Estado: Listo",
                Location = new Point(10, 675),
                Size = new Size(300, 40),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(224, 224, 224),
                BackColor = Color.FromArgb(48, 48, 48),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(5),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Botones de acción
            btnClip = new Button
            {
                Text = "✂ Recortar (Liang-Barsky)",
                Location = new Point(10, 725),
                Size = new Size(300, 35),
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnClip.FlatAppearance.BorderSize = 0;
            btnClip.Click += BtnClip_Click;

            btnClear = new Button
            {
                Text = "🗑 Limpiar",
                Location = new Point(10, 770),
                Size = new Size(145, 35),
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
                Location = new Point(165, 770),
                Size = new Size(145, 35),
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
                Text = "📋 INSTRUCCIONES:\n\n" +
                       "1. Define las coordenadas\n" +
                       "   de la línea\n" +
                       "2. Establece la ventana\n" +
                       "   de recorte\n" +
                       "3. Dibuja la línea\n" +
                       "4. Presiona 'Recortar'\n" +
                       "5. Observa la animación\n\n" +
                       "Liang-Barsky usa parámetros\n" +
                       "u0 y u1 para calcular la\n" +
                       "porción visible de la línea\n" +
                       "mediante pruebas p y q.",
                Location = new Point(10, 815),
                Size = new Size(300, 240),
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(224, 224, 224),
                BackColor = Color.FromArgb(48, 48, 48),
                Padding = new Padding(8),
                BorderStyle = BorderStyle.FixedSingle
            };

            pnlControls.Controls.AddRange(new Control[] {
                grpLine, grpClipWindow, grpAnimation, grpVisualization,
                lblStepsTitle, lstSteps, lblStats,
                btnClip, btnClear, btnStop, lblInstructions
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

            this.Controls.Add(picCanvas);
            this.Controls.Add(pnlControls);
        }

        private Label CreateLabel(string text, int x, int y)
        {
            return new Label
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(60, 20),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(224, 224, 224)
            };
        }

        private NumericUpDown CreateNumericUpDown(int x, int y, int min, int max, int value)
        {
            return new NumericUpDown
            {
                Location = new Point(x, y),
                Size = new Size(65, 25),
                Minimum = min,
                Maximum = max,
                Value = value,
                Font = new Font("Segoe UI", 9F)
            };
        }

        private void SetupCanvas()
        {
            canvas = new Bitmap(CANVAS_WIDTH, CANVAS_HEIGHT);
            displayBitmap = new Bitmap(CANVAS_WIDTH, CANVAS_HEIGHT);

            gfx = Graphics.FromImage(canvas);
            displayGfx = Graphics.FromImage(displayBitmap);

            gfx.SmoothingMode = SmoothingMode.AntiAlias;
            displayGfx.SmoothingMode = SmoothingMode.AntiAlias;

            clipXMin = (double)numXMin.Value;
            clipYMin = (double)numYMin.Value;
            clipXMax = (double)numXMax.Value;
            clipYMax = (double)numYMax.Value;

            RedrawDisplay();
        }

        private void RedrawDisplay()
        {
            displayGfx.Clear(bgColor);

            clipXMin = (double)numXMin.Value;
            clipYMin = (double)numYMin.Value;
            clipXMax = (double)numXMax.Value;
            clipYMax = (double)numYMax.Value;

            // Dibujar cuadrícula
            if (chkShowGrid.Checked)
            {
                DrawGrid();
            }

            // Dibujar ejes
            DrawAxes();

            // Dibujar ventana de recorte
            DrawClipWindow();

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
                // Eje X
                displayGfx.DrawLine(axisPen, 0, centerY, CANVAS_WIDTH, centerY);
                // Eje Y
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

        private void DrawClipWindow()
        {
            PointF p1 = WorldToScreen(clipXMin, clipYMax);
            PointF p2 = WorldToScreen(clipXMax, clipYMin);

            float width = p2.X - p1.X;
            float height = p2.Y - p1.Y;

            using (Pen windowPen = new Pen(windowColor, 3))
            {
                windowPen.DashStyle = DashStyle.Dash;
                displayGfx.DrawRectangle(windowPen, p1.X, p1.Y, width, height);
            }

            // Rellenar con transparencia
            using (SolidBrush fillBrush = new SolidBrush(Color.FromArgb(20, windowColor)))
            {
                displayGfx.FillRectangle(fillBrush, p1.X, p1.Y, width, height);
            }

            // Etiquetas de los bordes
            if (chkShowTests.Checked)
            {
                using (Font font = new Font("Segoe UI", 9, FontStyle.Bold))
                using (SolidBrush brush = new SolidBrush(testColor))
                {
                    displayGfx.DrawString("p=-dx, q=x0-xmin", font, brush, p1.X - 130, (p1.Y + p2.Y) / 2 - 20);
                    displayGfx.DrawString("p=dx, q=xmax-x0", font, brush, p2.X + 10, (p1.Y + p2.Y) / 2 - 20);
                    displayGfx.DrawString("p=-dy, q=y0-ymin", font, brush, (p1.X + p2.X) / 2 - 60, p2.Y + 10);
                    displayGfx.DrawString("p=dy, q=ymax-y0", font, brush, (p1.X + p2.X) / 2 - 60, p1.Y - 25);
                }
            }
        }

        private PointF WorldToScreen(double x, double y)
        {
            int centerX = CANVAS_WIDTH / 2;
            int centerY = CANVAS_HEIGHT / 2;

            float screenX = centerX + (float)x;
            float screenY = centerY - (float)y;

            return new PointF(screenX, screenY);
        }

        private void BtnDrawLine_Click(object sender, EventArgs e)
        {
            lineX0 = (double)numX0.Value;
            lineY0 = (double)numY0.Value;
            lineX1 = (double)numX1.Value;
            lineY1 = (double)numY1.Value;

            RedrawDisplay();
            DrawLine(lineX0, lineY0, lineX1, lineY1, lineColor, 2);

            picCanvas.Image = displayBitmap;
            picCanvas.Refresh();

            lblStats.Text = $"Línea dibujada: ({lineX0},{lineY0}) → ({lineX1},{lineY1})";
        }

        private void DrawLine(double x0, double y0, double x1, double y1, Color color, float width)
        {
            PointF p0 = WorldToScreen(x0, y0);
            PointF p1 = WorldToScreen(x1, y1);

            using (Pen pen = new Pen(color, width))
            {
                displayGfx.DrawLine(pen, p0, p1);
            }

            // Dibujar puntos extremos
            DrawPoint(x0, y0, color, 6);
            DrawPoint(x1, y1, color, 6);
        }

        private void DrawPoint(double x, double y, Color color, float size)
        {
            PointF p = WorldToScreen(x, y);

            using (SolidBrush brush = new SolidBrush(color))
            {
                displayGfx.FillEllipse(brush, p.X - size / 2, p.Y - size / 2, size, size);
            }

            using (Pen pen = new Pen(Color.White, 1))
            {
                displayGfx.DrawEllipse(pen, p.X - size / 2, p.Y - size / 2, size, size);
            }
        }

        private async void BtnClip_Click(object sender, EventArgs e)
        {
            btnClip.Enabled = false;
            btnDrawLine.Enabled = false;
            btnStop.Enabled = true;

            cancellationTokenSource = new CancellationTokenSource();
            isAnimating = true;

            lstSteps.Items.Clear();

            try
            {
                if (chkAnimate.Checked)
                {
                    await AnimateLiangBarsky(cancellationTokenSource.Token);
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
                btnDrawLine.Enabled = true;
                btnStop.Enabled = false;
                cancellationTokenSource?.Dispose();
                cancellationTokenSource = null;
            }
        }

        private async Task AnimateLiangBarsky(CancellationToken token)
        {
            RedrawDisplay();
            DrawLine(lineX0, lineY0, lineX1, lineY1, lineColor, 2);

            int delay = GetDelayFromTrackbar();

            var steps = CRecortarlineas.TraceLiangBarsky(
                lineX0, lineY0, lineX1, lineY1,
                clipXMin, clipYMin, clipXMax, clipYMax
            );

            double currentU0 = 0.0;
            double currentU1 = 1.0;

            foreach (var step in steps)
            {
                token.ThrowIfCancellationRequested();

                string message = "";

                switch (step.Action)
                {
                    case LiangBarskyStep.StepAction.Start:
                        message = $"Inicio Liang-Barsky: P0=({step.X0:F1},{step.Y0:F1}) P1=({step.X1:F1},{step.Y1:F1})";
                        if (chkShowParameters.Checked)
                        {
                            DrawParameterLabels(0.0, 1.0);
                        }
                        break;

                    case LiangBarskyStep.StepAction.ParamTest:
                        message = step.Message;
                        if (chkShowTests.Checked)
                        {
                            DrawTestInfo(step.P, step.Q);
                        }
                        break;

                    case LiangBarskyStep.StepAction.UpdateU:
                        currentU0 = step.U0;
                        currentU1 = step.U1;
                        message = step.Message;

                        if (chkShowParameters.Checked)
                        {
                            RedrawDisplay();
                            DrawLine(lineX0, lineY0, lineX1, lineY1, Color.LightGray, 1);
                            DrawParameterSegment(currentU0, currentU1);
                            DrawParameterLabels(currentU0, currentU1);
                        }
                        break;

                    case LiangBarskyStep.StepAction.Reject:
                        message = $"✗ RECHAZADA: {step.Message}";
                        DrawLine(lineX0, lineY0, lineX1, lineY1, rejectedLineColor, 2);
                        break;

                    case LiangBarskyStep.StepAction.FinalAccepted:
                        message = $"✓ ACEPTADA: ({step.X0:F1},{step.Y0:F1}) → ({step.X1:F1},{step.Y1:F1})";
                        RedrawDisplay();
                        DrawLine(lineX0, lineY0, lineX1, lineY1, Color.LightGray, 1);
                        DrawLine(step.X0, step.Y0, step.X1, step.Y1, clippedLineColor, 3);
                        if (chkShowParameters.Checked)
                        {
                            DrawParameterLabels(currentU0, currentU1);
                        }
                        break;

                    case LiangBarskyStep.StepAction.FinalRejected:
                        message = $"✗ RECHAZADA: {step.Message}";
                        DrawLine(lineX0, lineY0, lineX1, lineY1, rejectedLineColor, 2);
                        break;
                }

                lstSteps.Items.Add(message);
                lstSteps.SelectedIndex = lstSteps.Items.Count - 1;
                lblStats.Text = message;

                picCanvas.Image = displayBitmap;
                picCanvas.Refresh();

                await Task.Delay(delay, token);
            }
        }

        private void DrawTestInfo(double p, double q)
        {
            using (Font font = new Font("Consolas", 10, FontStyle.Bold))
            using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(200, 103, 58, 183)))
            using (SolidBrush textBrush = new SolidBrush(Color.White))
            {
                string text = $"p={p:F2}, q={q:F2}";
                SizeF size = displayGfx.MeasureString(text, font);
                RectangleF rect = new RectangleF(10, 10, size.Width + 10, size.Height + 5);
                displayGfx.FillRectangle(bgBrush, rect);
                displayGfx.DrawString(text, font, textBrush, 15, 12);
            }
        }

        private void DrawParameterSegment(double u0, double u1)
        {
            double x0 = lineX0 + u0 * (lineX1 - lineX0);
            double y0 = lineY0 + u0 * (lineY1 - lineY0);
            double x1 = lineX0 + u1 * (lineX1 - lineX0);
            double y1 = lineY0 + u1 * (lineY1 - lineY0);

            DrawLine(x0, y0, x1, y1, Color.FromArgb(255, 193, 7), 3);
            DrawPoint(x0, y0, u0Color, 8);
            DrawPoint(x1, y1, u1Color, 8);
        }

        private void DrawParameterLabels(double u0, double u1)
        {
            double x0 = lineX0 + u0 * (lineX1 - lineX0);
            double y0 = lineY0 + u0 * (lineY1 - lineY0);
            double x1 = lineX0 + u1 * (lineX1 - lineX0);
            double y1 = lineY0 + u1 * (lineY1 - lineY0);

            PointF p0 = WorldToScreen(x0, y0);
            PointF p1 = WorldToScreen(x1, y1);

            using (Font font = new Font("Consolas", 9, FontStyle.Bold))
            {
                using (SolidBrush brush = new SolidBrush(u0Color))
                {
                    displayGfx.DrawString($"u0={u0:F3}", font, brush, p0.X + 10, p0.Y - 20);
                }

                using (SolidBrush brush = new SolidBrush(u1Color))
                {
                    displayGfx.DrawString($"u1={u1:F3}", font, brush, p1.X + 10, p1.Y + 5);
                }
            }
        }

        private void PerformClipping()
        {
            RedrawDisplay();

            double x0 = lineX0, y0 = lineY0;
            double x1 = lineX1, y1 = lineY1;

            DrawLine(x0, y0, x1, y1, Color.LightGray, 1);

            bool accepted = CRecortarlineas.LiangBarskyClip(
                ref x0, ref y0, ref x1, ref y1,
                clipXMin, clipYMin, clipXMax, clipYMax
            );

            if (accepted)
            {
                DrawLine(x0, y0, x1, y1, clippedLineColor, 3);
                lblStats.Text = $"✓ Línea recortada: ({x0:F1},{y0:F1}) → ({x1:F1},{y1:F1})";
                lstSteps.Items.Add("Línea aceptada y recortada");
            }
            else
            {
                DrawLine(lineX0, lineY0, lineX1, lineY1, rejectedLineColor, 2);
                lblStats.Text = "✗ Línea rechazada (completamente fuera)";
                lstSteps.Items.Add("Línea rechazada");
            }

            picCanvas.Image = displayBitmap;
            picCanvas.Refresh();
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
            RedrawDisplay();
            lstSteps.Items.Clear();
            lblStats.Text = "Estado: Listo";
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

        private void LiangBarskyForm_Load(object sender, EventArgs e)
        {

        }
    }
}